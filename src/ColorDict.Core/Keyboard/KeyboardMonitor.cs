using ColorDict.Core.Helpers;

namespace ColorDict.Core.Keyboard
{
    public class KeyboardMonitor
    {
        private readonly PickerManager _pickerManager;
        private List<int> _currentlyPressedKeys = new List<int>();
        //private List<int> _activationKeys = new List<int>();
        private GlobalKeyboardHook _keyboardHook;

        public KeyboardMonitor(PickerManager pickerManager)
        {
            _pickerManager = pickerManager;
        }

        public void Start()
        {
            _keyboardHook = new GlobalKeyboardHook();
            _keyboardHook.KeyboardPressed += Hook_KeyboardPressed;
        }


        private void Hook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            var virtualCode = e.KeyboardData.VirtualCode;
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown || e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown)
            {
                if (!_currentlyPressedKeys.Contains(virtualCode))
                {
                    _currentlyPressedKeys.Add(virtualCode);
                }
            }
            else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp || e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyUp)
            {
                if (_currentlyPressedKeys.Contains(virtualCode))
                {
                    _currentlyPressedKeys.Remove(virtualCode);
                }
            }

            _currentlyPressedKeys.Sort();

            //if (ArraysAreSame(_currentlyPressedKeys, _activationKeys))
            //{
            //    _pickerManager.ShowPicker();
            //}

            if (_currentlyPressedKeys.Count == 1 && _currentlyPressedKeys[0] == 27)
            {
                _pickerManager.HidePicker();
            }
        }

        //private bool ArraysAreSame(List<int> first, List<int> second)
        //{
        //    if (first.Count != second.Count)
        //    {
        //        return false;
        //    }

        //    for (int i = 0; i < first.Count; i++)
        //    {
        //        if (first[i] != second[i])
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
