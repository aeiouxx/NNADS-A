namespace RailSim.Model
{
    public static class AdjacencyGraphExtensions
    {
        public static List<List<List<TVertex>>> FindAllDisjointTuples<TVertex>(this Graph<TVertex, IEdge<TVertex>> graph,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
        {
            var allPaths = FindAllPathsIterative(graph, startingVertices, endVertices);
            var maxTupleSize = Math.Min(startingVertices.Count(), endVertices.Count());
            var validTuples = new List<List<List<TVertex>>>();
            for (int n = 2; n <= maxTupleSize; n++)
            {
                var combinations = GetCombinations(allPaths.ToList(), n, 0);

                foreach (var combo in combinations)
                {
                    if (ArePathsDisjoint(combo))
                    {
                        validTuples.Add(combo);
                    }
                }
            }
            return validTuples;
        }

        private static bool ArePathsDisjoint<TVertex>(List<List<TVertex>> combo) where TVertex : notnull
        {
            HashSet<TVertex> vertices = new();

            foreach (var path in combo)
            {
                foreach (var vertex in path)
                {
                    if (vertices.Contains(vertex))
                    {
                        return false;
                    }
                    vertices.Add(vertex);
                }
            }

            return true;
        }

        private static IEnumerable<List<List<TVertex>>> GetCombinations<TVertex>(List<List<TVertex>> paths, int length, int startIndex)
            where TVertex : notnull
        {
            if (length == 1)
            {
                for (int i = startIndex; i < paths.Count; i++)
                {
                    yield return new List<List<TVertex>> { paths[i] };
                }
            }

            else
            {
                for (int i = startIndex; i <= paths.Count - length; i++)
                {
                    foreach (var combo in GetCombinations(paths, length - 1, i + 1))
                    {
                        yield return new List<List<TVertex>> { paths[i] }.Concat(combo).ToList();
                    }
                }
            }
        }
        public static IEnumerable<List<TVertex>> FindAllPathsIterative<TVertex, TEdge>(this Graph<TVertex, TEdge> graph,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
            where TEdge : IEdge<TVertex>
        {
            List<List<TVertex>> allPaths = new();
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
                    allPaths.Add(new List<TVertex>(currentPath));
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
