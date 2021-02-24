using System;
using System.Collections.Generic;

namespace Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public string ProjectCreatorEmailAddress { get; set; }

        //Navigation Properties
        public virtual ICollection<DrawingCanvasObject> ProjectDrawingCanvasObjects { get; set; }
    }
}
