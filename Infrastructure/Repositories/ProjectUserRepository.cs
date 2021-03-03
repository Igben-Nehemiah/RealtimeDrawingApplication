using Models;

namespace Infrastructure.Repositories
{
    public class ProjectUserRepository : Repository<ProjectUser>, IProjectUserRepository
    {
        public ProjectUserRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }
    }
}
