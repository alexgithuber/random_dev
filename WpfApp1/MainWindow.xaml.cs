
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Filer.go();
            //new OpenFileDialog().ShowDialog();
            tb1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            var progress = new Progress<string>();
            progress.ProgressChanged += (s, message) => tb1.AppendText(message);
            Console.SetOut(new ControlWriter(progress));
        }


        //+=(s,message)=>{
        //   tb1.Text+=message; 
        //}
        //}



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            tb1.ScrollToEnd();
            var task=Task.Run(() => {
                this.Dispatcher.Invoke(() => b1.IsEnabled = false);
                 Filer.go();
                this.Dispatcher.Invoke(() => b1.IsEnabled = true);
                });
          
            
        }
    }

    public class ControlWriter : TextWriter
    {
        private IProgress<string> progress;
        private StringBuilder sb;
        int printed = 0;
        public ControlWriter(IProgress<string> progress)
        {
            this.progress = progress;
            sb = new StringBuilder();
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
        public override string ToString()
        {
            return sb.ToString();
        }
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
    }
