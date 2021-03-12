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
