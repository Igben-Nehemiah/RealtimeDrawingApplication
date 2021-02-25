namespace Models
{
    public class ProjectUser
    {
        public int ProjectId { get; set; }
        public string UserEmail { get; set; }

        //Navigation Properties
        public virtual Project SharedProject { get; set; }
        public virtual User SharedUser { get; set; }
    }
}
