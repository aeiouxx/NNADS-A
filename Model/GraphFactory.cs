namespace RailSim.Model
{
    public static class GraphFactory
    {
        public static Graph<T, IEdge<T>> Create<T>() where T : notnull => new();
    }
}
