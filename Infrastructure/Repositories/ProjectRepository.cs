using Models;
using System.Collections.Generic;
using System.Linq;

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

        public Project GetProjectWithProjectName(string projectName)
        {
            return RealtimeDrawingApplicationContext.Projects.FirstOrDefault(p => p.ProjectName.ToLower()
             == projectName.ToLower());
        }

        public IEnumerable<Project> GetUserCreatedProjects(int userId)
        {
            return RealtimeDrawingApplicationContext.Projects
                .Where(p => p.ProjectCreator.UserId == userId)
                .ToList();
        }
    }
}
