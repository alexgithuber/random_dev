
using System.Runtime.InteropServices;
using System.Text;

public class FileObject
{

    private readonly String fullpath;
    private readonly long? length;
    public readonly bool exists;
    public readonly DateTime? LastWriteTime;
    public FileObject(string fullpath)
    {
        this.fullpath = fullpath;
        FileInfo fi=new(fullpath);
        exists= File.Exists(fullpath);
        this.length = exists?fi.Length:null;
        this.LastWriteTime = exists?fi.LastWriteTime:null;
    }
   
    public String Fullpath
    {
        get
        {
            return fullpath;
        }
    }
    public String FileNameWE
    {
        get
        {
            return Path.GetFileNameWithoutExtension(fullpath);
        }

    }
    public String FileName
    {
        get
        {
            return Path.GetFileName(fullpath);
        }

    }
    public String FileExt
    {
        get
        {
            return Path.GetExtension(fullpath);
        }
    }
    public String Directory
    {
        get
        {
#pragma warning disable CS8603 // Possible null reference return.
            return Path.GetDirectoryName(fullpath);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    public long Length
    {
        get { return length.GetValueOrDefault(0); }
    }
    
    
    [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
    private static extern long StrFormatByteSize(
        long fileSize
        , [MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer // change from LPWStr LPTstr
        , int bufferSize);

   
    public static string StrFormatByteSize(long filesize)
    {
        StringBuilder sb = new(11);
        StrFormatByteSize(filesize, sb, sb.Capacity);
        return sb.ToString();
    }


    public string LengthStr() => this.length.ToString() ?? "";
    public string TimeStr() => LastWriteTime.ToString() ?? "";
    public override string ToString()
    {
        return Fullpath;
    }

    public static void Test()
    {
        FileObject fi = new(@"c:\DumpStack.log");
        Console.WriteLine(fi.Fullpath);
        Console.WriteLine(fi.FileName);
        Console.WriteLine(fi.FileExt);
        Console.WriteLine(fi.Length);


    }
}


