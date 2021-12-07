using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mln_generator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int layers=0, nodes=0, intraLayerEdges =0, interLayerEdges = 0;
            string name = "asd";
            Console.WriteLine("MLN Generator v1.0");
            Console.WriteLine("Let's generate a graph!");
            Console.WriteLine("What is the name of the graph?");
            name = Console.ReadLine();
            Console.WriteLine("How many Layers?");
            layers = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("How many nodes per layer?");
            nodes = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("How many edges per layer?");
            intraLayerEdges = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("How many inter layer edges?");
            interLayerEdges = Convert.ToInt32(Console.ReadLine());
            generateGraph(name, layers, nodes, intraLayerEdges, interLayerEdges);
        }

        private static void generateGraph(string name, int numberOfLayers, int numberOfNodesPerLayer, int numberOfIntraLayerEdges, int numberOfInterLayerEdges)
        {
            Random rand = new Random();

            string starter = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
                "<graphml xmlns=\"http://graphml.graphdrawing.org/xmlns\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://graphml.graphdrawing.org/xmlns http://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd\">\n\n" +
                "<key id=\"d1\" for=\"node\" attr.name=\"Size\" attr.type=\"float\">\n" +
                "\t<default>1,0</default>\n" +
                "</key>\n" +
                "<key id=\"d0\" for=\"node\" attr.name=\"Position\" attr.type=\"Vector3\">\n" +
                "\t<default>(0,0,0)</default>\n" +
                "</key>\n" +
                "<key id=\"d2\" for=\"node\" attr.name=\"Shape\" attr.type=\"string\">\n" +
                "\t<default>aCube</default>\n" +
                "</key>\n" +
                "<key id=\"d4\" for=\"node\" attr.name=\"Level\" attr.type=\"int\">\n" +
                "\t<default>0</default>\n" +
                "</key>\n" +
                "<key id=\"d3\" for=\"edge\" attr.name=\"Shape\" attr.type=\"string\">\n" +
                "\t<default>a</default>\n" +
                "</key>\n" +
                "<graph id=\"G\" edgedefault=\"undirected\">\n";
            string ender = "\n\t</graph>\n" +
                "</graphml>";
            

            string nodesString = "";
            string interLayerEdgesString = "";
            string intraLayerEdgesString = "";
            
            for (int layer = 0; layer < numberOfLayers; layer++)
            {
                nodesString = nodesString + "<!-- nodes -->" + "\n";
                for (int node = 0; node < numberOfNodesPerLayer; node++)
                {
                    nodesString = nodesString +
                        "<node id=\"L" + layer + "N" + node + "\">\n" +
                        "\t<data key = \"d4\">" + layer + "</data>\n" +
                        "\t<data key = \"d2\">" + "vL" + layer + "</data>\n" +
                        "</node>\n";
                }
            }

            for (int layer = 0; layer < numberOfLayers; layer++)
            {
                intraLayerEdgesString = intraLayerEdgesString + "<!-- intra layer edges -->" + "\n";
                for (int edge = 0; edge < numberOfIntraLayerEdges; edge++)
                {
                    int node1 = rand.Next(numberOfNodesPerLayer);
                    int node2 = rand.Next(numberOfNodesPerLayer);
                    if (node1 == node2)
                    {
                        node2 = (node2 + 1) % numberOfNodesPerLayer;
                    }
                    intraLayerEdgesString = intraLayerEdgesString +
                        "<edge source=\"L" + layer + "N" + node1 + "\"" + " target=\"L" + layer + "N" + node2 + "\"" + ">\n" +
                        "\t<data key = \"d3\">a</data>\n" +
                        "</edge>" + "\n";
                }
            }

            
            interLayerEdgesString = interLayerEdgesString + "<!-- inter layer edges -->" + "\n";
            for (int edge = 0; edge < numberOfInterLayerEdges; edge++)
            {
                int randomLayer1 = rand.Next(numberOfLayers);
                int randomLayer2 = rand.Next(numberOfLayers);
                if (randomLayer1 == randomLayer2)
                {
                    randomLayer2 = (randomLayer2 + 1) % numberOfLayers;
                }
                interLayerEdgesString = interLayerEdgesString +
                    "<edge source=\"L" + randomLayer1 + "N" + rand.Next(numberOfNodesPerLayer) + "\"" + " target=\"L" + randomLayer2 + "N" + rand.Next(numberOfNodesPerLayer) + "\"" + ">\n" +
                    "\t<data key = \"d3\">b</data>\n" +
                    "</edge>" + "\n";
            }
            

            string completeString = starter + nodesString + intraLayerEdgesString+ interLayerEdgesString + ender;

            if (name == "")
            {
                name = numberOfLayers + "L" + numberOfNodesPerLayer + "N" + numberOfIntraLayerEdges + "intraE" + numberOfInterLayerEdges + "interE";
            }
            File.WriteAllText("../../../../" + name + ".graphml", completeString);

        }

    }
}
