using Jamesnet.Wpf.Controls;
using System.Windows;

namespace ColorDict.Forms.UI.Views
{
    public class ColorWindow : JamesWindow
    {
        static ColorWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorWindow), new FrameworkPropertyMetadata(typeof(ColorWindow)));
        }
        
        public ColorWindow()
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            Title = "ColorDict";
            ResizeMode = ResizeMode.NoResize;
        }
    }
}