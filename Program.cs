using RailSim.Model;

namespace RailSim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = new AdjacencyGraph<string, IEdge<string>>();

            graph.AddVertex("A");
            graph.AddVertex("B");

            graph.AddEdge(new Edge<string>("A", "B"));
        }
    }
}