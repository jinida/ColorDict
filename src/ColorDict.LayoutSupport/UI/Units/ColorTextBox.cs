using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ColorDict.LayoutSupport.UI.Units
{
    public class ColorTextBox : TextBox
    {
        static ColorTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorTextBox), new FrameworkPropertyMetadata(typeof(ColorTextBox)));
        }
        private string previousText;
        public ColorTextBox()
        {
            previousText = "";
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.TextChanged += TextBox_TextChanged;
            this.PreviewTextInput += TextBox_PreviewTextInput;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            
            Label label = this.Template.FindName("PART_Label", this) as Label;
            string contentName = (string) label.Content;

            int threshold = contentName == "V" || contentName == "S" ? 100 : contentName == "H" ? 360 : 255;
            try
            {
                if (int.Parse(textBox.Text) > threshold)
                {
                    SetValue(TextProperty, threshold.ToString());
                    CaretIndex = 3;
                }
                previousText = textBox.Text;
            }
            catch
            {
                if (textBox.Text != "")
                {
                    textBox.Text = previousText;
                }
                else
                {
                    textBox.Text = "";
                }
                CaretIndex = 3;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
