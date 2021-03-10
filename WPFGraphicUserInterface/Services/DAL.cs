using Infrastructure;
using Infrastructure.UnitOfWork;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.Services
{
    public static class DAL
    {
        public static bool IsApplicationUser(string email, out UserProxy userProxy)
        {

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var containsUser = unitOfWork.Users.ContainsUser(email);

                if (containsUser)
                {
                    var user = unitOfWork.Users.GetUserWithEmailAddress(email);
                    userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(user);

                    unitOfWork.Complete();
                    return true;
                }

                userProxy = null;
                unitOfWork.Complete();
                return false;
            }
        }
        public static bool IsValidUser(string email, string password, out UserProxy userProxy)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var user = unitOfWork.Users.GetUser(email, password);

                if (user != null)
                {
                    userProxy = ModelToProxyConverter.UserModelToUserProxyConverter(user);
                    unitOfWork.Complete();
                    return true;
                }

                userProxy = null;
                unitOfWork.Complete();
                return false;  
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

        public static void AddProjectToDatabase(ProjectProxy projectProxy)
        {
            //Convert project proxy to project model
            var newProject = ProxyToModelConverter.ProjectProxyToProjectModelConverter(projectProxy);

            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                unitOfWork.Projects.Add(newProject);
                unitOfWork.Complete();
            }

            //Gets the project and change the project creator
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                var project = unitOfWork.Projects.Get(newProject.ProjectId);
                project.ProjectCreator = ProxyToModelConverter.UserProxyToUserModelConverter(projectProxy.ProjectCreator);
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
                unitOfWork.Complete();
            }
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

                foreach(var drawingObj in drawingCanvasObjectModels)
                {
                    var drawingObjProxy = ModelToProxyConverter.DrawingCanvasObjectToDrawingCanvasObjectProxy(drawingObj);
                    drawingCanvasObjectsProxies.Add(drawingObjProxy);
                }

                unitOfWork.Complete();
            }

            return drawingCanvasObjectsProxies;
        }

        public static void SaveProjectDrawingCanvasObjectsToDB(IEnumerable<DrawingCanvasObjectProxy> drawingCanvasObjectProxies, 
            ProjectProxy projectProxy)
        {
            //Convert to drawingmodels
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
                foreach(var drawingCanvasObjectProxy in drawingCanvasObjectProxies)
                {
                    var drawingCanvasObjectModel = ProxyToModelConverter
                        .DrawingCanvasObjectProxyToDrawingCanvasObjectModelConverter(drawingCanvasObjectProxy);

                    drawingCanvasObjectModel.Project = unitOfWork.Projects.Get(projectProxy.ProjectId);

                    unitOfWork.DrawingCanvasObjects.Add(drawingCanvasObjectModel);
                }

                unitOfWork.Complete();
            }
        }

        public static ProjectProxy LoadProjectFromDatabase(UserProxy userProxy, string projectName)
        {
            using (var unitOfWork = new UnitOfWork(new RealtimeDrawingApplicationContext()))
            {
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

       
    }

}
