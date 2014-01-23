using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace KeyCodeCapture.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private KeyCapturer _keyCapturer;
        private ObservableCollection<Tuple<int, string, int>> _pressedKeys;

        public ObservableCollection<Tuple<int, string, int>> PressedKeys
        {
            get { return _pressedKeys; }
            set { _pressedKeys = value; RaisePropertyChanged(() => PressedKeys); }
        }

        public MainViewModel()
        {
            _keyCapturer = new KeyCapturer(UpdateKey);
            PressedKeys = new ObservableCollection<Tuple<int, string, int>>();
        }

        public RelayCommand SaveKeyCodes
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        FileName = "KeyCapture " + DateTime.Now.ToString("s").Replace(':',' '),
                        DefaultExt = ".csv"
                    };

                    if (saveDialog.ShowDialog().GetValueOrDefault())
                    {
                        using (var sw = new StreamWriter(saveDialog.FileName, append:false))
                        {
                            PressedKeys.ToList().ForEach(pk => sw.WriteLine("{0},{1}",pk.Item1,pk.Item2));
                        }
                    }
                });
            }
        }

        public RelayCommand Clear
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if ((MessageBox.Show("Are you sure?", "Clear key history", MessageBoxButton.OKCancel)) == MessageBoxResult.OK)
                        PressedKeys.Clear();
                });
            }
        }

        private void UpdateKey(int vkCode)
        {
            PressedKeys.Add(Tuple.Create(vkCode, GetKeyName(vkCode), PressedKeys.Count));
        }

        private string GetKeyName(int vkCode)
        {
            return KeyInterop.KeyFromVirtualKey(vkCode).ToString();
        }
    }
}