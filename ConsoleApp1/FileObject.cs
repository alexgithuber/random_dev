
public class FileObject
{

    private String fullpath_;

    public FileObject(string fullpath_)
    {
        this.fullpath_ = fullpath_;
    }

    public String Fullpath
    {
        get
        {
            return fullpath_;
        }
    }
    public String FileName
    {
        get
        {
            return Path.GetFileNameWithoutExtension(fullpath_);
        }

    }
    public String FileExt
    {
        get
        {
            return Path.GetExtension(fullpath_);
        }
    }

    public long Length
    {
        get { return new FileInfo(fullpath_).Length; }
    }
    public override string ToString()
    {
        return Fullpath;
    }

    public static void Test()
    {
        FileObject fi = new FileObject(@"c:\DumpStack.log");
        Console.WriteLine(fi.Fullpath);
        Console.WriteLine(fi.FileName);
        Console.WriteLine(fi.FileExt);
        Console.WriteLine(fi.Length);


    }
}


