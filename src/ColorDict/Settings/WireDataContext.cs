﻿using ColorDict.Editor.Local.ViewModels;
using ColorDict.Editor.UI.Views;
using ColorDict.Forms.Local.ViewModels;
using ColorDict.Forms.UI.Views;
using Jamesnet.Wpf.Global.Location;

namespace ColorDict.Settings
{
    internal class WireDataContext : ViewModelLocationScenario
    {
        protected override void Match(ViewModelLocatorCollection items)
        {
            items.Register<ColorWindow, ColorWindowViewModel>();
            items.Register<ColorDict.Editor.UI.Views.ColorEditor, ColorEditorViewModel>();
        }
    }
}
