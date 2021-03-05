namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectUserProxy
    {
        
        public string ProjectName { get; set; }
        public string UserEmail { get; set; }
        public bool CanEdit { get; set; }

        //Navigation Properties
        public virtual ProjectProxy SharedProject { get; set; }
        public virtual UserProxy SharedUser { get; set; }
    }
}
