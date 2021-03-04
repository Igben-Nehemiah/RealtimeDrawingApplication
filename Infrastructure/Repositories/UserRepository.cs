using Models;
using System.Collections.Generic;
using System.Data.Entity;

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

    }
}
