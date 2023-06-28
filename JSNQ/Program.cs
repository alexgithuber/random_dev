using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Intrinsics.X86;
using Microsoft.Win32;
using JSNQ;
//int[] THUMB_SIZE = { 256, 256 };//2*256;d

List<string> filename = new() { @"C:\Users\alxhb\Desktop\10605315631513189564_1.jpeg" , @"C:\Users\alxhb\Desktop\10605315631513189564_2.jpeg"};

foreach (string file in filename)
{ 
string file_new = Path.GetDirectoryName(file)+Path.DirectorySeparatorChar+Path.GetFileNameWithoutExtension(file)+"_thm.jpg";

Bitmap thumbnail = WindowsThumbnailProvider.GetThumbnail(
   file, 250, 256, ThumbnailOptions.ScaleUp);
thumbnail.Save(file_new, ImageFormat.Jpeg);
    ProcessStartInfo psi = new()
    {
        FileName = file_new,
        UseShellExecute = true,
        WindowStyle = ProcessWindowStyle.Normal
    };
    Console.Write(file);
Console.WriteLine(thumbnail.Size); ;

Process.Start(psi);
}