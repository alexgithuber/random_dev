//using System.Diagnostics;
using System.Data;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;
using static TimeIt;


    public class Filer
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static bool WriteLog = false;

    public static int totalFiles = -1;
        public static int countFile = 0;
#pragma warning restore CA2211 // Non-constant fields should not be visible
    public static void Go()
        {
            // tagging= group#_elem#_FileName.ext
            /* Console.WriteLine("bySize");
            Task.Run(() => ListFile(@"C:\Users\alxhb\Desktop\wes\test", @"C:\Users\alxhb\Desktop\wes\test", keepDirectoryStructure: true, byName: false, Simulation: true)).Wait();

            Console.WriteLine("byName");
            Task.Run(() => ListFile(@"C:\Users\alxhb\Desktop\wes\test", @"C:\Users\alxhb\Desktop\wes\test", keepDirectoryStructure: true, byName: true, Simulation: true)).Wait();
            */
            Tic();
            ScanFiles_old(@"C:\Users\alxhb\Desktop\wes");
            Toc();
            Tic();
            var res = ScanFiles(@"C:\Users\alxhb\Desktop\wes", true, true).ToList();
            ListHelper.WriteLine(res);
            Toc();
            //ListHelper.WriteLine(x);


            //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
            //ListFile(@"C:\Users\alxhb\Desktop\wes", @"C:\Users\alxhb\Desktop\wes2", false, false, true);
        }


        public static void ListFile(string SourceDirectory, string TargetDirectory, bool keepDirectoryStructure, bool byName, bool Simulation, CancellationToken? ct = null)
        {

            Console.WriteLine("Scanning Files");
            Tic();
            List<FileObject> FileList = ScanFiles(SourceDirectory).ToList();
            Console.WriteLine("total_f Files{0}", FileList.Count);
            Toc();



            var dubByName = FileList.GroupBy(fo => fo.FileNameWE).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
            var dubByLenght = FileList.GroupBy(fo => fo.Length).Where(go => go.Count() >= 2).OrderBy(go => go.Key);
            Tic();
            if (byName)
                MoveFile(SourceDirectory, TargetDirectory, keepDirectoryStructure, Simulation, dubByName, ct).Wait();
            else
                MoveFile(SourceDirectory, TargetDirectory, keepDirectoryStructure, Simulation, dubByLenght, ct).Wait();
            Toc();


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
                            DoMove(elem.file.Fullpath, TargetPath, Simulation);
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

        public static void DoMove(string source, string target, bool Simulation = true)
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


        private static List<FileObject> ScanFiles_old(string SourceDirectory)
        {
            List<FileObject> FileList;
            try
            {
                FileList = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories).
                Select(fn => new FileObject(fn)).ToList();



            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception: " + ex.Message);
                Console.ResetColor();
                FileList = new List<FileObject>();

            };
            return FileList;
        }
        public static IEnumerable<FileObject> ScanFiles(string SourceDirectory, bool recursive = true, bool subDirectoryFirst = false)
        {

            List<string> DirectoryList;
            List<FileObject> FileListD = new();
            List<FileObject> FileListF = new();
            try
            {




                FileListF.AddRange(Directory.GetFiles(SourceDirectory)
                    .Select(fn => new FileObject(fn)));

                DirectoryList = Directory.GetDirectories(SourceDirectory).ToList();
                if (recursive)
                    foreach (var directory in DirectoryList)
                    {
                        FileListD.AddRange(ScanFiles(directory, recursive, subDirectoryFirst));
                    }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception: {ex.Message} in Folder{SourceDirectory}");
                Console.ResetColor();
            };
            if (subDirectoryFirst)
                return FileListD.Concat(FileListF);
            else
                return FileListF.Concat(FileListD);
        }
    }
