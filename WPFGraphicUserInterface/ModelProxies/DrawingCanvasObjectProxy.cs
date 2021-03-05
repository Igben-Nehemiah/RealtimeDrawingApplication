using System.Windows;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace WPFGraphicUserInterface.ModelProxies
{
    [Serializable]
    public class DrawingCanvasObjectProxy :UIElement,  ISerializable
    {
        public int CanvasObjectId { get; set; }
        public string CanvasObjectGuid { get; set; }
        public string CanvasObjectName { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public string ShapeFill { get; set; }
        public string BorderFill { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string ItemType { get; set; }

        //Navigation Property
        public virtual ProjectProxy Project { get; set; }

        public DrawingCanvasObjectProxy()
        {
        }

        public DrawingCanvasObjectProxy(SerializationInfo info, StreamingContext context)
        {
            XPosition = (double)info.GetValue("XPosition", typeof(double));
            YPosition = (double)info.GetValue("YPosition", typeof(double));
            Width = (double)info.GetValue("Width", typeof(double));
            Height = (double)info.GetValue("Height", typeof(double));
            CanvasObjectGuid = (string)info.GetValue("CanvasObjectGuid", typeof(string));
            CanvasObjectName = (string)info.GetValue("CanvasObjectName", typeof(string));
            ShapeFill = (string)info.GetValue("ShapeFill", typeof(string));
            BorderFill = (string)info.GetValue("BorderFill", typeof(string));
            ItemType = (string)info.GetValue("ItemType", typeof(string));
        }

        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("XPosition", XPosition);
            info.AddValue("YPosition", YPosition);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
            info.AddValue("CanvasObjectGuid", CanvasObjectGuid);
            info.AddValue("CanvasObjectName", CanvasObjectName);
            info.AddValue("ShapeFill", ShapeFill);
            info.AddValue("BorderFill", BorderFill);
            info.AddValue("ItemType", ItemType);
        }
    }
}
