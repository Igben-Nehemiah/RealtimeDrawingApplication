using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WPFGraphicUserInterface.ModelProxies;
using Models;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Ioc;
using WPFUserInterface.Core;
using System.Threading;

namespace WPFGraphicUserInterface.Services
{
    public static class ExporterImporter
    {
        static IEventAggregator eventAggregator { get; set; } = App.ShellContainer.Resolve<IEventAggregator>();

        public static async Task ExportAsync(ProjectProxy project, string filePath, string exportType = "json")
        {
            if (exportType.ToLower() == "json")
            {
                await Task.Run(() => SerializeToJSON(project.ProjectDrawingCanvasObjects, filePath));
            }
            else
            {
                await Task.Run(() => SerializeToXML(project.ProjectDrawingCanvasObjects, filePath));
            }
        }

        public static async Task<IEnumerable<DrawingCanvasObjectProxy>> ImportAsync(string filePath, string importType = "json")
        {
            if (importType.ToLower() == "json")
            {
                return await Task.Run(() => ConvertDrawingObjModelsToDrawingObjProxies(DeserializeFromJSON(filePath)));
            }
            else
            {
                return await Task.Run(() => ConvertDrawingObjModelsToDrawingObjProxies(DeserializeFromXML(filePath)));
            }
        }

        public static void SerializeToXML(List<DrawingCanvasObjectProxy> projectDrawingObjects, string filePath)
        {
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Serializing as XML");

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

            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Serialization complete!");
            Thread.Sleep(2000);
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");
        }

        private static void SerializeToJSON(List<DrawingCanvasObjectProxy> projectDrawingObjects, string filePath)
        {
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Serializing as JSON");

            var drawingObjectsModels = new List<DrawingCanvasObject>();

            foreach (var drawingCanvasObjectProxy in projectDrawingObjects)
            {
                //Convert model to form that can be serialized
                var drawingCanvasObjectModel = ProxyToModelConverter
                    .DrawingCanvasObjectProxyToDrawingCanvasObjectModelConverter(drawingCanvasObjectProxy);

                drawingObjectsModels.Add(drawingCanvasObjectModel);
            }

            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();

            using (TextWriter tw = new StreamWriter(filePath))
            {
                jsonSerializer.Serialize(tw, drawingObjectsModels);
            }

            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Serialization complete!");
            Thread.Sleep(2000);
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");
        }


        private static IEnumerable<DrawingCanvasObject> DeserializeFromXML(string filePath)
        {
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Deserializing from XML!");

            var xmlSerializer = new XmlSerializer(typeof(List<DrawingCanvasObject>));
            var drawingObjects = new List<DrawingCanvasObject>();

            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = xmlSerializer.Deserialize(tr);
                drawingObjects = (List<DrawingCanvasObject>)obj;
            }

            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Deserialization complete!");
            Thread.Sleep(2000);
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");


            return drawingObjects;
        }

        private static IEnumerable<DrawingCanvasObject> DeserializeFromJSON(string filePath)
        {
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Deserializing from JSON!");

            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var drawingObjects = new List<DrawingCanvasObject>();

            using (TextReader tr = new StreamReader(filePath))
            {
                object obj = jsonSerializer.Deserialize(tr, typeof(List<DrawingCanvasObject>));
                drawingObjects = (List<DrawingCanvasObject>)obj;
            }

            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Deserialization complete!");
            Thread.Sleep(2000);
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");
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

