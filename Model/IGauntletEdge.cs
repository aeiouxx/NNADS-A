namespace RailSim.Model
{
    internal interface IGauntletEdge<TVertex> : IEdge<TVertex>
        where TVertex : notnull
    {
        public Option<TVertex> TraversibleFor
        { get; }
    }
}
