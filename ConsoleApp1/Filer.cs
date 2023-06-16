
using System;
//using System.Diagnostics;
using System.IO;
using static TimeIt;
using static System.Math;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Numerics;


public class Filer
    {
    public static void go()
    {   //
      
        ListFile();
    }
    

    public static void ListFile(string SourceDirectory = @"C:\Users\alxhb\Desktop\wes\test", string TargetDirectory = @"C:\Users\alxhb\Desktop\wes2\")
    {

        
        Console.WriteLine(SourceDirectory);
        List<FileObject> FileList;
        tic();
        try
        { FileList = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories).
            Select(fn => new FileObject(fn)).ToList();
            Console.WriteLine("total_f Files{0}", FileList.Count);
            toc();


        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            return;
            //FileList = new List<FileObject>();

        };
        var dupByName = FileList.GroupBy(fo => fo.FileName).Where(go => go.Count() >= 2).SelectMany(go => go).ToList();
        var dupByLenght = FileList.GroupBy(fo => fo.Length).Where(go => go.Count() >= 2).SelectMany(go => go).ToList();


        ListHelper.WriteLine(FileList);
        ListHelper.WriteLine(dupByName);
        ListHelper.WriteLine(dupByLenght);
   
        int count = 0;
        dupByLenght.ForEach(SourceFile => {
             string TargetFile = SourceFile.FileName+"_"+count.ToString()+ SourceFile.FileExt;
            TargetFile.ToString();
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            new FileInfo(TargetFile).Directory.Create();
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            Console.WriteLine(SourceFile.Fullpath + "to" + TargetFile);
            try { File.Copy(SourceFile.Fullpath, TargetFile, true); } catch { Console.WriteLine("File " + SourceFile.Fullpath + "not transfered"); };
             count++;   
         });
        Console.WriteLine("moved Files{0}", count);
        //dupByName_red.ForEach(targetFile => { System.IO.FileInfo("c:\\stuff\\a\\file.txt").Directory.Create(); };// File.Move(elem.Fullpath,TargetDirectory + Path.DirectorySeparatorChar + Path.GetRelativePath(SourceDirectory, elem.Fullpath)});



        //IEnumerable<int> duplicateItems = from file in FileList where Path.GetExtension(file).Equals("txt") select file.Length;



        //Console.WriteLine(FileList)
        /*
        FileList.ForEach(delegate(string name)
        {
            Console.WriteLine( "x|{0}",name);
        }


        Console.WriteLine("doubles {0}",duplicateItems);
        duplicateItems.ForEach(delegate (string name)
        {
            Console.WriteLine("dup|{0}", name);
        }*/
    }








    //public static (List<T>,List<int>) FindDupes<T>(List<T> input)
    public static List<int> FindDupes<T>(List<T> input)
        {
            return new List<int>();// new List<T>();

        }
        //             Situations:  Same name
        //             Situation: same size







        static string DisplayFile(FileSystemInfo fsi, int count)


        {
            {
                //  Assume that this entry is a file.
                string entryType = "File";

                // Determine if entry is really a directory
                if ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    entryType = "Directory";
                }
                //  Show this entry's type, name, and creation date.
                return string.Format("{0}: entry {1} ", entryType, fsi.FullName, count);
            }
        }


    }
