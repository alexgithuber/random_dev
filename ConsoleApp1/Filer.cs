//using System.Diagnostics;
using System.Data;
using static TimeIt;

public class Filer
{
    public static bool WriteLog = false;
    public static int totalFiles=-1;
    public static int countFile = 0;
    public static void go()
    {   //
        // tagging= group#_elem#_FileName.ext
        Console.WriteLine("bySize");
        Task.Run(() => ListFile(@"C:\Users\alxhb\Desktop\wes\test", @"C:\Users\alxhb\Desktop\wes2", keepDirectoryStructure: true, byName: false, Simulation: true));

        Console.WriteLine("byName");
        Task.Run(() => ListFile(@"C:\Users\alxhb\Desktop\wes\test", @"C:\Users\alxhb\Desktop\wes2", keepDirectoryStructure: true, byName: true, Simulation: true));

        //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
        //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
    }


    public static void ListFile(string SourceDirectory, string TargetDirectory, bool keepDirectoryStructure, bool byName, bool Simulation, CancellationToken? ct = null)
    {

        Console.WriteLine("Scanning Files");
        tic();
        List<FileObject> FileList = ScanFiles(SourceDirectory);
        Console.WriteLine("total_f Files{0}", FileList.Count);
        toc();



        var dubByName = FileList.GroupBy(fo => fo.FileNameWE).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
        var dubByLenght = FileList.GroupBy(fo => fo.Length).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
        tic();
        if (byName)
            MoveFile<string>(SourceDirectory, TargetDirectory, keepDirectoryStructure, Simulation, dubByName, ct).Wait();
        else
            MoveFile<long>(SourceDirectory, TargetDirectory, keepDirectoryStructure, Simulation, dubByLenght, ct).Wait();
        toc();


        static async Task MoveFile<T>(string SourceDirectory, string TargetDirectory, bool keepDirectoryStructure, bool Simulation, IOrderedEnumerable<IGrouping<T, FileObject>> dub, CancellationToken? ct = null)
        {
            ct?.ThrowIfCancellationRequested();
             totalFiles = dub.Aggregate(0, (now, next) => now + next.Count());
             countFile = 0;


            foreach (var group in dub.Select((group, i) => new { group, i })) // extension by int counter
            {
                ct?.ThrowIfCancellationRequested();
                //Console.WriteLine("#idx{0}: Key:{1} ", group.i, group.group.Key);
                foreach (var elem in group.group.Select((elem, i) => new { file = elem, i }))

                {
                    ct?.ThrowIfCancellationRequested();
                    await Task.Run(() =>
                    {
                        countFile++;

                        string TargetPath;
                        if (!keepDirectoryStructure)
                        {
                            TargetPath = Path.Combine(TargetDirectory, "D_" + group.i + "_" + elem.i + elem.file.FileName);
                        }
                        else
                        {
                            TargetPath = Path.Combine(TargetDirectory, Path.GetRelativePath(SourceDirectory, elem.file.Fullpath));
                        }
                        if (WriteLog) { WriteLog = false; Console.WriteLine("Moving ({0}/{1})", countFile, totalFiles); }
                        DoCopy(elem.file.Fullpath, TargetPath, Simulation);
                    }
                        );
                    //Console.WriteLine("\t#idx{0}:{1} {2}", group.i, elem.i,elem.file.Fullpath); 

                }

            }
     

            Console.WriteLine("Number of Files Moved: {0}", countFile);
            totalFiles = -1;
            countFile = 0;

        }
    }

    public static void DoCopy(string source, string target, bool Simulation = true)
    {
        if (!Simulation)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            _ = Directory.CreateDirectory(Path.GetDirectoryName(target));
#pragma warning restore CS8604 // Possible null reference argument.
            File.Move(source, target);
        }
        else
        {
            Thread.Sleep(1);
            Console.WriteLine("move file \n\t from {0} \n\t to {1}", source, target);
        }


    }


    private static List<FileObject> ScanFiles(string SourceDirectory)
    {
        List<FileObject> FileList;
        try
        {
            FileList = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories).
            Select(fn => new FileObject(fn)).ToList();



        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);

            FileList = new List<FileObject>();

        };
        return FileList;
    }
    private static List<FileObject> ScanFilesSafe(string SourceDirectory, bool recursive = true)
    {
        List<FileObject> FileList;
        try
        {
            FileList = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories).
            Select(fn => new FileObject(fn)).ToList();



        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);

            FileList = new List<FileObject>();

        };
        return FileList;
    }
}












