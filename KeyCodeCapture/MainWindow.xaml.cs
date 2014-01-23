using System.Windows;

namespace KeyCodeCapture
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Prevent space and enter from triggering button presses if they have keyboard focus
            SaveButton.PreviewKeyDown += (sender, args) => { args.Handled = true; };
            ClearButton.PreviewKeyDown += (sender, args) => { args.Handled = true; };
        }
    }
}
