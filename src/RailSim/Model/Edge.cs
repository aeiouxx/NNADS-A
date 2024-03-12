using System.Diagnostics;

namespace RailSim.Model
{
    [DebuggerDisplay("{From} -> {To}")]
    public class Edge<TVertex> : IEdge<TVertex>
        where TVertex : notnull
    {
        public TVertex From { get; protected set; }
        public TVertex To { get; protected set; }

        public Edge(TVertex from, TVertex to)
        {
            From = from;
            To = to;
        }

        public override string ToString() => $"{From} -> {To}";
    }
}
