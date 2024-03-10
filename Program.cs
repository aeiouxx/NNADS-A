using RailSim.Model;
using System.Diagnostics;

namespace RailSim
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var graph = GraphFactory.Create<string>();
            // TODO: remove me, just to generate the json...
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
            graph.AddVertex("v73");
            graph.AddEdge(new Edge<string>("v63", "v73"));
            graph.AddEdge(new Edge<string>("v69", "v73"));
            graph.AddVertex("v120");
            graph.AddEdge(new Edge<string>("v120", "v69"));
            graph.AddVertex("v122");
            graph.AddVertex("v124");
            graph.AddVertex("v64");
            graph.AddEdge(new Edge<string>("v122", "v64"));
            graph.AddEdge(new Edge<string>("v124", "v64"));
            graph.AddVertex("v126");
            graph.AddVertex("v128");
            graph.AddVertex("v59");
            graph.AddEdge(new Edge<string>("v126", "v59"));
            graph.AddEdge(new Edge<string>("v128", "v59"));
            graph.AddVertex("v130");
            graph.AddVertex("v132");
            graph.AddVertex("v140");
            graph.AddVertex("v51");
            graph.AddVertex("v52");
            graph.AddVertex("v53");
            graph.AddVertex("v55");
            graph.AddVertex("v58");
            graph.AddVertex("v134");
            graph.AddVertex("v136");
            graph.AddVertex("v138");
            graph.AddEdge(new Edge<string>("v130", "v58"));
            graph.AddEdge(new Edge<string>("v132", "v55"));
            graph.AddEdge(new Edge<string>("v55", "v58"));
            graph.AddEdge(new Edge<string>("v140", "v51"));
            graph.AddEdge(new Edge<string>("v51", "v52"));
            graph.AddEdge(new Edge<string>("v51", "v53"));
            graph.AddEdge(new Edge<string>("v52", "v134"));
            graph.AddEdge(new Edge<string>("v53", "v136"));
            graph.AddEdge(new Edge<string>("v53", "v138"));
            graph.AddEdge(new Edge<string>("v52", "v55"));
            graph.AddVertex("v75");
            graph.AddVertex("v82");
            graph.AddVertex("v81");
            graph.AddEdge(new Edge<string>("v64", "v82"));
            graph.AddEdge(new Edge<string>("v59", "v75"));
            graph.AddEdge(new Edge<string>("v58", "v81"));
            graph.AddEdge(new Edge<string>("v75", "v82"));
            graph.AddEdge(new Edge<string>("v75", "v81"));
            graph.AddVertex("v80");
            graph.AddVertex("v79");
            graph.AddVertex("v84");
            graph.AddVertex("v83");
            graph.AddEdge(new Edge<string>("v78", "v80"));
            graph.AddEdge(new Edge<string>("v77", "v79"));
            graph.AddEdge(new Edge<string>("v73", "v84"));
            graph.AddEdge(new Edge<string>("v82", "v84"));
            graph.AddEdge(new Edge<string>("v81", "v83"));
            graph.AddVertex("v85");
            graph.AddVertex("v86");
            graph.AddEdge(new Edge<string>("v80", "v85"));
            graph.AddEdge(new Edge<string>("v79", "v85"));
            graph.AddEdge(new Edge<string>("v79", "v86"));
            graph.AddEdge(new Edge<string>("v84", "v86"));
            graph.AddVertex("v87");
            graph.AddVertex("v88");
            graph.AddEdge(new Edge<string>("v85", "v87"));
            graph.AddEdge(new Edge<string>("v86", "v88"));
            graph.AddEdge(new Edge<string>("v83", "v88"));
            graph.AddVertex("v93");
            graph.AddVertex("v94");
            graph.AddEdge(new Edge<string>("v80", "v94"));
            graph.AddEdge(new Edge<string>("v83", "v93"));

            graph.AddVertex("v95");
            graph.AddVertex("v89");
            graph.AddVertex("v90");
            graph.AddVertex("v92");
            graph.AddVertex("v91");
            graph.AddEdge(new Edge<string>("v89", "v92"));
            graph.AddEdge(new Edge<string>("v90", "v91"));
            graph.AddEdge(new Edge<string>("v87", "v89"));
            graph.AddEdge(new Edge<string>("v88", "v90"));
            graph.AddEdge(new Edge<string>("v87", "v95"));
            graph.AddEdge(new Edge<string>("v88", "v95"));
            graph.AddEdge(new GauntletEdge<string>("v95", "v90", "v87"));
            graph.AddEdge(new GauntletEdge<string>("v95", "v89", "v88"));
            graph.AddVertex("v602");
            graph.AddVertex("v601");
            graph.AddVertex("v301");
            graph.AddVertex("v302");
            graph.AddEdge(new Edge<string>("v94", "v602"));
            graph.AddEdge(new Edge<string>("v92", "v94"));
            graph.AddEdge(new Edge<string>("v91", "v93"));
            graph.AddEdge(new Edge<string>("v92", "v601"));
            graph.AddEdge(new Edge<string>("v91", "v301"));
            graph.AddEdge(new Edge<string>("v93", "v302"));
            var startingVertices = new[] {
                "v113",
                "v111",
                "v109",
                "v107",
                "v103",
                "v101",
                "v102",
                "v104",
                "v106",
                "v108",
                "v112",
                "v114",
                "v120",
                "v122",
                "v124",
                "v126",
                "v128",
                "v130",
                "v132",
                "v140",
            };
            var endVertices = new[] {
                "v134",
                "v136",
                "v138",
                "v602",
                "v601",
                "v301",
                "v302",
            };
            Console.WriteLine($"Number of vertices: {graph.Vertices.Count()}");
            Console.WriteLine($"Number of edges: {graph.Edges.Count()}");
            Console.WriteLine($"Searching paths from: {string.Join(" ", startingVertices)}");
            Console.WriteLine($"To: {string.Join(" ", endVertices)}");
            Console.WriteLine($"Found: ");
            Console.WriteLine("---ITERATIVE FIND---");
            var paths2 = graph.FindAllPathsIterative(startingVertices, endVertices);
            Console.WriteLine($"Size: {paths2.Count()}");
            foreach (var path in paths2)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine("---DISJOINT TUPLES---");
            var st = Stopwatch.StartNew();
            var result = graph.FindAllDisjointTuples(startingVertices, endVertices);
            Console.WriteLine($"Time to find all tuples: {st.ElapsedMilliseconds} ms");
            //foreach (var tuple in result)
            //{
            //    Console.WriteLine($"{{ {String.Join(", ", tuple.Select(t => t.Identifier))} }}");
            //}
            st.Stop();
        }
    }
}