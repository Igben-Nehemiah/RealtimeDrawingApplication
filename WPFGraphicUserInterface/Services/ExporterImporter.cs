using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WPFGraphicUserInterface.ModelProxies;


namespace WPFGraphicUserInterface.Services
{
    public static class ExporterImporter
    {
        public static void Export(ProjectProxy project, string filePath, string exportType = "json")
        {
            if (exportType.ToLower() == "json")
            {
                SerializeToJSON(project.ProjectDrawingCanvasObjects, filePath);
            }
            else
            {
                SerializeToXML(project.ProjectDrawingCanvasObjects, filePath);
            }
        }

        public static IEnumerable<DrawingCanvasObjectProxy> Import(string filePath, string importType = "json")
        {
            if (importType.ToLower() == "json")
            {
                return DeserializeFromJSON(filePath);
            }
            else
            {
                return DeserializeFromXML(filePath);
            }
        }

        public static void SerializeToXML(IEnumerable<DrawingCanvasObjectProxy> projectDrawingObjects, string filePath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DrawingCanvasObjectProxy>));

            using (TextWriter tw = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(tw, projectDrawingObjects);
            }
        }

        private static void SerializeToJSON(IEnumerable<DrawingCanvasObjectProxy> projectDrawingObjects, string filePath)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();

            using (TextWriter tw = new StreamWriter(filePath))
            {
                jsonSerializer.Serialize(tw, projectDrawingObjects);
            }
        }


        private static IEnumerable<DrawingCanvasObjectProxy> DeserializeFromXML(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<DrawingCanvasObjectProxy>));
            var drawingObjects = new List<DrawingCanvasObjectProxy>();

            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = xmlSerializer.Deserialize(tr);
                drawingObjects = (List<DrawingCanvasObjectProxy>)obj;
            }

            return drawingObjects;
        }

        private static IEnumerable<DrawingCanvasObjectProxy> DeserializeFromJSON(string filePath)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var drawingObjects = new List<DrawingCanvasObjectProxy>();


            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = jsonSerializer.Deserialize(tr, typeof(List<DrawingCanvasObjectProxy>));
                drawingObjects = (List<DrawingCanvasObjectProxy>)obj;
            }

            return drawingObjects;
        }
    }
}

