﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace ColorDict.Editor.UI.Units
{
    public class PaletteBox : ListBox
    {
        static PaletteBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaletteBox), new FrameworkPropertyMetadata(typeof(PaletteBox)));
        }

        public static readonly DependencyProperty SelectionCommandProperty = DependencyProperty.Register("SelectionCommand", typeof(ICommand), typeof(PaletteBox));

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PaletteBoxItem();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            SelectionCommand?.Execute(SelectedItem);
        }

        public ICommand SelectionCommand
        {
            get { return (ICommand)this.GetValue(SelectionCommandProperty); }
            set { this.SetValue(SelectionCommandProperty, value); }
        }
    }
}
