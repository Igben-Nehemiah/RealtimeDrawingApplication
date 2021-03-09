using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Models
{
    public partial class DrawingCanvasObject
    {
        [Key]
        [XmlIgnoreAttribute]
        [JsonIgnore]
        public int CanvasObjectId { get; set; }

        [XmlAttribute]
        public string CanvasObjectGuid { get; set; }

        [XmlAttribute]
        public string CanvasObjectName { get; set; }

        [XmlAttribute]
        public double XPosition { get; set; }

        [XmlAttribute]
        public double YPosition { get; set; }

        [XmlAttribute]
        public string ShapeFill { get; set; }

        [XmlAttribute]
        public string BorderFill { get; set; }

        [XmlAttribute]
        public double Width {get; set; }

        [XmlAttribute]
        public double Height { get; set; }
        //public string ItemType { get; set; }

        //Navigation Property
        public virtual Project Project { get; set; }
    }
}