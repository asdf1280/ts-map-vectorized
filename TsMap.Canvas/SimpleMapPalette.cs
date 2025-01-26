using System.Drawing;

namespace TsMap.Canvas
{
    public class SimpleMapPalette : MapPalette
    {
        public SimpleMapPalette()
        {
            Background = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
            Road = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
            PrefabRoad = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
            PrefabLight = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
            PrefabDark = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
            PrefabGreen = new SolidBrush(Color.FromArgb(180, 255, 255, 255)); // TODO: Check if green has a specific z-index

            CityName = new SolidBrush(Color.FromArgb(255, 224, 112));

            FerryLines = new SolidBrush(Color.FromArgb(80, 255, 255, 255));

            Error = Brushes.LightCoral;
        }
    }
}
