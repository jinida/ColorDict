using Jamesnet.Wpf.Controls;
using System.Windows;

namespace ColorDict.Editor.UI.Views
{
    public class ColorEditor : JamesContent
    {
        static ColorEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorEditor), new FrameworkPropertyMetadata(typeof(ColorEditor)));
        }
    }
}