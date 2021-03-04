namespace WPFGraphicUserInterface.ModelProxies
{
    public class SharedProxy
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        //Navigation Properties
        public virtual ProjectProxy SharedProject { get; set; }
        public virtual UserProxy SharedUser { get; set; }
    }
}