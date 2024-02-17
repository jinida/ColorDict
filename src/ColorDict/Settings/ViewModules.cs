using ColorDict.Forms.UI.Views;
using Jamesnet.Wpf.Controls;
using Prism.Ioc;
using Prism.Modularity;

namespace ColorDict.Settings
{
    internal class ViewModules : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IViewable, MainContent>("MainContent");
        }
    }
}
