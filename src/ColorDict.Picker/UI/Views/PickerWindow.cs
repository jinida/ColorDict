using ColorDict.Core.Behavior;
using ColorDict.Picker.Local.ViewModels;
using Jamesnet.Wpf.Controls;
using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace ColorDict.Picker.UI.Views
{
    public class PickerWindow : JamesWindow
    {
        static PickerWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PickerWindow), new FrameworkPropertyMetadata(typeof(PickerWindow)));
        }

        public PickerWindow()
        {
            var changeWindowPositionBehavior = new ChangeWindowPositionBehavior(((PickerViewModel)DataContext).getMouseInfoProvider());
            Interaction.GetBehaviors(this).Add(changeWindowPositionBehavior);
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
        }
    }
}