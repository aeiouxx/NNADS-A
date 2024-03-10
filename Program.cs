using RailSim.Model;
using RailSim.Model.Persistence;
using System.Diagnostics;
using System.Text.Json;

namespace RailSim
{

    internal class Program
    {
        private class SearchData<TVertexKey>
           where TVertexKey : notnull
        {
            public List<TVertexKey> StartingVertices
            { get; set; }
            public List<TVertexKey> EndVertices
            { get; set; }
        }
        private static string DefaultGraphFilename = "graph.json";
        private static string DefaultSearchFilename = "search.json";
        private static string DefaultOutputFilename = "output.txt";
        private static string DefaultGraphLocation = $"input/{DefaultGraphFilename}";
        private static string DefaultSearchLocation = $"input/{DefaultSearchFilename}";
        private static string DefaultOutputLocation = $"output/{DefaultOutputFilename}";
        static int Main(string[] args)
        {
            if (args.Length > 3)
            {
                Console.Error.WriteLine("Too many arguments");
                PrintHelp();
                return 1;
            }
            var graphLocation = DetermineFilePath(args, 0, DefaultGraphFilename, DefaultGraphLocation);
            var searchLocation = DetermineFilePath(args, 1, DefaultSearchFilename, DefaultSearchLocation);
            var outputLocation = DetermineFilePath(args, 2, DefaultOutputFilename, DefaultOutputLocation);
            Console.WriteLine($"Graph file path: {graphLocation}");
            Console.WriteLine($"Search file path: {searchLocation}");
            Console.WriteLine($"Output file path: {outputLocation}");
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new EdgeConverter<string>(),
                    new GraphConverter<string, IEdge<string>>()
                }
            };

            var graphDefinitionText = File.ReadAllText(graphLocation);
            var graph = JsonSerializer.Deserialize<Graph<string, IEdge<string>>>(graphDefinitionText, options)!;

            var searchDefinitionText = File.ReadAllText(searchLocation);
            var searchData = JsonSerializer.Deserialize<SearchData<string>>(searchDefinitionText, options)!;
            Console.WriteLine($"Number of vertices: {graph.Vertices.Count()}");
            Console.WriteLine($"Number of edges: {graph.Edges.Count()}");
            Console.WriteLine($"Searching paths from: {string.Join(" ", searchData.StartingVertices)}");
            Console.WriteLine($"To: {string.Join(" ", searchData.EndVertices)}");
            Console.WriteLine($"Found: ");
            Console.WriteLine("---FOUND PATHS---");
            var paths = graph.FindAllPathsIterative(searchData.StartingVertices, searchData.EndVertices);
            foreach (var path in paths)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine($"Number of paths: {paths.Count()}");
            Console.WriteLine("---DISJOINT TUPLES---");
            var st = Stopwatch.StartNew();
            var result = graph.FindAllDisjointTuples(
                paths.ToList(),
                searchData.StartingVertices,
                searchData.EndVertices);
            Console.WriteLine($"Time to find all tuples: {st.ElapsedMilliseconds} ms");
            st.Stop();

            var dir = Path.GetDirectoryName(outputLocation)!;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(outputLocation))
            {
                File.Create(outputLocation).Close();
            }
            using (var fs = new FileStream(outputLocation, FileMode.Truncate))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine("---PATHS---");
                    foreach (var path in paths)
                    {
                        sw.WriteLine(path);
                    }
                    sw.WriteLine("---TUPLES---");
                    foreach (var tuple in result)
                    {
                        var text = String.Join(", ", tuple.Select(path => path.Identifier));
                        sw.WriteLine($"{text}");
                    }
                }
            }
            return 0;
        }
        private static void PrintHelp()
        {
            Console.WriteLine("Usage: RailSim [graph_filepath] [search_filepath] [output_filepath]");
            Console.WriteLine("       If no file paths are provided, default locations are used.");
        }
        private static string DetermineFilePath(string[] args, int index, string defaultFilename, string defaultLocation)
        {
            if (args.Length > index)
            {
                var providedPath = args[index];
                if (Directory.Exists(providedPath))
                {
                    return Path.Combine(providedPath, defaultFilename);
                }
                else if (!File.Exists(providedPath) && !Path.HasExtension(providedPath) && Directory.Exists(providedPath))
                {
                    return Path.Combine(providedPath, defaultFilename);
                }
                else
                {
                    return Path.GetFullPath(providedPath);
                }
            }

            return Path.GetFullPath(defaultLocation);
        }

    }
}