using Infrastructure;
using Models;
using System;

namespace Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var db = new RealtimeDrawingApplicationContext();
                var user1 = new User()
                {
                    UserFirstName = "Obomaese",
                    UserLastName = "Igben",
                    UserEmailAddress = "igbennehemiah@gmail.com",
                    UserPassword = "Bart Allen",
                };
                db.Users.Add(user1);
                db.SaveChanges();
                Console.WriteLine("Database Created!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
