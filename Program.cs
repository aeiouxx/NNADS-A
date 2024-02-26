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
            graph.AddEdge("A", "B", 17);
            graph.AddEdge("C", "D", 3);

            Console.WriteLine(graph.GetFormattedGraph());
            graph.RemoveEdges("A", "B");
            Console.WriteLine("Removed all edges from A");
            Console.WriteLine(graph.GetFormattedGraph());
        }
    }
}