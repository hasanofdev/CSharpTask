using System;
using Segment.Model;
using System.Diagnostics;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Drawing;
using System.Drawing.Imaging;

namespace CSharpTask3;

class Program
{
    public static void OpenWithDefaultProgram(string path)
    {
        using Process fileopener = new Process();
        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + path + "\"";
        fileopener.Start();
    }


    static void Main()
    {

        while (true)
        {
            Console.Clear();
            Console.WriteLine(@"Choose Command
1) Take ScreenShot
2) See al ScreenShots Names
3) Delete
4) Open ScreenShot");
            if (!int.TryParse(Console.ReadLine(), out int command))
            {
                Console.WriteLine("Incorrect Command");
                Console.ReadKey(false);
                continue;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ScreenShots");
            string imageId;

            switch (command)
            {
                case 1:
                    if (!Directory.Exists(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ScreenShots"))
                        Directory.CreateDirectory(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ScreenShots");
                    Console.WriteLine("Starting the process...");
                    Console.WriteLine();
                    Bitmap memoryImage;
                    memoryImage = new Bitmap(1920, 1080);
                    Size s = new Size(memoryImage.Width, memoryImage.Height);

                    Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                    
                    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
                    string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ScreenShots\ScreenShot_{Guid.NewGuid()}_{DateTime.Now:(dd_MMMM_hh_mm_ss_tt)}.png";

                    memoryImage.Save(fileName);
                    Console.WriteLine("Picture has been saved...");
                    break;

                case 2:
                    if (!directoryInfo.Exists)
                    {
                        Console.WriteLine("You have not saved any screenshot yet");
                        Console.ReadKey(false);
                        continue;
                    }

                    if (directoryInfo.GetFiles().Length == 0)
                    {
                        Console.WriteLine("No screenshots found");
                        Console.ReadKey(false);
                        continue;
                    }


                    foreach (var file in directoryInfo.GetFiles())
                        Console.WriteLine(file.Name);

                    Console.ReadKey(false);


                    break;


                case 3:

                    if (!directoryInfo.Exists)
                    {
                        Console.WriteLine("You have not saved any screenshot yet");
                        Console.ReadKey(false);
                        continue;
                    }

                    if (directoryInfo.GetFiles().Length == 0)
                    {
                        Console.WriteLine("No screenshots found");
                        Console.ReadKey(false);
                        continue;
                    }

                    Console.WriteLine("Enter first 8 sybmols id of screenshot");

                    imageId = Console.ReadLine()!;

                    if (string.IsNullOrEmpty(imageId) || imageId.Length != 8)
                    {
                        Console.WriteLine("Entered incorrect information");
                        Console.ReadKey(false);
                        break;
                    }

                    bool isDeleted = false;

                    foreach (var file in directoryInfo.GetFiles())
                    {
                        if (file.Name.Split('_')[1].Contains(imageId))
                        {
                            file.Delete();
                            Console.WriteLine("Deleted succesfully");
                            Console.ReadKey(false);
                            break;
                        }
                    }

                    if (!isDeleted)
                    {
                        Console.WriteLine("File not found");
                        Console.ReadKey(false);
                    }
                    break;

                case 4:
                    if (!directoryInfo.Exists)
                    {
                        Console.WriteLine("You have not saved any screenshot yet");
                        Console.ReadKey(false);
                        continue;
                    }

                    if (directoryInfo.GetFiles().Length == 0)
                    {
                        Console.WriteLine("No screenshots found");
                        Console.ReadKey(false);
                        continue;
                    }

                    Console.WriteLine("Enter first 8 sybmols id of screenshot");
                    imageId = Console.ReadLine()!;

                    foreach (var file in directoryInfo.GetFiles())
                    {
                        if (file.Name.Split('_')[1].Contains(imageId))
                            OpenWithDefaultProgram(@$"{directoryInfo.FullName}\{file.Name}");
                    }

                    break;

                default:
                    Console.WriteLine("Incorrect Command");
                    Console.ReadKey(false);
                    break;
            }

        }
    }
}
