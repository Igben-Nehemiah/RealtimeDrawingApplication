using System.Collections.Generic;

namespace Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }

        //Navigation Properties
        public virtual User User { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
