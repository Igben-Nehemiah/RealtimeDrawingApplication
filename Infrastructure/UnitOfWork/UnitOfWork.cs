using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealtimeDrawingApplicationContext _context;

        public UnitOfWork(RealtimeDrawingApplicationContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Projects = new ProjectRepository(_context);
            ProjectUsers = new ProjectUserRepository(_context);
            DrawingCanvasObjects = new DrawingCanvasObjectRepository(_context);
        }
        public IUserRepository Users { get; private set; }

        public IProjectRepository Projects { get; private set; }

        public IProjectUserRepository ProjectUsers { get; private set; }

        public IDrawingCanvasObject DrawingCanvasObjects { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProjectRepository Projects { get; }
        IProjectUserRepository ProjectUsers { get; }
        IDrawingCanvasObject DrawingCanvasObjects { get; }
        int Complete();
    }
}
