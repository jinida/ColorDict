using System.Windows.Controls;
using System.Windows;

namespace ColorDict.Editor.UI.Units
{
    public class PaletteBoxItem : ListBoxItem
    {
        static PaletteBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaletteBoxItem), new FrameworkPropertyMetadata(typeof(PaletteBoxItem)));
        }
    }
}
