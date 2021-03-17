using Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetUserCreatedProjects(int userId);

        User GetCreator(string projectName);

        User GetCreatorWithId(int projectId);

        Project GetProjectWithProjectName(string projectName, int projectCreatorId);
    }
}
