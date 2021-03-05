using System;
using System.Collections.Generic;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectProxy 
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual List<DrawingCanvasObjectProxy> ProjectDrawingCanvasObjects { get; set; }
        public virtual List<ProjectUserProxy> SharedUsers { get; set; }
        public virtual UserProxy ProjectCreator { get; set; }
    }
}
