using System.Collections.ObjectModel;

namespace ColorDict.Core.Models
{
    public class ExtractedColorCollection : ObservableCollection<ColorStampModel>
    {
        public void Insert(ColorStruct rgba)
        {
            Insert(0, new ColorStampModel(rgba));
            RemoveLast();
        }

        private void RemoveLast()
        {
            if (Count > 65)
            {
                RemoveAt(Count - 1);
            }
        }
    }
}
