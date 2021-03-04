namespace Models
{
    public partial class ProjectUser
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        //Navigation Properties
        public virtual Project SharedProject { get; set; }
        public virtual User SharedUser { get; set; }
    }
}
