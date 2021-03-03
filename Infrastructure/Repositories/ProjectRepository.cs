using Models;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }
    }
}
