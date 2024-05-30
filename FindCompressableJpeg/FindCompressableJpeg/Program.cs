using ExifLib;
using System.Diagnostics;
using System.Drawing;

namespace FindCompressableJpeg
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DirectoryInfo dir;
            if (args.Length == 0)
            {
                dir = new DirectoryInfo(".");
            }
            else
            {
                dir = new DirectoryInfo(args[0]);
            }

            FileInfo[] files = dir.GetFiles();

            Stopwatch sw = Stopwatch.StartNew();
            GetWidthHeightExif(files);
            sw.Stop();
            Console.WriteLine($"GetWidthHeightExif in {sw.ElapsedMilliseconds} ms");

            sw = Stopwatch.StartNew();
            GetWidthHeightBitmap(files);
            sw.Stop();
            Console.WriteLine($"GetWidthHeightBitmap in {sw.ElapsedMilliseconds} ms");

            Console.WriteLine();

            sw = Stopwatch.StartNew();
            CompareWidthHeight(files);
            sw.Stop();

            Console.WriteLine();
            Console.WriteLine($"CompareWidthHeight in {sw.ElapsedMilliseconds} ms");

            Console.WriteLine();

            sw = Stopwatch.StartNew();
            GetSizeRatioExif(files);
            sw.Stop();
            Console.WriteLine($"GetSizeRatioExif in {sw.ElapsedMilliseconds} ms");
        }

        private static bool FilterJpeg(FileInfo f)
        {
            return f.Name.ToLower().EndsWith(".jpg") || f.Name.ToLower().EndsWith(".jpeg") || f.Name.ToLower().EndsWith(".jfif");
        }

        private static void GetWidthHeightExif(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    try
                    {
                        //open file in readonly non locking file
                        var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        var exif = ExifReader.ReadJpeg(stream);
                        var exifHeight = exif.Height;
                        var exifWidth = exif.Width;
                        //Console.Write($"{f.Name} : {exifWidth} x {exifHeight}");
                        stream.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {ex} for {f.Name}");
                    }
                }
            }
        }

        private static void GetWidthHeightBitmap(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    try
                    {
                        using (var img = new Bitmap(f.FullName))
                        {
                            if (img != null)
                            {
                                //var nbPixels = img.Height * img.Width;

                                //Console.WriteLine($"{f.Name} : {img.Width} x {img.Height}");

                                //Console.WriteLine(f.Name + " : " + sizePerPixel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {ex} for {f.Name}");
                    }
                }
            }
        }

        private static void GetSizeRatioExif(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    try
                    {
                        //open file in readonly non locking file
                        var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        var exif = ExifReader.ReadJpeg(stream);
                        var exifHeight = exif.Height;
                        var exifWidth = exif.Width;
                        //Console.Write($"{f.Name} : {exifWidth} x {exifHeight}");

                        if (exif.ThumbnailSize != 0)
                        {
                        }

                        var nbPixels = exifHeight * exifWidth / 1024;
                        var sizeFor1024Pixel = f.Length / (nbPixels != 0 ? nbPixels : 1);

                        Console.WriteLine($"{f.Name} ({exifWidth} x {exifHeight}) :  {sizeFor1024Pixel}");

                        stream.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {ex} for {f.Name}");
                    }
                }
            }
        }

        private static void CompareWidthHeight(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    try
                    {
                        var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        var exif = ExifReader.ReadJpeg(stream);
                        var exifHeight = exif.Height;
                        var exifWidth = exif.Width;
                        //Console.Write($"{f.Name} : {exifWidth} x {exifHeight}");
                        stream.Close();

                        using (var img = new Bitmap(f.FullName))
                        {
                            if (img != null)
                            {
                                //var nbPixels = img.Height * img.Width / 1024;
                                //var sizePerPixel = f.Length / (nbPixels != 0 ? nbPixels : 1);

                                //Console.WriteLine($"{f.Name} : {img.Width} x {img.Height}");

                                if (exifWidth == img.Width && exif.Height == img.Height)
                                {
                                    Console.WriteLine($"OK {f.Name} : {exifWidth} x {exifHeight}");
                                }
                                else
                                {
                                    Console.WriteLine($"ERROR {f.Name} : {exifWidth} x {exifHeight} !=  {img.Width} x {img.Height}");
                                }

                                //Console.WriteLine(f.Name + " : " + sizePerPixel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {ex} for {f.Name}");
                    }
                }
            }
        }
    }
}