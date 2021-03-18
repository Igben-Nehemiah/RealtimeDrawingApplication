using Infrastructure;
using Infrastructure.UnitOfWork;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WPFGraphicUserInterface.ModelProxies;
using Prism.Ioc;
using WPFUserInterface.Core;

namespace WPFGraphicUserInterface.Services
{
    public static class DataAccessLayer
    {
        static IEventAggregator eventAggregator { get; set; } = App.ShellContainer.Resolve<IEventAggregator>();

        public static async Task<Tuple<bool, UserProxy>> CheckIfIsApplicationUserAsync(string email)
        {
            eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Checking if user is in the database");

            return await Task.Run(ValidateUser);

            Tuple<bool, UserProxy> ValidateUser()
            {
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var containsUser = unitOfWork.Users.ContainsUser(email);

                    if (containsUser)
                    {
                        var user = unitOfWork.Users.GetUserWithEmailAddress(email);
                        var userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(user);

                        unitOfWork.Complete();

                        eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("User authenticated!");
                        Thread.Sleep(1000);
                        eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Done!");
                        Thread.Sleep(1000);
                        eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");

                        return new Tuple<bool, UserProxy>(true, userProxy);
                    }

                    //var userProxy = null;
                    unitOfWork.Complete();

                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("User not in database!");
                    return new Tuple<bool, UserProxy>(false, null);
                }
            }

        }
        public static async Task<Tuple<bool, UserProxy>> CheckIfUserDetailIsValidAsync(string email, string password)
        {
            return await Task.Run(() => ValidateUser());

            Tuple<bool, UserProxy> ValidateUser()
            {
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var user = unitOfWork.Users.GetUser(email, password);

                    if (user != null)
                    {
                        var userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(user);
                        unitOfWork.Complete();

                        return new Tuple<bool, UserProxy>(true, userProxy);
                    }

                    unitOfWork.Complete();
                    return new Tuple<bool, UserProxy>(false, null);
                }
            }
        }

        public static void AddUserToDatabase(UserProxy userProxy)
        {
            //Convert user proxy to user model
            var newUser = ProxyToModelConverter.UserProxyToUserModelConverter(userProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                unitOfWork.Users.Add(newUser);
                unitOfWork.Complete();
            }
        }

        public static async Task AddProjectToDatabaseAsync(ProjectProxy projectProxy)
        {
            //Convert project proxy to project model
            await Task.Run(() =>
            {
                var newProject = ProxyToModelConverter.ProjectProxyToProjectModelConverter(projectProxy);

                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    unitOfWork.Projects.Add(newProject);
                    unitOfWork.Complete();
                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish($"Project {projectProxy.ProjectName} created and added to the database!");
                }

                //Gets the project and change the project creator
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var project = unitOfWork.Projects.Get(newProject.ProjectId);
                    project.ProjectCreator = ProxyToModelConverter.UserProxyToUserModelConverter(projectProxy.ProjectCreator);
                    unitOfWork.Complete();
                    Thread.Sleep(1000);
                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish($"Project {projectProxy.ProjectName} created and added to the database!");
                    Thread.Sleep(1000);
                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish($"Project creator set to " +
                        $"{ project.ProjectCreator.UserFirstName } {project.ProjectCreator.UserLastName }" +
                        $" created and added to the database!");
                    Thread.Sleep(1000);
                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");
                }
            });

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
                        projectToOverride = project;
                        unitOfWork.Complete();
                        return;
                    }
                }
                //Save new project
                unitOfWork.Projects.Add(project);
                unitOfWork.Complete();
            }
        }

        public static void DeleteProject(ProjectProxy actveProject)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var project = unitOfWork.Projects.Get(actveProject.ProjectId);

                var drawingCanvasObjects = unitOfWork.DrawingCanvasObjects.GetDrawingCanvasObjectsBelongingTo(project.ProjectId);

                drawingCanvasObjects = null;

                var projectUsers = unitOfWork.ProjectUsers.Find(pu => pu.ProjectId == project.ProjectId);

                projectUsers = null;

                unitOfWork.Projects.Remove(project);

                unitOfWork.Complete();
            }
        }

        public static IEnumerable<DrawingCanvasObjectProxy> LoadProjectWithProjectName(string projectName, int userId)
        {
            var drawingCanvasObjectsProxies = new List<DrawingCanvasObjectProxy>();

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var project = unitOfWork.Projects.GetProjectWithProjectName(projectName, userId);

                var projectDrawingCanvasObjects = unitOfWork.DrawingCanvasObjects.GetDrawingCanvasObjectsBelongingTo(project.ProjectId);

                foreach (var drawingObject in projectDrawingCanvasObjects)
                {
                    drawingCanvasObjectsProxies.Add(ModelToProxyConverter.DrawingCanvasObjectToDrawingCanvasObjectProxy(drawingObject));
                }
            }

            return drawingCanvasObjectsProxies;
        }

        public static async Task RemoveSharedUserAsync(string sharedUserEmailAddress, int projectId)
        {
            await Task.Run(() =>
            {
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var sharedUserId = unitOfWork.Users.GetUserWithEmailAddress(sharedUserEmailAddress).UserId;

                    unitOfWork.ProjectUsers.RemoveProjectUser(sharedUserId, projectId);

                    unitOfWork.Complete();
                }
            });
        }

        public static async Task EditSharedUserDetailsAsync((string, bool) newInfo, int sharedProjectId)
        {
            await Task.Run(() =>
            {
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var sharedUserId = unitOfWork.Users.GetUserWithEmailAddress(newInfo.Item1).UserId;
                    var canEdit = newInfo.Item2;

                    var ProjectUser = unitOfWork.ProjectUsers.GetProjectUser(sharedUserId, sharedProjectId);

                    if (ProjectUser != null)
                    {
                        ProjectUser.CanEdit = canEdit;
                    }

                    unitOfWork.Complete();
                }
            });
        }

        public static IEnumerable<ProjectProxy> LoadUserProjectsFromDatabase(UserProxy userProxy)
        {
            var userProjects = new List<ProjectProxy>();

            var userModel = ProxyToModelConverter.UserProxyToUserModelConverter(userProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var userCreatedProjects = unitOfWork.Projects.GetUserCreatedProjects(userModel.UserId);

                foreach (var projectModel in userCreatedProjects)
                {
                    var projectProxy = ModelToProxyConverter.ProjectModelToProjectProxyConverter(projectModel);
                    userProjects.Add(projectProxy);
                }
                unitOfWork.Complete();
            }

            return userProjects;
        }

        public static IEnumerable<DrawingCanvasObjectProxy> LoadProjectDrawingCanvasObjects(ProjectProxy projectProxy)
        {
            var drawingCanvasObjectsProxies = new List<DrawingCanvasObjectProxy>();

            var projectModel = ProxyToModelConverter.ProjectProxyToProjectModelConverter(projectProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var drawingCanvasObjectModels = unitOfWork.DrawingCanvasObjects
                    .GetDrawingCanvasObjectsBelongingTo(projectModel.ProjectId);

                foreach (var drawingObj in drawingCanvasObjectModels)
                {
                    var drawingObjProxy = ModelToProxyConverter.DrawingCanvasObjectToDrawingCanvasObjectProxy(drawingObj);
                    drawingCanvasObjectsProxies.Add(drawingObjProxy);
                }

                unitOfWork.Complete();
            }

            return drawingCanvasObjectsProxies;
        }

        public static UserProxy GetProjectCreatorWithProjectName(string projectName)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(unitOfWork.Projects.GetCreator(projectName));
                return userProxy;
            }
        }

        public static async Task SaveProjectDrawingCanvasObjectsToDBAsync(IEnumerable<DrawingCanvasObjectProxy> drawingCanvasObjectProxies,
            ProjectProxy projectProxy)
        {
            await Task.Run(() => SaveProject());

            void SaveProject()
            {
                eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Saving project to database!");

                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var previousDrawingComponents = unitOfWork.DrawingCanvasObjects.GetDrawingCanvasObjectsBelongingTo(projectProxy.ProjectId);

                    foreach (var previousDrawingComponent in previousDrawingComponents)
                    {
                        unitOfWork.DrawingCanvasObjects.Remove(previousDrawingComponent);
                    }

                    foreach (var drawingCanvasObjectProxy in drawingCanvasObjectProxies)
                    {
                        var drawingCanvasObjectModel = ProxyToModelConverter
                            .DrawingCanvasObjectProxyToDrawingCanvasObjectModelConverter(drawingCanvasObjectProxy);

                        drawingCanvasObjectModel.Project = unitOfWork.Projects.Get(projectProxy.ProjectId);

                        unitOfWork.DrawingCanvasObjects.Add(drawingCanvasObjectModel);
                        unitOfWork.Complete();
                    }


                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Project saved to database!");
                    Thread.Sleep(2000);
                    eventAggregator.GetEvent<ChangeStatusbarMessageEvent>().Publish("Ready!");
                }
            }
        }

        public static IEnumerable<ProjectUserProxy> GetProjectUsersOfProject(int id)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var projectUsers = new List<ProjectUserProxy>();
                var projectUsersModel = unitOfWork.ProjectUsers.Find(pu => pu.ProjectId == id);
                foreach (var projectUser in projectUsersModel)
                {
                    projectUsers.Add(ModelToProxyConverter.ProjectUserModelToProjectUserProxy(projectUser));
                }

                return projectUsers;
            }
        }

        public static ProjectProxy LoadProjectFromDatabase(UserProxy userProxy, string projectName)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                //Move to IProjectRepository later on
                var projects = unitOfWork.Projects.Find(p => p.ProjectCreator.UserEmailAddress == userProxy.UserEmailAddress
                && p.ProjectName == projectName)
                    .ToList();

                if (projects.Count != 0)
                {
                    unitOfWork.Complete();
                    return ModelToProxyConverter.ProjectModelToProjectProxyConverter(projects[0]);
                }
                unitOfWork.Complete();
                return null;
            }
        }


        public static async Task AddSharedUserToDatabaseAsync(ProjectUserProxy projectSharedUser)
        {
            //var projectUserModel = 
            await Task.Run(() =>
            {
                using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
                {
                    var projectUserModel = ProxyToModelConverter.ProjectUserProxyToProjectUserModelConverter(projectSharedUser);
                    unitOfWork.ProjectUsers.Add(projectUserModel);
                    unitOfWork.Complete();
                }
            });

        }

        public static ProjectProxy GetProjectWithProjectName(string projectName, int projectCreatorId)
        {
            ProjectProxy projectProxy;

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                projectProxy = ModelToProxyConverter.ProjectModelToProjectProxyConverter(unitOfWork.Projects.GetProjectWithProjectName(projectName, projectCreatorId));
                unitOfWork.Complete();
            }

            return projectProxy;
        }

        public static IEnumerable<ProjectUserProxy> LoadUserSharedProjectsFromDatabase(UserProxy pUserProxy)
        {
            var userSharedProjects = new List<ProjectUserProxy>();

            var userModel = ProxyToModelConverter.UserProxyToUserModelConverter(pUserProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var sharedProjects = unitOfWork.ProjectUsers.Find(pu => pu.UserId == pUserProxy.UserId);

                foreach (var sharedProject in sharedProjects)
                {
                    var projectProxy = ModelToProxyConverter.ProjectUserModelToProjectUserProxy(sharedProject);
                    userSharedProjects.Add(projectProxy);
                }
                unitOfWork.Complete();
            }

            return userSharedProjects;
        }

        public static ProjectProxy GetProjectWithId(int projectId)
        {
            ProjectProxy projectProxy;

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                projectProxy = ModelToProxyConverter.ProjectModelToProjectProxyConverter(unitOfWork.Projects.Get(projectId));
                unitOfWork.Complete();
            }

            return projectProxy;
        }

        public static int GetProjectCreatorIdWithProjectId(int projectId)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                return unitOfWork.Users.GetProjectCreator(projectId).UserId;
            }
        }

        public static UserProxy GetUserWithUserId(int userId)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var userProxy = unitOfWork.Users.Get(userId);

                return ModelToProxyConverter.UserModelToUserProxyConverter(userProxy);
            }
        }
    }
}
