using System.Text;

namespace RailSim.Model
{
    public class Graph<TVertex, TEdge>
    {
        #region Nested classes
        protected class Vertex
        {
            public TVertex Data
            { get; set; }
        }
        protected class Edge
        {
            public Vertex From
            { get; set; }
            public Vertex To
            { get; set; }
            public TEdge Data
            { get; set; }

            public Edge(Graph<TVertex, TEdge>.Vertex from, Graph<TVertex, TEdge>.Vertex to, TEdge data)
            {
                From = from;
                To = to;
                Data = data;
            }
        }
        #endregion
        private IEqualityComparer<TVertex> _vertexComparer;
        private IEqualityComparer<TEdge> _edgeComparer;

        // TODO: Need to rework this to use an adjacency list, this is terribly inefficient!!!
        private List<Vertex> _vertices = new();
        private List<Edge> _edges = new();
        public Graph() : this(null, null)
        {
        }
        public Graph(IEqualityComparer<TVertex>? vertexComparer, IEqualityComparer<TEdge>? edgeComparer)
        {
            _vertexComparer = vertexComparer ?? EqualityComparer<TVertex>.Default;
            _edgeComparer = edgeComparer ?? EqualityComparer<TEdge>.Default;
        }

        #region Manipulation
        public bool AddVertex(TVertex vertex)
        {
            if (ContainsVertex(vertex))
            {
                return false;
            }
            _vertices.Add(new Vertex { Data = vertex });
            return true;
        }

        private bool ContainsVertex(TVertex? vertex)
        {
            return _vertices.Any(v => _vertexComparer.Equals(v.Data, vertex));
        }

        public bool RemoveVertex(TVertex vertex)
        {
            var vertexToRemove = _vertices.FirstOrDefault(v => _vertexComparer.Equals(v.Data, vertex));
            if (vertexToRemove is null)
            {
                return false;
            }
            _vertices.Remove(vertexToRemove);
            _edges.Where(edge =>
                    _vertexComparer.Equals(edge.From.Data, vertex) ||
                    _vertexComparer.Equals(edge.To.Data, vertex))
                .ToList()
                .ForEach(e => _edges.Remove(e));
            return true;
        }

        public bool AddEdge(TVertex from, TVertex to, TEdge data)
        {
            var fromVertex = _vertices.FirstOrDefault(v => _vertexComparer.Equals(v.Data, from));
            var toVertex = _vertices.FirstOrDefault(v => _vertexComparer.Equals(v.Data, to));
            if (fromVertex == null || toVertex == null)
            {
                return false;
            }
            _edges.Add(new Edge(fromVertex, toVertex, data));
            return true;
        }

        public bool RemoveEdge(TVertex from, TVertex to, TEdge data)
        {
            var edgeToRemove = _edges.Where(e => _vertexComparer.Equals(e.From.Data, from) && _vertexComparer.Equals(e.To.Data, to))
                .FirstOrDefault(e => _edgeComparer.Equals(e.Data, data));
            if (edgeToRemove == null)
            {
                return false;
            }
            _edges.Remove(edgeToRemove);
            return true;
        }
        public bool RemoveEdges(TVertex from, TVertex to)
        {
            var edgesToRemove = _edges.Where(e => _vertexComparer.Equals(e.From.Data, from) && _vertexComparer.Equals(e.To.Data, to)).ToList();
            if (edgesToRemove.Count == 0)
            {
                return false;
            }
            edgesToRemove.ForEach(e => _edges.Remove(e));
            return true;
        }
        #endregion

        public string GetFormattedGraph()
        {
            StringBuilder sb = new();
            foreach (var vertex in _vertices)
            {
                sb.AppendLine($"{vertex.Data}");
                foreach (var edge in _edges.Where(e => e.From == vertex))
                {
                    sb.AppendLine($"\t->{edge.To.Data}: {edge.Data}");
                }
            }

            return sb.ToString();
        }
    }

}
