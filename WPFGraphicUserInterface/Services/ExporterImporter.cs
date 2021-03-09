using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WPFGraphicUserInterface.ModelProxies;
using Models;

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
                return ConvertDrawingObjModelsToDrawingObjProxies(DeserializeFromJSON(filePath));
            }
            else
            {
                return ConvertDrawingObjModelsToDrawingObjProxies(DeserializeFromXML(filePath));
            }
        }

        public static void SerializeToXML(List<DrawingCanvasObjectProxy> projectDrawingObjects, string filePath)
        {
            var drawingObjectsModels = new List<DrawingCanvasObject>();

            foreach (var drawingCanvasObjectProxy in projectDrawingObjects)
            {
                //Convert model to form that can be serialized
                var drawingCanvasObjectModel = ProxyToModelConverter
                    .DrawingCanvasObjectProxyToDrawingCanvasObjectModelConverter(drawingCanvasObjectProxy);

                drawingObjectsModels.Add(drawingCanvasObjectModel);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DrawingCanvasObject>));

            using (TextWriter tw = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(tw, drawingObjectsModels);
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


        private static IEnumerable<DrawingCanvasObject> DeserializeFromXML(string filePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<DrawingCanvasObject>));
            var drawingObjects = new List<DrawingCanvasObject>();

            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = xmlSerializer.Deserialize(tr);
                drawingObjects = (List<DrawingCanvasObject>)obj;
            }

            return drawingObjects;
        }

        private static IEnumerable<DrawingCanvasObject> DeserializeFromJSON(string filePath)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var drawingObjects = new List<DrawingCanvasObject>();

            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = jsonSerializer.Deserialize(tr, typeof(List<DrawingCanvasObject>));
                drawingObjects = (List<DrawingCanvasObject>)obj;
            }

            return drawingObjects;
        }

        public static IEnumerable<DrawingCanvasObjectProxy> ConvertDrawingObjModelsToDrawingObjProxies
            (IEnumerable<DrawingCanvasObject> drawingObjectModels)
        {
            var drawingCanvasObjectsProxies = new List<DrawingCanvasObjectProxy>();
            foreach (var drawingObjModel in drawingObjectModels)
            {
                var drawingObjProxy = ModelToProxyConverter.DrawingCanvasObjectToDrawingCanvasObjectProxy(drawingObjModel);
                drawingCanvasObjectsProxies.Add(drawingObjProxy);
            }

            return drawingCanvasObjectsProxies;
        }
    }
}

