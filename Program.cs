using System;
using System.Xml;
using System.IO;

namespace CombinedSplitter
{
    class Combo
    {
        // Defines XML element paths as constant strings to be used in the Main method
        private const string Xpath = "/Root/File_Name";
        private const string Xpath2 = "/Root/Node_1/SubNode_1";
        private const string Xpath3 = "/Root/Node_2/Child_2/Child_2_2/SubNode_2";
        private const string Xpath4 = "/Root/Node_3/Child_3/SubNode_3";
        private const string Node_1 = @"C:\"; // Destination for Node_1 XML file.
        private const string Node_2 = @"C:\"; // Destination for Node_2 XML file.
        private const string Node_3 = @"C:\"; // Destination for Node_3 XML file.
        private const string ComboNode_1 = @"C:\"; // Destination for combined Node_1 XML files
        private const string ComboNode_2 = @"C:\"; // Destination for combined Node_2 XML files
        private const string ComboNode_3 = @"C:\"; // Destination for combined Node_3 XML files

        static void Main(string[] _1)
        {
            // Allows user to paste in path for source XML into the Console
            Console.WriteLine("Input file path of source XML ::");
            string sourcePath = Console.ReadLine();

            // Loops process over each .xml in the given folder path
            foreach (var files in System.IO.Directory.GetFiles(sourcePath))
            {
                // Loads desired XML doc
                XmlDocument doc = new XmlDocument();
                doc.Load(files);

                /* Selects elements pointed to by the XML element paths listed outside the main method and sets 
                the target element - root 2 - to the inner text of root, the source element. */
                XmlNode root = doc.SelectSingleNode(Xpath);
                XmlNode root2 = doc.SelectSingleNode(Xpath2);
                XmlNode root3 = doc.SelectSingleNode(Xpath3);
                XmlNode root4 = doc.SelectSingleNode(Xpath4);
                root2.InnerText = root.InnerText;
                root3.InnerText = root.InnerText;
                root4.InnerText = root.InnerText;

                //Saves the doc with changes
                doc.Save(files);

                // Retrieves file name of original file within the XML; this was specific to my use case
                string docNum = Path.GetFileName(files);

                // Selects desired node, in this case, Node_1 under the Root Element
                XmlNodeList nodeList = doc.SelectNodes("/Root/Node_1");

                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node in nodeList)
                {
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer = new XmlTextWriter(filename: Node_1 + root.InnerText + ".xml", null))
                    {
                        writer.Formatting = Formatting.Indented;
                        node.WriteTo(writer);
                    }
                }
                // Had to Create a new element for Node_2 to be under so that it would
                // select the entire Node_2 and not the fragment within.
                XmlElement rootNew = doc.CreateElement("NewRootForNode_2");

                // Selects desired node, in this case, Node_2 under the Root Element
                XmlNodeList nodeList2 = doc.SelectNodes("descendant::Node_2");
                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node2 in nodeList2)
                {
                    // Appending the Node_2 node to the rootNew element we created
                    rootNew.AppendChild(node2);
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer2 = new XmlTextWriter(filename: Node_2 + root.InnerText + ".xml", null))
                    {

                        writer2.Formatting = Formatting.Indented;
                        rootNew.WriteContentTo(writer2);
                    }
                }

                // Selects desired node, in this case, Node_3 under the Root Element
                XmlNodeList nodeList3 = doc.SelectNodes("/Root/Node_3");

                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node3 in nodeList3)
                {
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer3 = new XmlTextWriter(filename: Node_3 + root.InnerText + ".xml", null))
                    {
                        writer3.Formatting = Formatting.Indented;
                        node3.WriteContentTo(writer3);
                    }
                }
            }
            
            // Now to combine them into a new folder in each split folder
            using (var combinedOutputNodeTwo = File.Create(ComboNode_2 + "CombinedNode_2.xml"))
            {
                foreach (var nodeTwoFiles in System.IO.Directory.GetFiles("C:\ /*location of separated Node_2 files*/ "))
                {

                    using (var input = File.OpenRead(nodeTwoFiles))
                    {
                        input.CopyTo(combinedOutputNodeTwo);
                    }

                }
            }
            using (var combinedOutputNodeOne = File.Create(ComboNode_1 + "CombinedNode_1.xml"))
            {
                foreach (var nodeOneFiles in System.IO.Directory.GetFiles("C:\ /*location of separated Node_1 files*/ "))
                {

                    using (var input = File.OpenRead(nodeOneFiles))
                    {
                        input.CopyTo(combinedOutputNodeOne);
                    }

                }
            }
            using (var combinedOutputNodeThree = File.Create(ComboNode_3 + "CombinedNode_3.xml"))
            {
                foreach (var nodeThreeFiles in System.IO.Directory.GetFiles("C:\ /*location of separated Node_3 files*/ "))
                {

                    using (var input = File.OpenRead(nodeThreeFiles))
                    {
                        input.CopyTo(combinedOutputNodeThree);
                    }

                }
            }


            // Keeps the Console from Peek-a-Booing
            Console.WriteLine("Press Any Key to [ESC]...");
            Console.ReadKey();
        }
    }
}
