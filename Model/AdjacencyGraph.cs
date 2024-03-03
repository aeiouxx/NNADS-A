namespace RailSim.Model
{
    public class AdjacencyGraph<TVertex, TEdge>
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
        public bool AddEdge(TVertex from, TVertex to, Func<TEdge> factory)
        {
            if (_adjacencyList.ContainsKey(from) && _adjacencyList.ContainsKey(to))
            {
                _adjacencyList[from].Add(factory());
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

        public void Print()
        {
            foreach (var vertex in Vertices)
            {
                Console.WriteLine(vertex);
                foreach (var edge in _adjacencyList[vertex])
                {
                    Console.WriteLine($"\t{edge}");
                }
            }
        }

        public IEnumerable<IEnumerable<TVertex>> FindPaths(
            IEnumerable<TVertex> startVertices,
            IEnumerable<TVertex> endVertices)
        {
            var paths = new List<IEnumerable<TVertex>>();
            foreach (var vertex in startVertices)
            {
                var result = Lmao(vertex, endVertices).ToList();
                paths.Add(result.SelectMany(p => p));
            }

            return paths;
        }


        /// <summary>
        ///  Directed acyclic graph so we don't need to check for cycles.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="endVertices"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<TVertex>> Lmao(
            TVertex start,
            IEnumerable<TVertex> endVertices,
            TVertex? precursor = default)
        {
            Stack<TVertex> toVisit = new();
            toVisit.Push(start);
            var path = new List<TVertex>();
            while (toVisit.Count > 0)
            {
                var current = toVisit.Pop();
                var newPath = new List<TVertex>(path).Append(current);
                if (endVertices.Contains(current))
                {
                    yield return newPath;
                }
                else
                {
                    foreach (var edge in GetOutgoingEdges(current))
                    {
                        switch (edge)
                        {
                            case GauntletEdge<TVertex> gauntletEdge:
                                if (precursor != null
                                    && gauntletEdge.TraversibleFor.HasValue
                                    && gauntletEdge.TraversibleFor.Value.Equals(precursor))
                                {
                                    toVisit.Push(gauntletEdge.To);
                                }
                                break;
                            case Edge<TVertex> simpleEdge:
                                toVisit.Push(simpleEdge.To);
                                break;
                        }
                    }
                    precursor = current;
                }
            }

        }
    }
}
