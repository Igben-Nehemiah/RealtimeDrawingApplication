using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WPFGraphicUserInterface.Services
{
    public static class BrushesProvider
    {
        public static List<Brush> GetBrushes()
        {
            var brushDictionary = typeof(Brushes).GetProperties().
                    Select(p => new { Name = p.Name, Brush = p.GetValue(null) as Brush }).
                    ToList();

            return brushDictionary.Select(v => v.Brush).ToList();
        }

        public static List<string> GetBrushNames()
        {
            var brushDictionary = typeof(Brushes).GetProperties().
                    Select(p => new { Name = p.Name, Brush = p.GetValue(null) as Brush }).
                    ToList();
            return brushDictionary.Select(v => v.Name).ToList();
        }
    }

}
