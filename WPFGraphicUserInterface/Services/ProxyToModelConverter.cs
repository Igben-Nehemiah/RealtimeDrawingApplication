using Models;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.Services
{
    public static class ProxyToModelConverter
    {
        public static User UserProxyToUserModelConverter(UserProxy userProxy)
        {
            var userModel = new User();

            userModel.UserFirstName = userProxy.UserFirstName;
            userModel.UserLastName = userProxy.UserLastName;
            userModel.UserEmailAddress = userProxy.UserEmailAddress;
            userModel.UserPassword = userProxy.UserPassword;
            userModel.UserId = userProxy.UserId;
            
            return userModel;
        }

        public static Project ProjectProxyToProjectModelConverter(ProjectProxy projectProxy)
        {
            var projectModel = new Project();

            projectModel.ProjectId = projectModel.ProjectId;
            //projectModel.ProjectCreator = UserProxyToUserModelConverter(projectProxy.ProjectCreator);
            projectModel.ProjectCreationDate = projectProxy.ProjectCreationDate;
            projectModel.ProjectId = projectProxy.ProjectId;
            projectModel.ProjectName = projectProxy.ProjectName;

            return projectModel;
        }

        public static DrawingCanvasObject DrawingCanvasObjectProxyToDrawingCanvasObjectModelConverter
            (DrawingCanvasObjectProxy drawingCanvasObjectProxy)
        {
            var drawingCanvasObjectModel = new DrawingCanvasObject();

            drawingCanvasObjectModel.XPosition = drawingCanvasObjectProxy.XPosition;
            drawingCanvasObjectModel.YPosition = drawingCanvasObjectProxy.YPosition;
            drawingCanvasObjectModel.Height = drawingCanvasObjectProxy.Height;
            drawingCanvasObjectModel.Width = drawingCanvasObjectProxy.Width;
            drawingCanvasObjectModel.ShapeFill = drawingCanvasObjectProxy.ShapeFill;
            drawingCanvasObjectModel.CanvasObjectGuid = drawingCanvasObjectProxy.CanvasObjectGuid;
            drawingCanvasObjectModel.CanvasObjectId = drawingCanvasObjectProxy.CanvasObjectId;
            drawingCanvasObjectModel.BorderFill = drawingCanvasObjectProxy.BorderFill;
            drawingCanvasObjectModel.CanvasObjectName = drawingCanvasObjectProxy.CanvasObjectName;
            //drawingCanvasObjectModel.ItemType = drawingCanvasObjectProxy.typ;

            return drawingCanvasObjectModel;
        }

        public static ProjectUser ProjectUserProxyToProjectUserModelConverter
            (ProjectUserProxy projectUserProxy)
        {
            var projectUser = new ProjectUser();

            projectUser.CanEdit = projectUserProxy.CanEdit;
            projectUser.UserId = projectUserProxy.UserId;
            projectUser.ProjectId = projectUserProxy.ProjectId;

            return projectUser;
        }
    }
}
