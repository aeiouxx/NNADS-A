using RailSim.Model;
using RailSim.Model.Persistence;
using System.Text.Json;

namespace RailSim
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphFactory.Create<string>();

            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");
            graph.AddVertex("E");

            graph.AddEdge(new Edge<string>("A", "C"));
            graph.AddEdge(new Edge<string>("A", "D"));
            graph.AddEdge(new Edge<string>("B", "C"));
            graph.AddEdge(new Edge<string>("B", "E"));

            graph.AddEdge(new GauntletEdge<string>("C", "D", "B"));
            graph.AddEdge(new GauntletEdge<string>("C", "E", "A"));

            graph.Print();
            Console.ReadKey(intercept: true);

            var startingVertices = new[] { "A", "B" };
            var endVertices = new[] { "D", "E" };
            Console.WriteLine($"Searching paths from: {string.Join(" ", startingVertices)}");
            Console.WriteLine($"To: {string.Join(" ", endVertices)}");
            Console.WriteLine($"Found: ");
            var paths = graph.FindAllPathsRecursive(startingVertices, endVertices);
            foreach (var path in paths)
            {
                Console.WriteLine(string.Join(" -> ", path));
            }


            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new EdgeConverter<string>(),
                    new AdjacencyGraphConverter<string, IEdge<string>>()
                }
            };

            string jsonText = JsonSerializer.Serialize(graph, options);
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "graph.json");
            File.WriteAllText(filePath, jsonText);


            var parsedGraph = JsonSerializer.Deserialize<AdjacencyGraph<string, IEdge<string>>>(jsonText, options);
        }
    }
}