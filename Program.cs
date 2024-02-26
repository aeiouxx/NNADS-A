using RailSim.Model;

namespace RailSim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph<string, int> graph = new();

            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");

            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "C", 2);
            graph.AddEdge("C", "D", 3);
            graph.AddEdge("B", "D", 44);
            graph.AddEdge("A", "D", 5);

            var paths = graph.FindAllPaths("A", "D");
        }
    }
}