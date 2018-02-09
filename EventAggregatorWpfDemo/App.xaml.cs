using System.Collections.Generic;
using System.Windows;
using EventAggregator;

namespace EventAggregatorWpfDemo
{
    /// <summary>
    ///     App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public static EventHub EventHub { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EventHub = new EventHub();

            Current.MainWindow = new MainWindow();
            Current.MainWindow = new AnotherMainWindow();

            this.Send(Consts.StartUp, new KeyValuePair<string, object>("top",500), new KeyValuePair<string, object>("left",500));
            this.Send(EventHub, Consts.StartUp);
        }
    }
}