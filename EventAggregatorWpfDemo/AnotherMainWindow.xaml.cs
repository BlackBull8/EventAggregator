using System.Windows;
using EventAggregator;

namespace EventAggregatorWpfDemo
{
    /// <summary>
    ///     AnotherMainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnotherMainWindow
    {
        public AnotherMainWindow()
        {
            InitializeComponent();

            this.Subscribe(App.EventHub, Consts.StartUp, message => { Show(); });
        }

        private void AnotherMainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}