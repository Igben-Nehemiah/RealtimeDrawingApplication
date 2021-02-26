using System;
using System.Collections.Generic;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectProxy
    {
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual ICollection<DrawingCanvasObjectProxy> ProjectDrawingCanvasObjects { get; set; }
        public virtual ICollection<ProjectUserProxy> SharedUsers { get; set; }
        public virtual UserProxy ProjectCreator { get; set; }
    }
}
