using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }

        public RealtimeDrawingApplicationContext RealtimeDrawingApplicationContext
        {
            get { return Context as RealtimeDrawingApplicationContext; }
        }

        public IEnumerable<Project> GetUserCreatedProjects(int userId)
        {
            return RealtimeDrawingApplicationContext.Projects
                .Include(p => p.ProjectCreator.UserId == userId);
        }
    }
}
