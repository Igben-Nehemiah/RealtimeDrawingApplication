using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual ICollection<DrawingCanvasObject> ProjectDrawingCanvasObjects { get; set; }
        public virtual ICollection<ProjectUser> SharedUsers { get; set; }
        public virtual User ProjectCreator { get; set; }
    }
}
