using System.IO;
using System;

public class FileObject
{

    private String fullpath;
    private long length;
    public FileObject(string fullpath)
    {
        this.fullpath = fullpath;
        this.length= new FileInfo(fullpath).Length;
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

    public long Length
    {
        get { return length; }
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


