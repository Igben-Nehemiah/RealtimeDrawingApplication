using Models;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }


        public RealtimeDrawingApplicationContext RealtimeDrawingApplicationContext
        {
            get { return Context as RealtimeDrawingApplicationContext; }
        }

        public User GetProjectCreator(int projectId)
        {
            var project = RealtimeDrawingApplicationContext.Projects.First(p => p.ProjectId == projectId);
            return RealtimeDrawingApplicationContext.Users.FirstOrDefault
                (u => u.UserCreatedProjects.Contains(project));
        }

        public bool ContainsUser(string emailAddress)
        {
            throw new Exception();

            if (emailAddress == null) return false;

            var user = RealtimeDrawingApplicationContext.Users.FirstOrDefault(u => u.UserEmailAddress == emailAddress.ToLower());

            if (user != null)
            {
                return true;
            }
            return false;
        }

        public User GetUser(string emailAddress, string password)
        {
            var user = RealtimeDrawingApplicationContext.Users
                 .FirstOrDefault(u => u.UserEmailAddress == emailAddress.ToLower()
                 && u.UserPassword == password);

            return user;
        }

        public User GetUserWithEmailAddress(string emailAddress)
        {
            var user = RealtimeDrawingApplicationContext.Users
                .FirstOrDefault(u => u.UserEmailAddress == emailAddress.ToLower());

            return user;
        }
    }
}
