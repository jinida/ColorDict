using System.Drawing;

namespace ColorDict.Core.Interfaces
{
    public interface IColorProvider
    {
        Color GetPixelColor(System.Windows.Point pixelPosition);
        Color GetAverageColor(Rectangle area);
    }
}
