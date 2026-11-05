using System;
using System.Buffers.Binary;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageMismatchFinder
{
    public static class AppTests
    {
        public static int RunAll()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("Running ImageMismatchFinder Self-Tests...");
            Console.WriteLine("==================================================");

            string testDir = Path.Combine(Path.GetTempPath(), "ImageMismatchFinder_Tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(testDir);

            int passed = 0;
            int failed = 0;

            try
            {
                // Test 1: Real JPEG
                string realJpg = Path.Combine(testDir, "real_photo.jpg");
                CreateBitmapAndSave(realJpg, ImageFormat.Jpeg, 320, 240);
                var infoJpg = ImageFormatDetector.Detect(realJpg);
                Assert(infoJpg.Format == DetectedImageFormat.Jpeg, "Real JPEG detection", ref passed, ref failed);

                // Test 2: Real PNG with .jpg extension (Mismatch!)
                string fakePng = Path.Combine(testDir, "fake_png.jpg");
                CreateBitmapAndSave(fakePng, ImageFormat.Png, 640, 480);
                var infoPng = ImageFormatDetector.Detect(fakePng);
                Assert(infoPng.Format == DetectedImageFormat.Png && infoPng.Width == 640 && infoPng.Height == 480,
                    $"PNG disguised as JPG detection (Got {infoPng.FormatName}, {infoPng.Width}x{infoPng.Height})", ref passed, ref failed);

                // Test 3: Real BMP with .jpeg extension (Mismatch!)
                string fakeBmp = Path.Combine(testDir, "fake_bmp.jpeg");
                CreateBitmapAndSave(fakeBmp, ImageFormat.Bmp, 400, 300);
                var infoBmp = ImageFormatDetector.Detect(fakeBmp);
                Assert(infoBmp.Format == DetectedImageFormat.Bmp && infoBmp.Width == 400 && infoBmp.Height == 300,
                    $"BMP disguised as JPEG detection (Got {infoBmp.FormatName}, {infoBmp.Width}x{infoBmp.Height})", ref passed, ref failed);

                // Test 4: WebP (VP8X) with .jfif extension (Mismatch!)
                string fakeWebp = Path.Combine(testDir, "fake_webp.jfif");
                CreateMockWebP(fakeWebp, 1920, 1080);
                var infoWebp = ImageFormatDetector.Detect(fakeWebp);
                Assert(infoWebp.Format == DetectedImageFormat.WebP && infoWebp.Width == 1920 && infoWebp.Height == 1080,
                    $"WebP disguised as JFIF detection (Got {infoWebp.FormatName}, {infoWebp.Width}x{infoWebp.Height})", ref passed, ref failed);

                // Test 5: AVIF with .jpg extension (Mismatch!)
                string fakeAvif = Path.Combine(testDir, "fake_avif.jpg");
                CreateMockAvif(fakeAvif, 1280, 720);
                var infoAvif = ImageFormatDetector.Detect(fakeAvif);
                Assert(infoAvif.Format == DetectedImageFormat.Avif && infoAvif.Width == 1280 && infoAvif.Height == 720,
                    $"AVIF disguised as JPG detection (Got {infoAvif.FormatName}, {infoAvif.Width}x{infoAvif.Height})", ref passed, ref failed);

                // Test 6: Empty file with .jpg extension
                string fakeEmpty = Path.Combine(testDir, "empty_file.jpg");
                File.WriteAllBytes(fakeEmpty, Array.Empty<byte>());
                var infoEmpty = ImageFormatDetector.Detect(fakeEmpty);
                var itemEmpty = new ImageFileItem { RealType = infoEmpty.FormatName, Width = infoEmpty.Width, Height = infoEmpty.Height };
                Assert(infoEmpty.Format == DetectedImageFormat.Empty && infoEmpty.FormatName == "Empty" && itemEmpty.Dimensions == "-",
                    $"Empty file detection (Got {infoEmpty.FormatName}, dimensions: '{itemEmpty.Dimensions}')", ref passed, ref failed);

                // Test 7: Unknown file with .jpg extension (e.g. text/binary file)
                string fakeUnknown = Path.Combine(testDir, "unknown_doc.jpg");
                File.WriteAllText(fakeUnknown, "Hello world, this is a plain text file disguised as a jpeg.");
                var infoUnknown = ImageFormatDetector.Detect(fakeUnknown);
                var itemUnknown = new ImageFileItem { RealType = infoUnknown.FormatName, Width = infoUnknown.Width, Height = infoUnknown.Height };
                Assert(infoUnknown.Format == DetectedImageFormat.Unknown && infoUnknown.FormatName == "Unknown" && itemUnknown.Dimensions == "Unknown",
                    $"Unknown file detection (Got {infoUnknown.FormatName}, dimensions: '{itemUnknown.Dimensions}')", ref passed, ref failed);

                // Test 8: INI File Save and Load
                string iniPath = Path.Combine(testDir, "test_settings.ini");
                var ini1 = new IniFile(iniPath);
                ini1.SetValue("Settings", "SearchPath", @"C:\TestDirectory");
                ini1.SetBool("Settings", "SearchSubdirectories", true);
                ini1.SetInt("Settings", "WeeksFilter", 3);
                ini1.SetBool("Settings", "CopyFullPath", true);
                ini1.Save();

                var ini2 = new IniFile(iniPath);
                bool iniOk = ini2.GetValue("Settings", "SearchPath") == @"C:\TestDirectory" &&
                             ini2.GetBool("Settings", "SearchSubdirectories") == true &&
                             ini2.GetInt("Settings", "WeeksFilter") == 3 &&
                             ini2.GetBool("Settings", "CopyFullPath") == true;
                Assert(iniOk, "INI Save and Load settings", ref passed, ref failed);

                // Test 9: Subdirectory, Weeks Filter, Empty & Unknown files included in search
                string subDir = Path.Combine(testDir, "SubFolder");
                Directory.CreateDirectory(subDir);
                string nestedFakePng = Path.Combine(subDir, "nested_fake.jpeg");
                CreateBitmapAndSave(nestedFakePng, ImageFormat.Png, 800, 600);

                // Old fake file (30 days old)
                string oldFakePng = Path.Combine(testDir, "old_fake.jpg");
                CreateBitmapAndSave(oldFakePng, ImageFormat.Png, 500, 500);
                File.SetLastWriteTime(oldFakePng, DateTime.Now.AddDays(-30));

                // Perform simulated search with weeks = 1 (should exclude oldFakePng, but include fakePng, fakeBmp, fakeWebp, fakeAvif, fakeEmpty, fakeUnknown, nestedFakePng)
                var foundItems = new System.Collections.Generic.List<ImageFileItem>();
                DateTime cutoff = DateTime.Now.AddDays(-7);
                foreach (var file in Directory.EnumerateFiles(testDir, "*.*", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(file);
                    if (ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        ext.Equals(".jfif", StringComparison.OrdinalIgnoreCase))
                    {
                        var fi = new FileInfo(file);
                        if (fi.LastWriteTime < cutoff)
                            continue;

                        var detected = ImageFormatDetector.Detect(file);
                        // All non-JPEG are considered mismatches (including Empty and Unknown)
                        if (detected.Format != DetectedImageFormat.Jpeg)
                        {
                            foundItems.Add(new ImageFileItem
                            {
                                FileName = fi.Name,
                                FilePath = fi.FullName,
                                RealType = detected.FormatName,
                                Width = detected.Width,
                                Height = detected.Height
                            });
                        }
                    }
                }

                bool foundCorrect = foundItems.Count == 7 &&
                                    !foundItems.Exists(f => f.FileName == "old_fake.jpg") &&
                                    !foundItems.Exists(f => f.FileName == "real_photo.jpg") &&
                                    foundItems.Exists(f => f.FileName == "empty_file.jpg") &&
                                    foundItems.Exists(f => f.FileName == "unknown_doc.jpg") &&
                                    foundItems.Exists(f => f.FileName == "nested_fake.jpeg");
                Assert(foundCorrect, $"Search includes Empty & Unknown and filters weeks (Found {foundItems.Count}/7 items)", ref passed, ref failed);

                Console.WriteLine("==================================================");
                Console.WriteLine($"Tests Finished: {passed} Passed, {failed} Failed.");
                Console.WriteLine("==================================================");
            }
            finally
            {
                try
                {
                    Directory.Delete(testDir, true);
                }
                catch { }
            }

            return failed == 0 ? 0 : 1;
        }

        private static void Assert(bool condition, string testName, ref int passed, ref int failed)
        {
            if (condition)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[PASS] {testName}");
                Console.ResetColor();
                passed++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FAIL] {testName}");
                Console.ResetColor();
                failed++;
            }
        }

        private static void CreateBitmapAndSave(string path, ImageFormat format, int width, int height)
        {
            using var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.CornflowerBlue);
            }
            bmp.Save(path, format);
        }

        private static void CreateMockWebP(string path, int width, int height)
        {
            // RIFF header (12 bytes) + VP8X header (18 bytes) = 30 bytes
            byte[] data = new byte[30];
            // "RIFF"
            data[0] = (byte)'R'; data[1] = (byte)'I'; data[2] = (byte)'F'; data[3] = (byte)'F';
            BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(4, 4), 22); // File size - 8
            // "WEBP"
            data[8] = (byte)'W'; data[9] = (byte)'E'; data[10] = (byte)'B'; data[11] = (byte)'P';
            // "VP8X"
            data[12] = (byte)'V'; data[13] = (byte)'P'; data[14] = (byte)'8'; data[15] = (byte)'X';
            BinaryPrimitives.WriteInt32LittleEndian(data.AsSpan(16, 4), 10); // Chunk size
            // Flags at byte 20
            data[20] = 0x00;
            // Width - 1 (24-bit)
            int w = width - 1;
            data[24] = (byte)(w & 0xFF);
            data[25] = (byte)((w >> 8) & 0xFF);
            data[26] = (byte)((w >> 16) & 0xFF);
            // Height - 1 (24-bit)
            int h = height - 1;
            data[27] = (byte)(h & 0xFF);
            data[28] = (byte)((h >> 8) & 0xFF);
            data[29] = (byte)((h >> 16) & 0xFF);

            File.WriteAllBytes(path, data);
        }

        private static void CreateMockAvif(string path, int width, int height)
        {
            // ISOBMFF ftyp box (32 bytes) + mock ispe box (20 bytes) = 52 bytes
            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            // ftyp box: 32 bytes
            bw.Write(BinaryPrimitives.ReverseEndianness(32)); // size = 32
            bw.Write(new byte[] { (byte)'f', (byte)'t', (byte)'y', (byte)'p' });
            bw.Write(new byte[] { (byte)'a', (byte)'v', (byte)'i', (byte)'f' }); // major brand = avif
            bw.Write(BinaryPrimitives.ReverseEndianness(0)); // minor version = 0
            bw.Write(new byte[] { (byte)'a', (byte)'v', (byte)'i', (byte)'f' }); // compatible brand 1
            bw.Write(new byte[] { (byte)'m', (byte)'i', (byte)'f', (byte)'1' }); // compatible brand 2
            bw.Write(new byte[] { (byte)'m', (byte)'i', (byte)'a', (byte)'f' }); // compatible brand 3
            bw.Write(BinaryPrimitives.ReverseEndianness(0)); // padding

            // ispe box: 20 bytes
            bw.Write(BinaryPrimitives.ReverseEndianness(20)); // size = 20
            bw.Write(new byte[] { (byte)'i', (byte)'s', (byte)'p', (byte)'e' });
            bw.Write(BinaryPrimitives.ReverseEndianness(0)); // version & flags = 0
            bw.Write(BinaryPrimitives.ReverseEndianness(width));
            bw.Write(BinaryPrimitives.ReverseEndianness(height));

            File.WriteAllBytes(path, ms.ToArray());
        }
    }
}
