
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Linq.Expressions;
using System.Timers;
using System.Configuration;
using WPFLib;
namespace WinFileSort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow : Window
    {
        private System.Timers.Timer? timer;
        CancellationTokenSource? ct_source;


        public MainWindow()
        {
            InitializeComponent();
               this.DataContext = this;
#if DEBUG
            SourceDirectory.Folder= @"C:\Users\alxhb\Desktop\wes2";
            TargetDirectory.Folder= @"C:\Users\alxhb\Desktop\wes";
           

#endif


            Console.SetOut(new TextBoxConsole(tb1));
        }

//}

       
  
        private async void BStart_Click(object sender, RoutedEventArgs e)
        {

            timer = new System.Timers.Timer
            {
                AutoReset = true,
                Interval = 1000,
                Enabled = true
            };
            timer.Elapsed += delegate (Object? source , ElapsedEventArgs ign) {
                this.Dispatcher.Invoke(() =>
                BStart.Content = String.Format("move {0}/{1}", Filer.countFile, Filer.totalFiles));  
            };
            if (!(Directory.Exists(SourceDirectory.Folder) && Directory.Exists(TargetDirectory.Folder)))
            {
                Console.WriteLine("One or both directories do not exist");
                return; }
            ct_source = new CancellationTokenSource();
            tb1.Clear();
            BStart.IsEnabled = false;

            {
                bool kDS= keepDirectoryStructure.IsChecked.GetValueOrDefault();
                bool bN= byName.IsChecked.GetValueOrDefault();
                bool Sim= Simulation.IsChecked.GetValueOrDefault();
                var sd = SourceDirectory.Folder;
                var td = TargetDirectory.Folder;
                    await Task.Run(() =>
                    {
                        try
                        {
                            Filer.ListFile(sd, td, kDS, bN, Sim, ct_source.Token);
                        }
                        catch (AggregateException)                            //Thread.Sleep(10);
                        {
                           
                            Console.WriteLine("Task was cancelled");
                           
                        }
                        this.Dispatcher.Invoke(() => tb1.ScrollToEnd());
                        this.Dispatcher.Invoke(() => BStart.IsEnabled = true);
                        this.Dispatcher.Invoke(() => BStart.Content="Start");
                        timer.Enabled = false;
                    });
                

            }
           
        }
        private void BStop_Click(object sender, RoutedEventArgs e)
        {
               ct_source?.Cancel();
        }

        private void CopyLog(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tb1.Text);
         
        }
    }

   
    }
