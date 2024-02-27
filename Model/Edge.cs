using System.Diagnostics;

namespace RailSim.Model
{
    [DebuggerDisplay("{From} -> {To}")]
    public class Edge<TVertex> : IEdge<TVertex>
        where TVertex : notnull
    {
        public TVertex From { get; }
        public TVertex To { get; }

        public Edge(TVertex from, TVertex to)
        {
            From = from;
            To = to;
        }
    }
}
