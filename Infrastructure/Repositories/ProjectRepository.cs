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

        public User GetCreatorWithId(int creatorId)
        {
            
            var project = RealtimeDrawingApplicationContext.Projects
                .FirstOrDefault(p => p.ProjectCreator.UserId == creatorId);

            return RealtimeDrawingApplicationContext.Users.FirstOrDefault(u => u.UserCreatedProjects.Contains(project));

        }

        public User GetCreator(string projectName)
        {
            var project = RealtimeDrawingApplicationContext.Projects
                .FirstOrDefault(p => p.ProjectName.ToLower() == projectName.ToLower());

            return RealtimeDrawingApplicationContext.Users.FirstOrDefault(u => u.UserCreatedProjects.Contains(project));

        }

        public Project GetProjectWithProjectName(string projectName, int projectCreatorId)
        {
            return RealtimeDrawingApplicationContext.Projects.FirstOrDefault(p => p.ProjectName.ToLower()
             == projectName.ToLower() && p.ProjectCreator.UserId == projectCreatorId);
        }

       
        public IEnumerable<Project> GetUserCreatedProjects(int userId)
        {
            return RealtimeDrawingApplicationContext.Projects
                .Where(p => p.ProjectCreator.UserId == userId)
                .ToList();
        }
    }
}
