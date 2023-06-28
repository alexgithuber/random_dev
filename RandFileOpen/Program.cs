// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var files = Filer.ScanFiles("work").ToList();
Random rd=new((int)DateTime.Now.Ticks & 0x0000FFFF);
if (files.Count == 0) return;

int idx = rd.Next(files.Count);

ProcessStartInfo psi = new()
{
    FileName = files[idx].Fullpath,
    UseShellExecute = true,
    WindowStyle = ProcessWindowStyle.Normal
};

Process.Start(psi);