using Models;
using System;
using WPFGraphicUserInterface.ModelProxies;

namespace WPFGraphicUserInterface.Services
{
    public static class ModelToProxyConverter
    {
        public static UserProxy UserModelToUserProxyConverter(User userModel)
        {
            var userProxy = new UserProxy();

            userProxy.UserFirstName = userModel.UserFirstName;
            userProxy.UserLastName = userModel.UserLastName;
            userProxy.UserEmailAddress = userModel.UserEmailAddress;
            userProxy.UserPassword = userModel.UserPassword;
            userProxy.UserId = userModel.UserId;

            return userProxy;
        }

        public static ProjectProxy ProjectModelToProjectProxyConverter(Project projectModel)
        {
            var projectProxy = new ProjectProxy();

            projectProxy.ProjectName = projectModel.ProjectName;
            projectProxy.ProjectId = projectModel.ProjectId;
            //projectProxy.ProjectCreator

            return projectProxy;
        }

        public static DrawingCanvasObjectProxy DrawingCanvasObjectToDrawingCanvasObjectProxy(DrawingCanvasObject drawingObj)
        {
            var drawingObjProxy = new DrawingCanvasObjectProxy();

            drawingObjProxy.Height = drawingObj.Height;
            drawingObjProxy.Width = drawingObj.Width;
            drawingObjProxy.XPosition = drawingObj.XPosition;
            drawingObjProxy.CanvasObjectGuid = drawingObj.CanvasObjectGuid;
            drawingObjProxy.CanvasObjectName = drawingObj.CanvasObjectName;
            drawingObjProxy.BorderFill = drawingObj.BorderFill;
            drawingObjProxy.YPosition = drawingObj.YPosition;
            drawingObjProxy.ShapeFill = drawingObj.ShapeFill;

            return drawingObjProxy;
        }

        public static ProjectUserProxy ProjectUserModelToProjectUserProxy(ProjectUser projectUser)
        {
            var projectUserProxy = new ProjectUserProxy();

            projectUserProxy.CanEdit = projectUser.CanEdit;
            projectUserProxy.ProjectId = projectUser.ProjectId;
            projectUserProxy.UserId = projectUser.UserId;

            return projectUserProxy;
        }
    }
}
