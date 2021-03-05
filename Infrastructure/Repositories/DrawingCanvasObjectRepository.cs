using Models;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class DrawingCanvasObjectRepository : Repository<DrawingCanvasObject>, IDrawingCanvasObjectRepository
    {
        public DrawingCanvasObjectRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }
        public RealtimeDrawingApplicationContext RealtimeDrawingApplicationContext
        {
            get { return Context as RealtimeDrawingApplicationContext; }
        }

        public IEnumerable<DrawingCanvasObject> GetDrawingCanvasObjectsBelongingTo(int projectId)
        {
            return RealtimeDrawingApplicationContext.DrawingCanvasObjects
                .Where(dco => dco.Project.ProjectId == projectId)
                .ToList();
        }
    }
}
