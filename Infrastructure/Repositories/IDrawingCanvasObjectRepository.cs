using Models;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public interface IDrawingCanvasObjectRepository : IRepository<DrawingCanvasObject>
    {
        IEnumerable<DrawingCanvasObject> GetDrawingCanvasObjectsBelongingTo(int projectId);
    }
}
