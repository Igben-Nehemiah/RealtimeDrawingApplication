using Models;

namespace Infrastructure.Repositories
{
    public class DrawingCanvasObjectRepository : Repository<DrawingCanvasObject>, IDrawingCanvasObject
    {
        public DrawingCanvasObjectRepository(RealtimeDrawingApplicationContext context) : base(context)
        {
        }
    }
}
