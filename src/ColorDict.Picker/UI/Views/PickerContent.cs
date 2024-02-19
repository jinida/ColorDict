using Jamesnet.Wpf.Controls;
using System.Windows;

namespace ColorDict.Picker.UI.Views
{
    public class PickerContent : JamesContent
    {
        static PickerContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PickerContent), new FrameworkPropertyMetadata(typeof(PickerContent)));
        }

    }
}