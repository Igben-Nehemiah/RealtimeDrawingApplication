using Models;

namespace Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserWithEmailAddress(string emailAddress);

        User GetUser(string emailAddress, string password);

        User GetProjectCreator(int projectId);

        bool ContainsUser(string emailAddress);
    }
}
