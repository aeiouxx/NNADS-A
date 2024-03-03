namespace RailSim.Model
{
    public class GauntletEdge<TVertex> : Edge<TVertex>,
        IGauntletEdge<TVertex>
        where TVertex : notnull
    {
        public Option<TVertex> TraversibleFor
        { get; protected set; }


        public GauntletEdge(TVertex from, TVertex to)
            : base(from, to)
        {
            TraversibleFor = Option<TVertex>.None;
        }

        public GauntletEdge(TVertex from, TVertex to, TVertex traversibleFor)
            : base(from, to)
        {
            TraversibleFor = traversibleFor;
        }

        public bool IsTraversibleFrom(TVertex vertex)
        {
            if (!TraversibleFor.HasValue)
            {
                return false;
            }

            return TraversibleFor.Value.Equals(vertex);
        }

        public override string ToString() => $"For {(TraversibleFor.HasValue ? TraversibleFor.Value : "undefined")}: {From} -> {To}";
    }
}
