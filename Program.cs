using System;
using System.Xml;
using System.IO;

namespace CombinedSplitter
{
    class Combo
    {
        // Defines XML element paths as constant strings to be used in the Main method
        private const string Xpath = "/ROW_Easement/File_Name";
        private const string Xpath2 = "/ROW_Easement/tblDocument/VendorDocumentID";
        private const string Xpath3 = "/ROW_Easement/tblPersonOfInterest/From/From_1/VendorNamesID";
        private const string Xpath4 = "/ROW_Easement/tblTractMain/tblTractMain_1/VendorTractID";
        private const string Doc = @"C:\Users\taylor.r\Documents\XMLs\SeparatedDocumentXMLs\Doc_";
        private const string POI = @"C:\Users\taylor.r\Documents\XMLs\SeparatedPOIXMLs\POI_";
        private const string Tract = @"C:\\Users\\taylor.r\\Documents\\XMLs\\SeparatedTractMainXMLs\\TractMain_";
        private const string DocPath = @"C:\Users\taylor.r\Documents\XMLs\SeparatedDocumentXMLs\CombinedDOCS\";
        private const string ComboPOI = @"C:\Users\taylor.r\Documents\XMLs\SeparatedPOIXMLs\CombinedPOI\";
        private const string TractMain = @"C:\Users\taylor.r\Documents\XMLs\SeparatedTractMainXMLs\CombinedTractMain\";

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

                // Allow user to specify which Doc they are working on
                string docNum = Path.GetFileName(files);

                // Selects desired node, in this case, tblTractMain under the ROW_Easement Element
                XmlNodeList nodeList = doc.SelectNodes("/ROW_Easement/tblDocument");

                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node in nodeList)
                {
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer = new XmlTextWriter(filename: Doc + root.InnerText + ".xml", null))
                    {
                        //var docTree = new XElement("Root");
                        //docTree.Add(writer);
                        writer.Formatting = Formatting.Indented;
                        node.WriteTo(writer);
                    }
                }
                // Had to Create a new element for tblPersonOfInterest to be under so that it would
                // select the entire tblPersonOfInterest node and not the fragment within.
                XmlElement rootNew = doc.CreateElement("poiRoot");

                // Selects desired node, in this case, tblTractMain under the ROW_Easement Element
                XmlNodeList nodeList2 = doc.SelectNodes("descendant::tblPersonOfInterest");
                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node2 in nodeList2)
                {
                    // Appending the tblPersonOfInterest node to the rootNew node we created
                    rootNew.AppendChild(node2);
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer2 = new XmlTextWriter(filename: POI + root.InnerText + ".xml", null))
                    {

                        writer2.Formatting = Formatting.Indented;
                        rootNew.WriteContentTo(writer2);
                    }
                }

                // Selects desired node, in this case, tblTractMain under the ROW_Easement Element
                XmlNodeList nodeList3 = doc.SelectNodes("/ROW_Easement/tblTractMain");

                // Traverses the elements within the node selected above and applies the code within to each element.
                foreach (XmlNode node3 in nodeList3)
                {
                    // Outputs Node Selection to xml using XmlTextWriter
                    using (XmlTextWriter writer3 = new XmlTextWriter(filename: Tract + root.InnerText + ".xml", null))
                    {
                        writer3.Formatting = Formatting.Indented;
                        node3.WriteContentTo(writer3);
                    }
                }
            }
            // Now to combine them into a new folder in each split folder
            using (var combinedOutput = File.Create(ComboPOI + "CombinedPOIs.xml"))
            {
                foreach (var poiFiles in System.IO.Directory.GetFiles("C:\\Users\\taylor.r\\Documents\\XMLs\\SeparatedPOIXMLs"))
                {

                    using (var input = File.OpenRead(poiFiles))
                    {
                        input.CopyTo(combinedOutput);
                    }

                }
            }
            using (var combinedOutputDoc = File.Create(DocPath + "CombinedDocs.xml"))
            {
                foreach (var docFiles in System.IO.Directory.GetFiles("C:\\Users\\taylor.r\\Documents\\XMLs\\SeparatedDocumentXMLs"))
                {

                    using (var input = File.OpenRead(docFiles))
                    {
                        input.CopyTo(combinedOutputDoc);
                    }

                }
            }
            using (var combinedOutputTractMain = File.Create(TractMain + "CombinedTracts.xml"))
            {
                foreach (var tractFiles in System.IO.Directory.GetFiles("C:\\Users\\taylor.r\\Documents\\XMLs\\SeparatedTractMainXMLs"))
                {

                    using (var input = File.OpenRead(tractFiles))
                    {
                        input.CopyTo(combinedOutputTractMain);
                    }

                }
            }


            // Keeps the Console from Peek-a-Booing
            Console.WriteLine("Press Any Key to [ESC]...");
            Console.ReadKey();
        }
    }
}