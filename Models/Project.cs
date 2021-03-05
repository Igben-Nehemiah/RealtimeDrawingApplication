using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public partial class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }

        //Navigation Properties
        public virtual List<DrawingCanvasObject> ProjectDrawingCanvasObjects { get; set; }
        public virtual List<ProjectUser> SharedUsers { get; set; }
        public virtual User ProjectCreator { get; set; }
    }
}
