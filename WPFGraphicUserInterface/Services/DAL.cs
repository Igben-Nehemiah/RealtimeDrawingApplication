using Infrastructure;
using Infrastructure.UnitOfWork;
using WPFGraphicUserInterface.ModelProxies;
using WPFGraphicUserInterface.Views;

namespace WPFGraphicUserInterface.Services
{
    public static class DAL
    {
        public static bool IsValidUser(string email, string password, out UserProxy userProxy)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var users = unitOfWork.Users.GetAll();

                if (users == null)
                {
                    userProxy = null;
                    return false;
                }
                
                foreach (var user in users)
                {
                    if (user.UserEmailAddress == email && user.UserPassword == password)
                    {
                        userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(user);
                        return true;
                    }
                }
                userProxy = null;
                return false;
            }
        }

        public static void AddNewUserToDatabase(UserProxy userProxy)
        {
            //Convert user proxy to user model
            var newUser = ProxyToModelConverter.UserProxyToUserModelConverter(userProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                unitOfWork.Users.Add(newUser);
                unitOfWork.Complete();
            }
        }


        public static void SaveProjectToDatabase(ProjectProxy projectToSave)
        {
            var project = ProxyToModelConverter.ProjectProxyToProjectModelConverter(projectToSave);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var savedProjects = unitOfWork.Projects.GetAll();

                foreach (var savedProject in savedProjects)
                {
                    if (project.ProjectId == savedProject.ProjectId)
                    {
                        //Modify project
                        var projectToOverride = unitOfWork.Projects.Get(savedProject.ProjectId);
                        projectToOverride= project;
                        unitOfWork.Complete();
                        return;
                    }
                }
                //Save new project
                unitOfWork.Projects.Add(project);
            }
        }

        public static void LoadUserProjectsFromDatabase(UserProxy userProxy)
        {
            var userModel = ProxyToModelConverter.UserProxyToUserModelConverter(userProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var userCreatedProjects = unitOfWork.Projects.GetUserCreatedProjects(userModel.UserId);
                unitOfWork.Complete();
            }
        }

    }
}
