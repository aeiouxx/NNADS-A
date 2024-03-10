using System.Collections.Concurrent;

namespace RailSim.Model
{
    public static class AdjacencyGraphExtensions
    {
        public static List<List<Path<TVertex>>> FindAllDisjointTuples<TVertex>(this Graph<TVertex, IEdge<TVertex>> graph,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
        {
            var paths = FindAllPathsIterative(graph, startingVertices, endVertices).ToList();
            int maxTupleSize = Math.Min(startingVertices.Count(), endVertices.Count());
            bool[,] collisions = PrepareCollisionMatrix(paths);

            List<List<int>> tuples = new();
            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = i + 1; j < paths.Count; j++)
                {
                    if (!collisions[i, j])
                    {
                        tuples.Add(new List<int> { i, j });
                    }
                }
            }

            for (int tupleSize = 3; tupleSize <= maxTupleSize; tupleSize++)
            {
                ConcurrentBag<List<int>> newTuples = new();
                var toExpand = tuples.Where(t => t?.Count == tupleSize - 1).ToList();
                Parallel.ForEach(toExpand, tuple =>
                {
                    var lastInTuple = tuple.Last(); // Ensures we only look forward to avoid permutations
                    var candidateIndices = paths.Select((path, index) => index)
                                                .Where(index => index > lastInTuple && !tuple.Contains(index))
                                                .ToList();
                    foreach (var pathIndex in candidateIndices)
                    {
                        bool isDisjoint = tuple.All(index => !collisions[index, pathIndex]);
                        if (isDisjoint)
                        {
                            var newTuple = new List<int>(tuple) { pathIndex };
                            newTuples.Add(newTuple);
                        }
                    }
                });
                // If we cant create any n-tuples, we obviously wont be able to create any n+1-tuples.
                if (newTuples.Count == 0)
                {
                    break;
                }
                Console.WriteLine($"Tupling for {tupleSize}");
                Console.WriteLine($"Adding: {newTuples.Count}");
                tuples.AddRange(newTuples.Where(t => t != null));
                Console.WriteLine($"Total: {tuples.Count}");
            }

            var nulls = tuples.Where(t => t == null).ToList();
            return tuples.Select(tuple => tuple.Select(index => paths[index]).ToList()).ToList();
        }

        private static bool[,] PrepareCollisionMatrix<TVertex>(List<Path<TVertex>> paths) where TVertex : notnull
        {
            int count = paths.Count;
            var matrix = new bool[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    matrix[i, j] = !paths[i].IsDisjoint(paths[j]);
                    matrix[j, i] = matrix[i, j];
                }
            }
            return matrix;
        }

        public static IEnumerable<Path<TVertex>> FindAllPathsIterative<TVertex, TEdge>(this Graph<TVertex, TEdge> graph,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            int pathNumber = 1;
            List<Path<TVertex>> allPaths = new();
            HashSet<TVertex> goalSet = new(endVertices);
            Stack<(List<TVertex> Path, Option<TVertex> precursor)> pathStack = new();
            foreach (var startingVertex in startingVertices)
            {
                pathStack.Push((new List<TVertex> { startingVertex }, Option<TVertex>.None));
            }
            while (pathStack.Count > 0)
            {
                var (currentPath, lastVertex) = pathStack.Pop();
                TVertex current = currentPath[^1];
                // directed acyclic graph so the only way to have a "cycle" is a path of 1 vertex (start = goal)
                if (goalSet.Contains(current)
                    && currentPath.Count > 1)
                {
                    allPaths.Add(
                        new Path<TVertex>($"A{pathNumber++}",
                            new List<TVertex>(currentPath)));
                }

                foreach (var edge in graph.GetOutgoingEdges(current))
                {
                    if (edge is GauntletEdge<TVertex> gauntlet)
                    {
                        if (gauntlet.TraversibleFor.HasValue
                               && gauntlet.TraversibleFor.Value.Equals(lastVertex.Value))
                        {
                            pathStack.Push((new List<TVertex>(currentPath) { edge.To }, current));
                        }
                    }
                    else
                    {
                        pathStack.Push((new List<TVertex>(currentPath) { edge.To }, current));
                    }
                }
            }
            return allPaths;
        }
        public static IEnumerable<List<TVertex>> FindAllPathsRecursive<TVertex, TEdge>(this Graph<TVertex, TEdge> graph,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            List<List<TVertex>> allPaths = new();
            foreach (var startingVertex in startingVertices)
            {
                foreach (var endVertex in endVertices)
                {
                    FindPathsInternal(graph, startingVertex, endVertex, new List<TVertex>(), allPaths);
                }
            }
            return allPaths;
        }
        private static void FindPathsInternal<TVertex, TEdge>(Graph<TVertex, TEdge> graph,
            TVertex currentVertex,
            TVertex endVertex,
            List<TVertex> currentPath,
            List<List<TVertex>> allPaths)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            currentPath.Add(currentVertex);
            if (currentVertex.Equals(endVertex))
            {
                allPaths.Add(new List<TVertex>(currentPath));
            }
            else
            {
                foreach (var edge in graph.GetOutgoingEdges(currentVertex))
                {
                    TVertex? precursor = currentPath.Count > 1 ? currentPath[^2] : default;
                    if (precursor != null && edge is IGauntletEdge<TVertex> gauntletEdge)
                    {
                        if (gauntletEdge.TraversibleFor.HasValue
                            && gauntletEdge.TraversibleFor.Value.Equals(precursor))
                        {
                            FindPathsInternal(graph, edge.To, endVertex, currentPath, allPaths);
                        }
                    }
                    else
                    {
                        FindPathsInternal(graph, edge.To, endVertex, currentPath, allPaths);
                    }
                }
            }
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }
}
