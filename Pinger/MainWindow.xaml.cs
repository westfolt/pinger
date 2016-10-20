using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;

namespace Pinger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task pingTask;
        private NotifyIcon notifyIcon = new NotifyIcon();
        private PingLogic pingIt;
        System.Drawing.Icon greenIcon = Pinger.Properties.Resources.greenIcon;
        System.Drawing.Icon yellowIcon = Pinger.Properties.Resources.yellowIcon;
        System.Drawing.Icon redIcon = Pinger.Properties.Resources.redIcon;
        private RunOnStartup startup;

        public MainWindow()
        {
            InitializeComponent();
            this.notifyIcon.Icon = greenIcon;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            pingIt = new PingLogic(WebAddressInput.Text);
            startup = new RunOnStartup();
            checkBoxLaunch.IsChecked = startup.IsStartup();

            Task.Factory.StartNew(Pinging);//фоновая задача
        }

        private void InterfaceUpdater(PingLogic pingIt)
        {
            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(255, 12, 255, 0));
            if (pingIt.Connected == false)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart) delegate()
                {
                    myBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    RoundIndicator.Fill = myBrush;
                    textBloxkPingAmount.Text = pingIt.RoundTrip;
                    notifyIcon.Icon = redIcon;
                    notifyIcon.Text = pingIt.Site + "\n" + pingIt.RoundTrip;
                });
            }
            else
            {
                switch (pingIt.SpeedShow)
                {
                    case 0:
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            myBrush = new SolidColorBrush(Color.FromArgb(255, 12, 255, 0));
                            RoundIndicator.Fill = myBrush;
                            textBloxkPingAmount.Text = pingIt.RoundTrip;
                            notifyIcon.Icon = greenIcon;
                            notifyIcon.Text = pingIt.Site + "\n" + pingIt.RoundTrip;
                        });
                        break;
                    case 1:
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            myBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                            RoundIndicator.Fill = myBrush;
                            textBloxkPingAmount.Text = pingIt.RoundTrip;
                            notifyIcon.Icon = yellowIcon;
                            notifyIcon.Text = pingIt.Site + "\n" + pingIt.RoundTrip;
                        });
                        break;
                    case 2:
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            myBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                            RoundIndicator.Fill = myBrush;
                            textBloxkPingAmount.Text = pingIt.RoundTrip;
                            notifyIcon.Icon = redIcon;
                            notifyIcon.Text = pingIt.Site + "\n" + pingIt.RoundTrip;
                        });
                        break;
                }
            }
        }
        private void Pinging()
        {
            while (true)
            {
                pingIt.Ping();
                InterfaceUpdater(pingIt);
                Thread.Sleep(500);
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;
        }
        private void ApplySettingsButton_Click(object sender, RoutedEventArgs e)
        {
            pingIt.Site = this.WebAddressInput.Text;
            if (checkBoxLaunch.IsChecked==true)
            {
                startup.SetStartup();
            }
            else
            {
                startup.RemoveStartup();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    this.ShowInTaskbar = false;
                    break;
            }
        }
    }
}
