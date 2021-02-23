using System;
using System.Collections.Generic;

namespace Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }

        //Navigation Properties
        public virtual User ProjectCreator { get; set; }
        public virtual ICollection<DrawingCanvasObject> ProjectDrawingCanvasObjects { get; set; }
    }
}
