namespace RailSim.Model
{
    public static class AdjacencyGraphExtensions
    {
        //public static IEnumerable<List<TVertex>> FindAllPathsIterative<TVertex, TEdge>(this AdjacencyGraph<TVertex, TEdge> graph,
        //    IEnumerable<TVertex> startingVertices,
        //    IEnumerable<TVertex> endVertices)
        //    where TVertex : notnull
        //    where TEdge : IEdge<TVertex>
        //{
        //    var allPaths = new List<List<TVertex>>();
        //    var stack = new Stack<(TVertex currentVertex, List<TVertex> Path, int? precursorIndex)>();
        //}
        public static IEnumerable<List<TVertex>> FindAllPathsRecursive<TVertex, TEdge>(this AdjacencyGraph<TVertex, TEdge> graph,
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
        private static void FindPathsInternal<TVertex, TEdge>(AdjacencyGraph<TVertex, TEdge> graph,
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
