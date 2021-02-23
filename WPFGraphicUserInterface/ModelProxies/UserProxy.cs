using System;
using System.Collections.Generic;
using System.Text;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class UserProxy
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }

        //Navigation properties
        public virtual ICollection<ProjectProxy> UserCreatedProjects { get; set; }
        public virtual ICollection<SharedProxy> UserSharedProjects { get; set; }
    }
}
