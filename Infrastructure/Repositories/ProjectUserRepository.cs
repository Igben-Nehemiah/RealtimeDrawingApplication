using Models;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ProjectUserRepository : Repository<ProjectUser>, IProjectUserRepository
    {
        public ProjectUserRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }

        public RealtimeDrawingApplicationContext RealtimeDrawingApplicationContext
        {
            get { return Context as RealtimeDrawingApplicationContext; }
        }

        public ProjectUser GetProjectUser(int sharedUserId, int sharedProjectId)
        {
            var projectUser = RealtimeDrawingApplicationContext.ProjectUsers
                .FirstOrDefault(pu => pu.ProjectId == sharedProjectId && pu.UserId == sharedUserId);

            return projectUser;
        }

        public void RemoveProjectUser(int sharedUserId, int projectId)
        {
            var projectuser = GetProjectUser(sharedUserId, projectId);
            RealtimeDrawingApplicationContext.Remove(projectuser);
        }
    }
}
