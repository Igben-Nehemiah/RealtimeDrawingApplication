using System.ComponentModel.DataAnnotations;

namespace Models
{
    public partial class DrawingCanvasObject
    {
        [Key]
        public int CanvasObjectId { get; set; }
        public string CanvasObjectGuid { get; set; }
        public string CanvasObjectName { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public string ShapeFill { get; set; }
        public string BorderFill { get; set; }
        public double Width {get; set; }
        public double Height { get; set; }
    }
}