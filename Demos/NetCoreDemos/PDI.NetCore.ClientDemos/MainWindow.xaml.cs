using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PDI.NetCore.ClientDemos
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
