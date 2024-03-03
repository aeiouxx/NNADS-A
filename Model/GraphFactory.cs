namespace RailSim.Model
{
    public static class GraphFactory
    {
        public static AdjacencyGraph<T, IEdge<T>> Create<T>() where T : notnull => new();
    }
}
