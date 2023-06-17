//using System.Diagnostics;
using System.Data;
using static TimeIt;

public class Filer
{
    public static async Task go()
    {   //
        // tagging= group#_elem#_FileName.ext
        await Task.Run(()=>ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, true, true));
        //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
        //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
    }


    public static async Task ListFile(string SourceDirectory, string TargetDirectory, bool keepDirectoryStructure, bool byName, bool Simulation)
    {

        Console.WriteLine("Scanning Files");
        tic();
        List<FileObject> FileList = ScanFiles(SourceDirectory);
        toc();
        Console.WriteLine("total_f Files{0}", FileList.Count);
        
                var dubByName = FileList.GroupBy(fo => fo.FileName).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
                var dubByLenght = FileList.GroupBy(fo => fo.Length).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
        
       
        
        foreach (var group in dubByLenght.Select((group, i) => new {group,i}))
        {
            //Console.WriteLine("#idx{0}: Key:{1} ", group.i, group.group.Key);
            foreach (var elem in group.group.Select((elem, i) => new { x = elem, i }))

            {
                await Task.Run(() => DoCopy(elem.x.Fullpath, "tooox"));
                //Console.WriteLine("\t#idx{0}:{1} {2}", group.i, elem.i,elem.x.Fullpath); 
              
            }
            
        }
        Console.WriteLine("Finnished");
    }

    public static void DoCopy(string source, string target, bool Simulation = true)
    {
        int x = 0;
        if (Simulation) { 
            Console.WriteLine("sim move file from{0} to {1}", source, target);
        Thread.Sleep(1); 
        }
        else File.Copy(source, target);
    
    
    }


    private static List<FileObject> ScanFiles(string SourceDirectory)
    {
        List<FileObject> FileList;
        try
        {
            FileList = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories).
            Select(fn => new FileObject(fn)).ToList();

            toc();


        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);

            FileList = new List<FileObject>();

        };
        return FileList;
    }
}








    



