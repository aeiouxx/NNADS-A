namespace RailSim.Model
{
    public class Graph<TVertex, TEdge>
        where TVertex : notnull
        where TEdge : IEdge<TVertex>
    {
        private static Predicate<TEdge> True => _ => true;
        private static Predicate<TEdge> False => _ => false;
        #region Fields
        private Dictionary<TVertex, List<TEdge>> _adjacencyList = new();
        #endregion
        #region Properties
        public IEnumerable<TVertex> Vertices => _adjacencyList.Keys;
        public IEnumerable<TEdge> Edges => _adjacencyList.Values.SelectMany(edgeList => edgeList);
        #endregion

        /// <summary>
        /// Adds a vertex to the graph if it doesn't already exist.
        /// </summary>
        public bool AddVertex(TVertex vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList.Add(vertex, new List<TEdge>());
                return true;
            }
            return false;
        }
        public bool ContainsVertex(TVertex vertex)
        {
            return _adjacencyList.ContainsKey(vertex);
        }
        /// <summary>
        /// Removes a vertex and all of it's incident edges from the graph.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns>The removed vertex if it was found, otherwise <see langword="null"/></returns>
        public bool RemoveVertex(TVertex vertex)
        {
            if (_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList.Remove(vertex);
                foreach (var edgeList in _adjacencyList.Values)
                {
                    edgeList.RemoveAll(edge => edge.From.Equals(vertex) || edge.To.Equals(vertex));
                }
                return true;
            }
            return false;
        }

        public bool AddEdge(TEdge edge)
        {
            if (_adjacencyList.ContainsKey(edge.From) && _adjacencyList.ContainsKey(edge.To))
            {
                _adjacencyList[edge.From].Add(edge);
                return true;
            }
            return false;
        }
        public bool AddEdge(TVertex from, TVertex to, Func<TVertex, TVertex, TEdge> factory)
        {
            if (_adjacencyList.ContainsKey(from) && _adjacencyList.ContainsKey(to))
            {
                _adjacencyList[from].Add(factory(from, to));
                return true;
            }

            return false;
        }
        public bool RemoveEdge(TEdge edge)
        {
            if (_adjacencyList.ContainsKey(edge.From) && _adjacencyList.ContainsKey(edge.To))
            {
                return _adjacencyList[edge.From].Remove(edge);
            }
            return false;
        }
        /// <summary>
        /// Doesn't remove the entries for gauntlet edge traversible for.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="filter">If left to be null, matches any edge</param>
        public bool RemoveEdge(TVertex from, TVertex to, Predicate<TEdge>? filter = null)
        {
            if (_adjacencyList.ContainsKey(from) && _adjacencyList.ContainsKey(to))
            {
                filter ??= True;
                return _adjacencyList[from]
                    .Where(edge => edge.From.Equals(from) && edge.To.Equals(to) && filter(edge))
                    .Any(edge => _adjacencyList[from]
                        .Remove(edge));
            }

            return false;
        }
        public IEnumerable<TEdge> GetIncidentEdges(TVertex vertex)
        {
            if (_adjacencyList.ContainsKey(vertex))
            {
                foreach (var edgeList in _adjacencyList.Values)
                {
                    foreach (var edge in edgeList)
                    {
                        if (edge.From.Equals(vertex) || edge.To.Equals(vertex))
                        {
                            yield return edge;
                        }
                    }
                }
            }

            yield break;
        }
        public IEnumerable<TEdge> GetIncomingEdges(TVertex vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                yield break;
            }

            foreach (var edgeList in _adjacencyList.Values)
            {
                foreach (var edge in edgeList)
                {
                    if (edge.To.Equals(vertex))
                    {
                        yield return edge;
                    }
                }
            }
        }
        public IEnumerable<TEdge> GetOutgoingEdges(TVertex vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                yield break;
            }
            foreach (var edge in _adjacencyList[vertex])
            {
                yield return edge;
            }
        }
    }
}
