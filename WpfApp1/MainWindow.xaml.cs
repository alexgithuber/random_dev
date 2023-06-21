
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

namespace WpfApp1
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
            #if DEBUG
                SourceDirectory.Text = @"C:\Users\alxhb\Desktop\wes2";
                TargetDirectory.Text = @"C:\Users\alxhb\Desktop\wes";
            #endif
            var progress = new Progress<string>();
            progress.ProgressChanged += (s, message) => {
                tb1.AppendText(message);
                //tb1.ScrollToEnd();
            };
            Console.SetOut(new ControlWriter(progress));
        }

//}

       
  
        private async void BStart_Click(object sender, RoutedEventArgs e)
        {
            timer=new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += delegate (Object? source , ElapsedEventArgs ign) {
                this.Dispatcher.Invoke(() =>
                BStart.Content = String.Format("move {0}/{1}", Filer.countFile, Filer.totalFiles));  
            };
            if (!(Directory.Exists(SourceDirectory.Text) && Directory.Exists(TargetDirectory.Text)))
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
                var sd = SourceDirectory.Text;
                var td = TargetDirectory.Text;
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

        private void copy_log(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tb1.Text);
        }

        private void SourceDirectoryB_Click(object sender, RoutedEventArgs e)
        {
                System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
                if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SourceDirectory.Text = folder.SelectedPath;
                }
          
          //        ;
            
        }

        private void TargetDirectoryB_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetDirectory.Text = folder.SelectedPath;
            }
        }

        
    }

    public class ControlWriter : TextWriter
    {
        private IProgress<string> progress;
 
      
        public ControlWriter(IProgress<string> progress)
        {
            this.progress = progress;
     
        }

        public override void Write(char value)
        {
            progress.Report(value.ToString());
        }

        public override void Write(string? value)
        {
            if (value != null) {
                progress.Report(value);
            }
        }
   
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
    }
