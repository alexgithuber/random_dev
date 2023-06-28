using System.Windows;
using System.Windows.Controls;


/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
namespace WPFLib
{
    public partial class DirectFolder : UserControl
    {
        public DirectFolder()
        {
            InitializeComponent();
            this.DataContext = this;
            folder = this.Name;
        }
        private string folder;
        public string Folder
        {
            get { return folder; }
            set
            {

                if (System.IO.Directory.Exists(value))
                {
                    folder = value;
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