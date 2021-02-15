namespace Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        

        //Navigation Properties
        public virtual Account Account { get; set; }
    }
}
