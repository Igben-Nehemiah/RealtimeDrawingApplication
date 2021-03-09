using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WPFGraphicUserInterface.Services
{
    public static class BrushConverterHelper
    {
        static List<Brush> BrushesColours { get; set; } = BrushesProvider.GetBrushes();
        static List<string> BrushesNames { get; set; } = BrushesProvider.GetBrushNames();

        public static string ConvertToString(Brush brush)
        {
            var index = BrushesColours.IndexOf(brush);
            return BrushesNames[index];
        }

        public static Brush ConvertToBrush(string brushName)
        {
            var index = BrushesNames.IndexOf(brushName);
            return BrushesColours[index];
        }
    }

}
