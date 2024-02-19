using ColorDict.Core.Helpers;
using ColorDict.Core.Interfaces;
using ColorDict.Core.Keyboard;
using ColorDict.Core.Mouse;
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
            var window = Container.Resolve<ColorWindow>();
            window.Closed += MainWindow_Closed;
            return window;
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            PickerManager pickerManager = new PickerManager();
            ColorProvider colorProvider = new ColorProvider();
            containerRegistry.RegisterInstance(pickerManager);
            containerRegistry.RegisterInstance<IColorProvider>(colorProvider);
            containerRegistry.RegisterInstance(new KeyboardMonitor(pickerManager));
            containerRegistry.RegisterInstance<IMouseInfoProvider>(new MouseInfoProvider(pickerManager, colorProvider));
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            PickerManager pickerManager = Container.Resolve<PickerManager>();
            if (pickerManager != null)
            {
                pickerManager.UnregisterPicker();
            }
            Environment.Exit(0);
        }
    }
}
