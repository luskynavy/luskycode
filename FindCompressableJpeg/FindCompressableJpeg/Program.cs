﻿using ExifLib;
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
                string dirString = args[0];
                //remove trailing \
                if (dirString.EndsWith("\\"))
                {
                    dirString = dirString.Substring(0, dirString.Length - 1);
                }

                dir = new DirectoryInfo(dirString);
            }

            FileInfo[] files = dir.GetFiles();

            Stopwatch sw = Stopwatch.StartNew();
            //GetWidthHeightExif(files);
            //sw.Stop();
            //Console.WriteLine($"GetWidthHeightExif in {sw.ElapsedMilliseconds} ms");

            //sw = Stopwatch.StartNew();
            //GetWidthHeightBitmap(files);
            //sw.Stop();
            //Console.WriteLine($"GetWidthHeightBitmap in {sw.ElapsedMilliseconds} ms");

            //Console.WriteLine();

            //sw = Stopwatch.StartNew();
            //CompareWidthHeight(files);
            //sw.Stop();

            //Console.WriteLine();
            //Console.WriteLine($"CompareWidthHeight in {sw.ElapsedMilliseconds} ms");

            //Console.WriteLine();

            sw = Stopwatch.StartNew();
            GetSizeRatioExif(files);
            sw.Stop();
            Console.WriteLine($"GetSizeRatioExif in {sw.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Filter image name
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private static bool FilterJpeg(FileInfo f)
        {
            return f.Name.ToLower().EndsWith(".jpg") || f.Name.ToLower().EndsWith(".jpeg") || f.Name.ToLower().EndsWith(".jfif");
        }

        /// <summary>
        /// Test image with ExifReader
        /// </summary>
        /// <param name="files"></param>
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

        /// <summary>
        /// Test image with Bitmap
        /// </summary>
        /// <param name="files"></param>
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

        /// <summary>
        /// Get size ratio with ExifReader and Bitmap
        /// </summary>
        /// <param name="files"></param>
        private static void GetSizeRatioExif(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    int height = 0, width = 0;

                    try
                    {
                        try
                        {
                            //Try ExifReader
                            GetExifReaderHeightWidth(f, out height, out width);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"ExifReader Exception {ex} for {f.Name}");

                            try
                            {
                                //Try Bitmap
                                GetBitmapHeightWidth(f, ref height, ref width);
                            }
                            catch (Exception exBitmap)
                            {
                                Console.WriteLine($"Bitmap Exception {exBitmap} for {f.Name}");
                            }
                        }

                        var nbPixels = height * width / 1024;
                        var sizeFor1024Pixel = f.Length / (nbPixels != 0 ? nbPixels : 1);

                        Console.WriteLine($"{f.Name} ({height} x {width}) : {sizeFor1024Pixel}");


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception {ex} for {f.Name}");
                    }
                }
            }
        }

        /// <summary>
        /// Get height and width with Bitmap
        /// </summary>
        /// <param name="f"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        private static void GetBitmapHeightWidth(FileInfo f, ref int height, ref int width)
        {
            using (var img = new Bitmap(f.FullName))
            {
                if (img != null)
                {
                    height = img.Height;
                    width = img.Width;
                }
            }
        }

        /// <summary>
        /// Get height and width with ExifReader
        /// </summary>
        /// <param name="f"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        private static void GetExifReaderHeightWidth(FileInfo f, out int height, out int width)
        {
            //open file in readonly non locking file
            var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            var exif = ExifReader.ReadJpeg(stream);
            height = exif.Height;
            width = exif.Width;

            stream.Close();
        }

        /// <summary>
        /// Compare width and height with ExifReader and Bitmap
        /// </summary>
        /// <param name="files"></param>
        private static void CompareWidthHeight(FileInfo[] files)
        {
            foreach (FileInfo f in files)
            {
                if (FilterJpeg(f))
                {
                    try
                    {

                        int exifHeight = 0, exifWidth = 0;

                        try
                        {
                            var stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                            var exif = ExifReader.ReadJpeg(stream);
                            exifHeight = exif.Height;
                            exifWidth = exif.Width;
                            stream.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"ExifReader Exception {ex} for {f.Name}");
                        }

                        using (var img = new Bitmap(f.FullName))
                        {
                            if (img != null)
                            {
                                if (exifWidth == img.Width && exifHeight == img.Height)
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