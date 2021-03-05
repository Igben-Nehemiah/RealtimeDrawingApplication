using Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Services;

namespace Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //    var db = new RealtimeDrawingApplicationContext();
            //    var user1 = new User()
            //    {
            //        UserFirstName = "Obomaese",
            //        UserLastName = "Igben",
            //        UserEmailAddress = "igbennehemiah@gmail.com",
            //        UserPassword = "Bart Allen",
            //    };
            //    db.Users.Add(user1);
            //    db.SaveChanges();
            //    Console.WriteLine("Database Created!");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            XmlSerializer serializer = new XmlSerializer(typeof(List<DrawingCanvasObjectProxy>), "WPFGraphicUserInterface.ModelProxies");

            BinaryFormatter bf = new BinaryFormatter();

            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();

            DrawingCanvasObjectProxy obj = new DrawingCanvasObjectProxy();
            obj.CanvasObjectName = "Item one";

            //using (TextWriter tw = new StreamWriter("testing_export.json"))
            //{
            //    jsonSerializer.Serialize(tw, new List<DrawingCanvasObjectProxy> { obj, new DrawingCanvasObjectProxy() { BorderFill = "green" } });
            //}
            //Console.WriteLine("Complete");

            using (TextWriter tw = new StreamWriter("testing_export.xml"))
            {
                serializer.Serialize(tw, new List<DrawingCanvasObjectProxy> { obj, new DrawingCanvasObjectProxy() { BorderFill = "green" } });
            }
            Console.WriteLine("Complete");

            //using (Stream s = File.Open("testing_export", FileMode.Create))
            //{
            //    bf.Serialize(s, new List<DrawingCanvasObjectProxy> { obj, new DrawingCanvasObjectProxy() { BorderFill ="green"} });
            //}

            //ExporterImporter.SerializeToXML(new List<DrawingCanvasObjectProxy> { obj }, "testing_export");
            //Console.WriteLine("Export complete!");

        }
    }
}
