using RailSim.Model;

namespace RailSim
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphFactory.Create<string>();
            graph.AddVertex("v113");
            graph.AddVertex("v111");
            graph.AddVertex("v61");
            graph.AddEdge(new Edge<string>("v113", "v61"));
            graph.AddEdge(new Edge<string>("v111", "v61"));
            graph.AddVertex("v109");
            graph.AddVertex("v107");
            graph.AddVertex("v62");
            graph.AddEdge(new Edge<string>("v109", "v62"));
            graph.AddEdge(new Edge<string>("v107", "v62"));
            graph.AddVertex("v68");
            graph.AddEdge(new Edge<string>("v61", "v68"));
            graph.AddEdge(new Edge<string>("v62", "v68"));
            graph.AddVertex("v103");
            graph.AddVertex("v65");
            graph.AddVertex("v101");
            graph.AddVertex("v57");
            graph.AddEdge(new Edge<string>("v103", "v65"));
            graph.AddEdge(new Edge<string>("v101", "v57"));
            graph.AddEdge(new Edge<string>("v57", "v65"));
            graph.AddVertex("v71");
            graph.AddEdge(new Edge<string>("v68", "v71"));
            graph.AddEdge(new Edge<string>("v65", "v71"));
            graph.AddVertex("v102");
            graph.AddVertex("v56");
            graph.AddEdge(new Edge<string>("v102", "v56"));
            graph.AddVertex("v66");
            graph.AddEdge(new Edge<string>("v57", "v66"));
            graph.AddEdge(new Edge<string>("v56", "v66"));
            graph.AddVertex("v67");
            graph.AddEdge(new Edge<string>("v56", "v67"));
            graph.AddVertex("v104");
            graph.AddEdge(new Edge<string>("v104", "v67"));
            graph.AddVertex("v72");
            graph.AddEdge(new Edge<string>("v66", "v72"));
            graph.AddVertex("v76");
            graph.AddEdge(new Edge<string>("v67", "v76"));
            graph.AddVertex("v78");
            graph.AddEdge(new Edge<string>("v71", "v78"));
            graph.AddEdge(new Edge<string>("v72", "v78"));
            graph.AddVertex("v77");
            graph.AddEdge(new Edge<string>("v72", "v77"));
            graph.AddEdge(new Edge<string>("v76", "v77"));
            graph.AddVertex("v106");
            graph.AddVertex("v70");
            graph.AddVertex("v108");
            graph.AddEdge(new Edge<string>("v106", "v70"));
            graph.AddEdge(new Edge<string>("v108", "v70"));
            graph.AddVertex("v74");
            graph.AddEdge(new Edge<string>("v70", "v74"));
            graph.AddEdge(new Edge<string>("v74", "v76"));
            graph.AddVertex("v112");
            graph.AddVertex("v60");
            graph.AddVertex("v63");
            graph.AddEdge(new Edge<string>("v112", "v60"));
            graph.AddEdge(new Edge<string>("v60", "v63"));
            graph.AddEdge(new Edge<string>("v63", "v74"));
            graph.AddVertex("v114");
            graph.AddVertex("v54");
            graph.AddEdge(new Edge<string>("v114", "v54"));
            graph.AddEdge(new Edge<string>("v54", "v60"));
            graph.AddVertex("v69");
            graph.AddEdge(new Edge<string>("v54", "v69"));

            var startingVertices = new[] { "v113", "v11" };
            var endVertices = new[] { };
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
        }
    }
}