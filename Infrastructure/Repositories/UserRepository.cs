using Models;
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

        public bool ContainsUser(string emailAddress)
        {
            var user = RealtimeDrawingApplicationContext.Users
                 .Where(u => u.UserEmailAddress == emailAddress.ToLower())
                 .ToList();

            if (user.Count != 0)
            {
                return true;
            }
            return false;
        }

        public User GetUser(string emailAddress, string password)
        {
            var user = RealtimeDrawingApplicationContext.Users
                 .Where(u => u.UserEmailAddress == emailAddress.ToLower()
                 && u.UserPassword == password)
                 .ToList();

            if (user.Count != 0)
            {
                return user[0];
            }
            return null;
        }

        public User GetUserWithEmailAddress(string emailAddress)
        {
            var user = RealtimeDrawingApplicationContext.Users
                .Where(u => u.UserEmailAddress == emailAddress.ToLower())
                .ToList();

            if (user.Count != 0)
            {
                return user[0];
            }
            return null;
        }
    }
}
