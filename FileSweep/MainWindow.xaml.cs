using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.IO;
using WPFLib;
using System.Runtime;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace FileSweep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IEnumerator<FileObject> files = new List<FileObject>().GetEnumerator();
        public MainWindow()
        {
            InitializeComponent();

            Console.SetOut(new TextBoxConsole(tb1));
#if DEBUG
            SourceDirectory.Folder = @"C:\Users\alxhb\Desktop\wes";
            WorkDirectory.Folder = @"C:\Users\alxhb\Desktop\wes\data";
            TargetDirectory.Folder = @"C:\Users\alxhb\Desktop\wes123";

#endif
        }
        public string delete_dir = "_delete";
        public string stage_dir = "_stage";
        public string numeric_dir = @"_num\n_";

        private FileObject? CurrentFile;
        private FileObject? LastFile;
    
        private void NextFile()
        {
            if (CurrentFile is not null)
            {
                LastFile = CurrentFile;
            }
            if (files is null)
            {
                CurrentFile = null;
            }
            else if (files.MoveNext())
                {
                    CurrentFile = files.Current;
                UpdateUI();
            }
                else {
                    CurrentFile = null;
                UpdateUI();
            }
        }
        private void RevisitLastFile_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            if (LastFile is not null && CurrentFile is not null)
            { 
                (LastFile, CurrentFile) = (CurrentFile, LastFile);
                UpdateUI();
            }
        }
        private void Stage_Click(object? sender = null, RoutedEventArgs? e = null)
        {
           
                    MoveCurrentFileToDir(stage_dir);
           

           
        }


        private void Delete_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            MoveCurrentFileToDir(delete_dir);
        }
        private void Push_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            if (CurrentFile is not null)
            {
                string TargetDir = "";
                if (keepDirectoryStructure.IsChecked.GetValueOrDefault(false))
                {
                    TargetDir = Path.GetRelativePath(SourceDirectory.Folder, CurrentFile.Directory);
                }
                MoveCurrentFileToDir(TargetDir);
                }
        }

    

        private void PressNum(int num)
        {
            MoveCurrentFileToDir(numeric_dir + num);

          
        }

        private void Skip_Click(object? sender = null, RoutedEventArgs? e = null)
        {
            NextFile();
        }

        private void MoveCurrentFileToDir(string target_dir)
        {
            me.Stop();
            if (CurrentFile is not null)
            {
                string target = Path.Join(TargetDirectory.Folder, target_dir, CurrentFile.FileName);
                if (File.Exists(target))
                {
                    Console.WriteLine($"File Exists at Target {target}  (Source {CurrentFile.Fullpath})");
                    CurrentFile = new(target);
                    NextFile();
                }

                try
                {

                    Filer.DoMove(CurrentFile.Fullpath, target, Simulate.IsChecked.GetValueOrDefault(true));
                }
                catch (IOException)
                {
                    Thread.Sleep(30);
                    if (!(File.Exists(target) && !File.Exists(CurrentFile.Fullpath))) // file was copied depite error (..)
                        Console.WriteLine($"#######Repeated: File {CurrentFile.Fullpath}  to {target} was not copied ");
                   //else = ok.
                }
                CurrentFile = new(target);
                NextFile();
            }
        }
        private void UpdateUI()
        {
            if (CurrentFile is not null)
            {
                FileName.Text = CurrentFile.Directory;
                Folder.Text = CurrentFile.FileName;
                FileSize.Content = "Size: "+ CurrentFile.LengthStr();
                FileDate.Content = $"Date {CurrentFile.TimeStr()}";
                me.LoadFile(CurrentFile.Fullpath);

               
            }
            else
            {
                Folder.Text = "";
                FileName.Text = "No File";
                me.Stop();
    

            }
            
        }    
        
        private void Load_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog FD = new();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                me.LoadFile(FD.FileName);
            }

        }
        private void Init_Click(object sender, RoutedEventArgs e)
        {
            tb1.Clear();
            if (!Directory.Exists(SourceDirectory.Folder))
                {
                Console.WriteLine($"Source Directory {SourceDirectory.Folder} not found");
                return;
                }
            if (!Directory.Exists(WorkDirectory.Folder))
            {
                Console.WriteLine($"Work Directory {WorkDirectory.Folder} not found");
                return;
            }
            if (!Directory.Exists(TargetDirectory.Folder))
            {
                Console.WriteLine($"Target Directory {TargetDirectory.Folder} not found");
                return;
            }
            files = Filer.ScanFiles(WorkDirectory.Folder, true, false).GetEnumerator();
            Console.WriteLine($"number of Files={Filer.ScanFiles(WorkDirectory.Folder, true, false).Count()}");
            
            Skip_Click();
        }
        private void Copy_log(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tb1.Text);
        }

       
             private void ProcessKey(object sender, System.Windows.Input.KeyEventArgs e)
             {
                switch (e.Key)
                {
                    case Key.Up:
                        Stage_Click(sender, e);
                    break;
                    case Key.Left:
                        Delete_Click(sender, e);
                    break;
                    case Key.Right:
                        Push_Click(sender, e); 
                    break;
                    case Key.Down:
                        Skip_Click(sender, e);
                    break;
                    case Key k_i when k_i >= Key.NumPad0 && k_i <= Key.NumPad9:
                        PressNum((int)k_i - (int)Key.NumPad0);
                    break;
                    case Key k_i when k_i >= Key.D0 && k_i <= Key.D9:
                        PressNum((int)k_i - (int)Key.D0);
                    break;
                }
            e.Handled = true;


             }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += ProcessKey;
        }

     
    }
}
