using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RailSim.Model
{
    public static class GraphExtensions
    {
        /* Because the collision matrix is a symmetric matrix, we can use a single bit array to store the triangle of the matrix.
        * The diagonal also doesn't need to be stored, as it will always be true (a path always collides with itself).
        *  1 1 0 1    . . . .  for zero based indexing
        *  1 1 1 0 -> 1 . . .  can just store as a bit array of 101100 
        *  0 1 1 0    0 1 . .  (i, j) -> (i * (i - 1) / 2) + j 
        *  1 0 0 1    1 0 0 .  (2, 1) -> (2 * (2 - 1) / 2) + 1 = 2 => 101100[2] = 1
        *  Requires allocating C(n, 2) bits, instead of n^2
        */
        public class CollisionMatrix
        {
            private readonly BitArray _bits;
            private readonly int _dimension;
            public CollisionMatrix(int dimension)
            {
                if (dimension < 2)
                {
                    throw new ArgumentException("Dimension must be at least 2");
                }
                _dimension = dimension;
                int numberOfElements = dimension * (dimension - 1) / 2;
                _bits = new BitArray(numberOfElements);
            }
            public bool this[int i, int j]
            {
                get
                {
#if DEBUG
                    AssertValidIndex(i, j);
#endif
                    if (i == j)
                    {
                        return true;
                    }
                    if (i < j)
                    {
                        (i, j) = (j, i);
                    }
                    return _bits[GetIndex(i, j)];
                }
                set
                {
#if DEBUG
                    AssertValidIndex(i, j);
#endif
                    // we don't actually store the diagonal as it's always true
                    if (i == j)
                    {
                        return;
                    }
                    if (i < j)
                    {
                        (i, j) = (j, i);
                    }
                    _bits[GetIndex(i, j)] = value;
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private int GetIndex(int i, int j)
            {
                return (i * (i - 1) / 2) + j;
            }

            [Conditional("DEBUG")]
            public void AssertValidIndex(int i, int j)
            {
                Debug.Assert(i >= 0 && i < _dimension && j >= 0 && j < _dimension);
            }
        }
        public static List<List<Path<TVertex>>> FindAllDisjointTuples<TVertex>(this Graph<TVertex, IEdge<TVertex>> graph,
            List<Path<TVertex>> paths,
            IEnumerable<TVertex> startingVertices,
            IEnumerable<TVertex> endVertices)
            where TVertex : notnull
        {
            int maxTupleSize = Math.Min(startingVertices.Count(), endVertices.Count());
            // We can use a matrix to store the collision information between paths.
            // This avoids having to check multiple times whether a pair of paths is disjoint.
            var collisions = PrepareCollisionMatrix(paths, paths.Count);
            // Because we initialize our tuples in a strictly increasing order, we can avoid permutations and 
            // only look forward to avoid duplicates. This reduces the search space quite significantly.
            // ( i.e. if we have {0, 1} we don't have to worry about {1, 0} and can start searching from index of 2)
            // same applies for 3-tuples, etc.
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
            Console.WriteLine($"Adding {tuples.Count} 2-tuples.");
            for (int tupleSize = 3; tupleSize <= maxTupleSize; tupleSize++)
            {
                ConcurrentBag<List<int>> newTuples = new();
                var toExpand = tuples.Where(t => t?.Count == tupleSize - 1).ToList();
                Parallel.ForEach(toExpand, tuple =>
                {
                    // Because our tuples store indices in a strictly increasing order, we can
                    // resume the search from the last index in the tuple. While guaranteeing 
                    // that we don't create any duplicates nor combinations
                    var lastInTuple = tuple.Last();
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
                // If we can't create any n-tuples, we obviously wont be able to create any (n+1)-tuples.
                if (newTuples.Count == 0)
                {
                    break;
                }
                Console.WriteLine($"Adding {newTuples.Count} {tupleSize}-tuples");
                tuples.AddRange(newTuples);
            }

            Console.WriteLine($"Total: {tuples.Count}");
            return tuples.Select(tuple => tuple.Select(index => paths[index]).ToList()).ToList();
        }

        private static CollisionMatrix PrepareCollisionMatrix<TVertex>(List<Path<TVertex>> paths, int count)
            where TVertex : notnull
        {
            var matrix = new CollisionMatrix(count);
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    matrix[i, j] = !paths[i].IsDisjoint(paths[j]);
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
