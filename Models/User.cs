using System;
using System.Collections.Generic;

namespace Models
{
    public partial class User 
    {
        //public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }

        //Navigation properties
        public virtual ICollection<Project> UserCreatedProjects { get; set; }
        public virtual ICollection<ProjectUser> UserSharedProjects { get; set; }
    }

}
