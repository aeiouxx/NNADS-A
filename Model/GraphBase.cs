using System.Text;

namespace RailSim.Model
{
    public class Graph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : notnull
    {
        #region Nested classes
        protected class Vertex
        {
            public TVertex Data
            { get; set; }
        }
        protected class Edge
        {
            public Vertex To
            { get; set; }
            public TEdge Data
            { get; set; }

            public Edge(Graph<TVertex, TEdge>.Vertex to, TEdge data)
            {
                To = to;
                Data = data;
            }
        }
        #endregion
        #region Fields
        private readonly IEqualityComparer<TVertex> _vertexComparer;
        private Dictionary<Vertex, List<Edge>> _adjacencyList = new();
        private Dictionary<TVertex, Vertex> _verticesLookup = new();
        #endregion
        /// <summary>
        /// Creates a new instance of <see cref="Graph{TVertex, TEdge}"/>
        /// </summary>
        /// <param name="customVertexComparer">Will use <see cref="EqualityComparer{TVertex}.Default"/> if <see langword="null"/></param>
        public Graph(IEqualityComparer<TVertex>? customVertexComparer = null)
        {
            _vertexComparer = customVertexComparer ?? EqualityComparer<TVertex>.Default;
        }

        #region Manipulation
        public bool AddVertex(TVertex data)
        {
            if (_verticesLookup.ContainsKey(data))
            {
                return false;
            }
            var vertex = new Vertex { Data = data };
            _verticesLookup[data] = vertex;
            _adjacencyList[vertex] = new List<Edge>();
            return true;
        }
        public bool RemoveVertex(TVertex vertex)
        {
            if (!_verticesLookup.TryGetValue(vertex, out var vertexToRemove))
            {
                return false;
            }
            _adjacencyList.Remove(vertexToRemove);
            _verticesLookup.Remove(vertex);
            foreach (var edges in _adjacencyList.Values)
            {
                edges.RemoveAll(e => _vertexComparer.Equals(e.To.Data, vertex));
            }
            return true;
        }

        public bool AddEdge(TVertex from, TVertex to, TEdge data)
        {
            if (!_verticesLookup.TryGetValue(from, out var fromVertex) || !_verticesLookup.TryGetValue(to, out var toVertex))
            {
                return false;
            }
            _adjacencyList[fromVertex].Add(new Edge(toVertex, data));
            return true;
        }

        public bool RemoveEdge(TVertex from, TVertex to, TEdge data)
        {
            if (!_verticesLookup.TryGetValue(from, out var fromVertex))
            {
                return false;
            }
            var edges = _adjacencyList[fromVertex];
            var edgeToRemove = edges.FirstOrDefault(e => _vertexComparer.Equals(e.To.Data, to) && e.Data.Equals(data));
            if (edgeToRemove == null)
            {
                return false;
            }
            return edges.Remove(edgeToRemove);
        }
        public int RemoveEdges(TVertex from, TVertex to)
        {
            if (!_verticesLookup.TryGetValue(from, out var fromVertex))
            {
                return 0;
            }
            return _adjacencyList[fromVertex]?.RemoveAll(e => _vertexComparer.Equals(e.To.Data, to)) ?? 0;
        }
        #endregion

        public string GetFormattedGraph()
        {
            StringBuilder sb = new();
            foreach (var vertex in _adjacencyList.Keys)
            {
                sb.Append(vertex.Data);
                sb.Append(" -> ");
                foreach (var edge in _adjacencyList[vertex])
                {
                    sb.Append(edge.To.Data);
                    sb.Append(" (");
                    sb.Append(edge.Data);
                    sb.Append(") ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        #region Pathfinding

        public List<List<TVertex>> FindAllPaths(TVertex from, TVertex to)
        {
            return FindAllPaths(new[] { from }, new[] { to });
        }
        public List<List<TVertex>> FindAllPaths(IEnumerable<TVertex> from, IEnumerable<TVertex> to)
        {
            var paths = new List<List<TVertex>>();
            var endVertices = new HashSet<TVertex>(to, _vertexComparer);

            foreach (var start in from)
            {
                if (_verticesLookup.TryGetValue(start, out var startVertex))
                {
                    FindAllPathsRecursive(startVertex, endVertices, paths, new List<TVertex>());
                }
            }
            return paths;
        }

        private void FindAllPathsRecursive(Vertex current, HashSet<TVertex> to, List<List<TVertex>> paths, List<TVertex> currentPath)
        {
            currentPath.Add(current.Data);
            if (to.Contains(current.Data))
            {
                paths.Add(new List<TVertex>(currentPath));
            }


            if (_adjacencyList.TryGetValue(current, out var edges))
            {
                foreach (var edge in edges)
                {
                    if (!currentPath.Contains(edge.To.Data))
                    {
                        FindAllPathsRecursive(edge.To, to, paths, currentPath);
                    }
                }
            }
            // backtracking
            currentPath.RemoveAt(currentPath.Count - 1);
        }
        #endregion
    }

}
