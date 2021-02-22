using System.Collections.Generic;

namespace WPFGraphicUserInterface.ModelProxies
{
    public class ProjectProxy
    {
        public string ProjectName { get; set; }
        public ICollection<UserProxy> SharedUsers { get; set; }

    }
}
