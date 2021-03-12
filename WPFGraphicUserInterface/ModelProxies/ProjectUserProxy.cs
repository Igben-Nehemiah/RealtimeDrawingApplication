namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectUserProxy
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual ProjectProxy SharedProject { get; set; }
        public virtual UserProxy SharedUser { get; set; }
    }
}
