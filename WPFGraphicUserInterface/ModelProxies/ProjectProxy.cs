using System;
using System.Collections.Generic;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectProxy
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public string ProjectCreatorEmailAddress { get; set; }

        //Navigation Properties
        public virtual ICollection<DrawingCanvasObjectProxy> ProjectDrawingCanvasObjects { get; set; }
    }
}
