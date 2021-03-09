using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Models
{
    [Serializable, 
        XmlRoot(Namespace = "List of drawing components")]
    public partial class DrawingCanvasObject : ISerializable
    {
        public DrawingCanvasObject()
        {
        }

        public DrawingCanvasObject(SerializationInfo info, StreamingContext context)
        {
            XPosition = (double)info.GetValue("XPosition", typeof(double));
            YPosition = (double)info.GetValue("YPosition", typeof(double));
            Width = (double)info.GetValue("Width", typeof(double));
            Height = (double)info.GetValue("Height", typeof(double));
            CanvasObjectGuid = (string)info.GetValue("CanvasObjectGuid", typeof(string));
            CanvasObjectName = (string)info.GetValue("CanvasObjectName", typeof(string));
            ShapeFill = (string)info.GetValue("ShapeFill", typeof(string));
            BorderFill = (string)info.GetValue("BorderFill", typeof(string));
            //ItemType = (string)info.GetValue("ItemType", typeof(string));
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
            //info.AddValue("ItemType", ItemType);
        }
    }

}
