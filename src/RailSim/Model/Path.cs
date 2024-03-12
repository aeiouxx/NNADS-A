namespace RailSim.Model
{
    public class Path<TVertex>
    {
        public string Identifier { get; set; }
        public List<TVertex> Vertices { get; init; }

        public Path(string identifier, List<TVertex> vertices)
        {
            Identifier = identifier;
            Vertices = vertices;
        }

        public bool IsDisjoint(Path<TVertex> other)
        {
            return !Vertices.Intersect(other.Vertices).Any();
        }

        public override string ToString()
        {
            return $"{Identifier}: {string.Join(" -> ", Vertices)}";
        }
    }
}
