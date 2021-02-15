using System;
using System.Collections.Generic;

namespace Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        
        //Navigation properties
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
