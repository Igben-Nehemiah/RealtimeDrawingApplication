using System;
using System.Collections.Generic;

namespace Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }

        //Navigation properties
        public virtual ICollection<Project> UserCreatedProjects { get; set; }
        public virtual ICollection<Shared> UserSharedProjects { get; set; }
    }
}
