namespace RailSim.Model
{
    internal class TaggedEdge<TVertex, TTag> : Edge<TVertex>, ITagged<TTag>
        where TVertex : notnull
    {
        public TTag Tag { get; }

        public TaggedEdge(TVertex from, TVertex to, TTag tag)
            : base(from, to)
        {
            Tag = tag;
        }
    }
}
