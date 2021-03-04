using Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetUserCreatedProjects(int userId);
    }
}
