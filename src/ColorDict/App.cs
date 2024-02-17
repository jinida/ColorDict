using ColorDict.Forms.UI.Views;
using Jamesnet.Wpf.Controls;
using Prism.Ioc;
using System.Windows;

namespace ColorDict
{
    internal class App : JamesApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ColorWindow>();
        }

    }
}
