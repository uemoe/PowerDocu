using System;
using System.IO;
using PowerDocu.Common;

namespace PowerDocu.FlowDocumenter
{
    public static class FlowDocumentationGenerator
    {
        public static void GenerateWordDocumentation(string filePath, string wordTemplate = null)
        {
            if (!File.Exists(filePath))
            {
                NotificationHelper.SendNotification("File not found: " + filePath);
            }

            string path = Path.GetDirectoryName(filePath);
            DateTime startDocGeneration = DateTime.Now;
            FlowParser flowParserFromZip = new FlowParser(filePath);
            if (flowParserFromZip.packageType == FlowParser.PackageType.SolutionPackage)
            {
                path += @"\Solution " + CharsetHelper.GetSafeName(Path.GetFileNameWithoutExtension(filePath));
            }

            foreach (FlowEntity flow in flowParserFromZip.getFlows())
            {
                GraphBuilder gbzip = new GraphBuilder(flow, path);
                gbzip.buildTopLevelGraph();
                gbzip.buildDetailedGraph();
                if (String.IsNullOrEmpty(wordTemplate) || !File.Exists(wordTemplate))
                {
                    FlowWordDocBuilder wordzip = new FlowWordDocBuilder(flow, path, null);
                }
                else
                {
                    FlowWordDocBuilder wordzip = new FlowWordDocBuilder(flow, path, wordTemplate);
                }
            }
            
            DateTime endDocGeneration = DateTime.Now;
            NotificationHelper.SendNotification("FlowDocumenter: Created Word documentation for " + filePath + ". A total of " + flowParserFromZip.getFlows().Count + " files were processed in " + (endDocGeneration - startDocGeneration).TotalSeconds + " seconds.");

        }
    }
}