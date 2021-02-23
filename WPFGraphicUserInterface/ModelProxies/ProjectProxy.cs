using System;
using System.Collections.Generic;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectProxy
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }

        //Navigation Properties
        public virtual UserProxy ProjectCreator { get; set; }
        public virtual ICollection<DrawingCanvasObjectProxy> ProjectDrawingCanvasObjects { get; set; }
    }
}
