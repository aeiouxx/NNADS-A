namespace RailSim.Model
{
    public interface IEdge<TVertex>
    {
        TVertex From
        { get; }
        TVertex To
        { get; }
    }
}
