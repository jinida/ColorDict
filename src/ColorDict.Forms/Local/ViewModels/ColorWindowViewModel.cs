using CommunityToolkit.Mvvm.ComponentModel;
using Jamesnet.Wpf.Controls;
using Prism.Ioc;
using Prism.Regions;

namespace ColorDict.Forms.Local.ViewModels
{
    public class ColorWindowViewModel : ObservableObject, IViewLoadable
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;

        public ColorWindowViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        { 
            _containerProvider = containerProvider;
            _regionManager = regionManager;
        }

        public void OnLoaded(IViewable view)
        {
            IViewable mainContent = _containerProvider.Resolve<IViewable>("MainContent");
            IRegion mainRegion = _regionManager.Regions["MainRegion"];

            if (!mainRegion.Views.Contains(mainContent))
            {
                mainRegion.Add(mainContent);
            }
            mainRegion.Activate(mainContent);
        }
    }
}
