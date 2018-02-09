using System.Windows;
using EventAggregator;

namespace EventAggregatorWpfDemo
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly EventMessageHandler _eventMessageHandler;

        public MainWindow()
        {
            InitializeComponent();

            _eventMessageHandler = this.Subscribe(Consts.StartUp, message =>
            {
                Top = message.GetValue<int>("top");
                Left = message.GetValue<int>("left");
                Show();
                this.Unsubscribe(_eventMessageHandler);
            });
        }

        private void OpenAnotherMainWindow_OnClick(object sender, RoutedEventArgs e)
        {
            var anotherMainWindow = new AnotherMainWindow();
            anotherMainWindow.Show();
        }
    }
}