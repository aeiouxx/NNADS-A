namespace RailSim.Model
{
    public interface IEdge<TVertex>
        where TVertex : notnull
    {
        TVertex From
        { get; }
        TVertex To
        { get; }
    }
}
