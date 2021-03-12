using Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IProjectUserRepository : IRepository<ProjectUser>
    {
        ProjectUser GetProjectUser(int sharedUserId, int sharedProjectId);
        void RemoveProjectUser(int sharedUserId, int projectId);
    }
}
