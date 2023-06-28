using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
namespace WPFLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class RMedia : UserControl
    {
        public RMedia()
        {
            InitializeComponent();
        }

        bool ispaused = true;
        public void LoadFile(string f)
        {
            if (File.Exists(f))
            {
                me.Source = new Uri(f);
                me.Play();
                ispaused = false;
            }
            
        
        }
        public void Stop()
        {
            me.Stop();
            me.Close();
            me.Source = null;
            ispaused = true;
        }
     
        private void Me_failed(object sender, RoutedEventArgs e)
        {
            me.Stop();
            ispaused = true;
        }
        private void Stop_click(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        private void Pause_click(object sender, RoutedEventArgs e)
        {
            if (!me.HasVideo)
                { return; }
            if (!ispaused)
            {
                me.Pause();
                ispaused= true;
            }
            else
            {
                me.Play();
                ispaused= false;    
            }
        }
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (me.HasVideo)
            {
                int time_ms = (int)(me.NaturalDuration.TimeSpan.TotalMilliseconds * args.NewValue);

                TimeSpan ts = new(0, 0, 0, 0,time_ms);
                me.Position = ts;
            }
        }



    }
}
