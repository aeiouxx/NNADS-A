using RailSim.Model;

namespace RailSim
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphFactory.Create<string>();
            graph.AddVertex("V23");
            graph.AddVertex("V21");
            graph.AddVertex("V22");
            graph.AddVertex("V24");
            graph.AddVertex("V12");
            graph.AddVertex("V14");
            graph.AddVertex("V30");
            graph.AddVertex("V17");
            graph.AddVertex("V29");
            graph.AddVertex("V18");
            graph.AddVertex("V27");
            graph.AddVertex("V13");
            graph.AddVertex("V15");
            graph.AddVertex("V16");
            graph.AddVertex("V19");
            graph.AddVertex("V28");

            graph.AddEdge(new Edge<string>("V23", "V12"));
            graph.AddEdge(new Edge<string>("V21", "V14"));
            graph.AddEdge(new Edge<string>("V22", "V15"));
            graph.AddEdge(new Edge<string>("V24", "V13"));
            graph.AddEdge(new Edge<string>("V13", "V15"));
            graph.AddEdge(new Edge<string>("V15", "V16"));
            graph.AddEdge(new Edge<string>("V12", "V14"));
            graph.AddEdge(new Edge<string>("V14", "V30"));
            graph.AddEdge(new Edge<string>("V30", "V17"));
            graph.AddEdge(new Edge<string>("V17", "V29"));
            graph.AddEdge(new Edge<string>("V29", "V18"));
            graph.AddEdge(new Edge<string>("V18", "V27"));
            graph.AddEdge(new Edge<string>("V18", "V19"));
            graph.AddEdge(new Edge<string>("V19", "V28"));
            graph.AddEdge(new Edge<string>("V16", "V19"));
            graph.AddEdge(new Edge<string>("V16", "V17"));


            var startingVertices = new[] { "V23", "V21", "V22", "V24", "V30", "V29" };
            var endVertices = new[] { "V30", "V29", "V27", "V28" };
            Console.WriteLine($"Searching paths from: {string.Join(" ", startingVertices)}");
            Console.WriteLine($"To: {string.Join(" ", endVertices)}");
            Console.WriteLine($"Found: ");
            var paths = graph.FindAllPathsRecursive(startingVertices, endVertices);
            foreach (var path in paths)
            {
                Console.WriteLine(string.Join(" -> ", path));
            }
            Console.WriteLine("---ITERATIVE FIND---");
            var paths2 = graph.FindAllPathsIterative(startingVertices, endVertices);
            foreach (var path in paths2)
            {
                Console.WriteLine(string.Join(" -> ", path));
            }
            Console.WriteLine("---DISJOINT TUPLES---");
            var result = graph.FindAllDisjointTuples(startingVertices, endVertices);
            //var options = new JsonSerializerOptions
            //{
            //    WriteIndented = true,
            //    Converters =
            //    {
            //        new EdgeConverter<string>(),
            //        new AdjacencyGraphConverter<string, IEdge<string>>()
            //    }
            //};
            //string jsonText = JsonSerializer.Serialize(graph, options);
            //var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "graph.json");
            //File.WriteAllText(filePath, jsonText);
            //var parsedGraph = JsonSerializer.Deserialize<AdjacencyGraph<string, IEdge<string>>>(jsonText, options);
        }
    }
}