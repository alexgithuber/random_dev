using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
namespace WPFLib
{
    public partial class DirectFolder : UserControl
    {
        public DirectFolder()
        {
            this.DataContext = this;
            InitializeComponent();
            folder = this.Name;
        }
        private string folder;
        public string Folder
        {
            get { return folder; }
            set
            {
                folder = value;
                if (DC2.Text != value)
                { DC2.Text = value; };
               
                if (System.IO.Directory.Exists(value))
                {
                    
                   

                }
            }
        }

        private void ChooseDirectory(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog FD = new();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Folder = FD.SelectedPath;
            }




        }

        private void DC_TextChanged(object sender, TextChangedEventArgs e)
        {
            Folder = DC2.Text;
        
        }
        //public string xFolder { Get; Set };
        /* private void OnDirectoryClick(object sender, RoutedEventArgs e)
         {
            // System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
             //if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
             {
              //   Directory = folder.SelectedPath;
             }


         }*/
    }

}