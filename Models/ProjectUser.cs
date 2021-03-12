using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class ProjectUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual Project SharedProject { get; set; }
        public virtual User SharedUser { get; set; }
    }
}
