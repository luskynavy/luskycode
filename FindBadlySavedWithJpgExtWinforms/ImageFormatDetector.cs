using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace ImageMismatchFinder
{
    public enum DetectedImageFormat
    {
        Unknown,
        Empty,
        Jpeg,
        Png,
        WebP,
        Avif,
        Bmp,
        Gif,
        Tiff
    }

    public class ImageInfo
    {
        public DetectedImageFormat Format { get; set; } = DetectedImageFormat.Unknown;
        public string FormatName { get; set; } = "Unknown";
        public int Width { get; set; }
        public int Height { get; set; }
        public bool HasDimensions => Width > 0 && Height > 0;
        public string DimensionsText => HasDimensions ? $"{Width} x {Height}" : (Format == DetectedImageFormat.Empty ? "-" : "Unknown");
    }

    public static class ImageFormatDetector
    {
        private const int MaxHeaderRead = 8192;

        public static ImageInfo Detect(string filePath)
        {
            var info = new ImageInfo();

            byte[] buffer;
            int bytesRead;
            try
            {
                var fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                    return info;

                if (fileInfo.Length == 0)
                {
                    info.Format = DetectedImageFormat.Empty;
                    info.FormatName = "Empty";
                    return info;
                }

                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                buffer = new byte[Math.Min(MaxHeaderRead, (int)Math.Min(fs.Length, MaxHeaderRead))];
                bytesRead = fs.Read(buffer, 0, buffer.Length);
            }
            catch
            {
                return info;
            }

            if (bytesRead == 0)
            {
                info.Format = DetectedImageFormat.Empty;
                info.FormatName = "Empty";
                return info;
            }

            var span = buffer.AsSpan(0, bytesRead);

            // 1. JPEG check: Starts with 0xFF, 0xD8, 0xFF
            if (span.Length >= 3 && span[0] == 0xFF && span[1] == 0xD8 && span[2] == 0xFF)
            {
                info.Format = DetectedImageFormat.Jpeg;
                info.FormatName = "JPEG";
                GetJpegDimensions(filePath, span, info);
                return info;
            }

            // 2. PNG check: 89 50 4E 47 0D 0A 1A 0A
            if (span.Length >= 8 &&
                span[0] == 0x89 && span[1] == 0x50 && span[2] == 0x4E && span[3] == 0x47 &&
                span[4] == 0x0D && span[5] == 0x0A && span[6] == 0x1A && span[7] == 0x0A)
            {
                info.Format = DetectedImageFormat.Png;
                info.FormatName = "PNG";
                if (span.Length >= 24)
                {
                    // IHDR chunk: width at 16-19, height at 20-23 (big endian)
                    info.Width = BinaryPrimitives.ReadInt32BigEndian(span.Slice(16, 4));
                    info.Height = BinaryPrimitives.ReadInt32BigEndian(span.Slice(20, 4));
                }
                return info;
            }

            // 3. BMP check: 42 4D ("BM")
            if (span.Length >= 26 && span[0] == 0x42 && span[1] == 0x4D)
            {
                // DIB header size at offset 14 (4 bytes little-endian)
                int dibSize = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(14, 4));
                if (dibSize >= 40 && span.Length >= 26)
                {
                    info.Format = DetectedImageFormat.Bmp;
                    info.FormatName = "BMP";
                    info.Width = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(18, 4));
                    info.Height = Math.Abs(BinaryPrimitives.ReadInt32LittleEndian(span.Slice(22, 4)));
                    return info;
                }
                else if (dibSize == 12 && span.Length >= 22)
                {
                    info.Format = DetectedImageFormat.Bmp;
                    info.FormatName = "BMP";
                    info.Width = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(18, 2));
                    info.Height = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(20, 2));
                    return info;
                }
            }

            // 4. WEBP check: RIFF....WEBP
            if (span.Length >= 12 &&
                span[0] == (byte)'R' && span[1] == (byte)'I' && span[2] == (byte)'F' && span[3] == (byte)'F' &&
                span[8] == (byte)'W' && span[9] == (byte)'E' && span[10] == (byte)'B' && span[11] == (byte)'P')
            {
                info.Format = DetectedImageFormat.WebP;
                info.FormatName = "WEBP";
                GetWebPDimensions(span, info);
                return info;
            }

            // 5. AVIF check: ISOBMFF ftyp box with avif/avis brand
            if (IsAvif(span))
            {
                info.Format = DetectedImageFormat.Avif;
                info.FormatName = "AVIF";
                GetAvifDimensions(span, filePath, info);
                return info;
            }

            // 6. GIF check: GIF87a or GIF89a
            if (span.Length >= 10 &&
                span[0] == (byte)'G' && span[1] == (byte)'I' && span[2] == (byte)'F' &&
                span[3] == (byte)'8' && (span[4] == (byte)'7' || span[4] == (byte)'9') && span[5] == (byte)'a')
            {
                info.Format = DetectedImageFormat.Gif;
                info.FormatName = "GIF";
                info.Width = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(6, 2));
                info.Height = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(8, 2));
                return info;
            }

            // 7. TIFF check: II*\0 or MM\0*
            if (span.Length >= 4 &&
                ((span[0] == 0x49 && span[1] == 0x49 && span[2] == 0x2A && span[3] == 0x00) ||
                 (span[0] == 0x4D && span[1] == 0x4D && span[2] == 0x00 && span[3] == 0x2A)))
            {
                info.Format = DetectedImageFormat.Tiff;
                info.FormatName = "TIFF";
                return info;
            }

            return info;
        }

        private static void GetWebPDimensions(ReadOnlySpan<byte> span, ImageInfo info)
        {
            if (span.Length < 16) return;

            string fourCC = Encoding.ASCII.GetString(span.Slice(12, 4));
            if (fourCC == "VP8 " && span.Length >= 30)
            {
                // VP8 lossy: 3-byte frame tag at 20-22, start code 9D 01 2A at 23-25
                if (span[23] == 0x9D && span[24] == 0x01 && span[25] == 0x2A)
                {
                    int w = span[26] | (span[27] << 8);
                    int h = span[28] | (span[29] << 8);
                    info.Width = w & 0x3FFF;
                    info.Height = h & 0x3FFF;
                }
            }
            else if (fourCC == "VP8L" && span.Length >= 25)
            {
                // VP8L lossless: 0x2F signature at byte 20, then 32 bits containing width/height
                if (span[20] == 0x2F)
                {
                    uint bits = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(21, 4));
                    info.Width = (int)(1 + (bits & 0x3FFF));
                    info.Height = (int)(1 + ((bits >> 14) & 0x3FFF));
                }
            }
            else if (fourCC == "VP8X" && span.Length >= 30)
            {
                // VP8X extended: 24-bit width at 24-26, 24-bit height at 27-29 (both minus 1)
                int w = span[24] | (span[25] << 8) | (span[26] << 16);
                int h = span[27] | (span[28] << 8) | (span[29] << 16);
                info.Width = w + 1;
                info.Height = h + 1;
            }
        }

        private static bool IsAvif(ReadOnlySpan<byte> span)
        {
            if (span.Length < 12) return false;

            // ISOBMFF ftyp box: bytes 4-7 must be "ftyp"
            if (span[4] != (byte)'f' || span[5] != (byte)'t' || span[6] != (byte)'y' || span[7] != (byte)'p')
                return false;

            int boxSize = BinaryPrimitives.ReadInt32BigEndian(span.Slice(0, 4));
            if (boxSize < 8 || boxSize > 1024)
                boxSize = Math.Min(span.Length, 256); // fallback to a reasonable search limit

            int searchLimit = Math.Min(span.Length, boxSize);

            // Check major brand (bytes 8-11)
            string majorBrand = Encoding.ASCII.GetString(span.Slice(8, 4));
            if (majorBrand == "avif" || majorBrand == "avis")
                return true;

            // Check compatible brands starting at byte 16 (after 4-byte minor version)
            for (int i = 16; i + 4 <= searchLimit; i += 4)
            {
                string brand = Encoding.ASCII.GetString(span.Slice(i, 4));
                if (brand == "avif" || brand == "avis")
                    return true;
            }

            return false;
        }

        private static void GetAvifDimensions(ReadOnlySpan<byte> span, string filePath, ImageInfo info)
        {
            // Search for "ispe" (Image Spatial Extents box) in span
            // Structure: 4 bytes size, "ispe" (4 bytes), 1 byte version, 3 bytes flags, 4 bytes width, 4 bytes height
            byte[] pattern = [(byte)'i', (byte)'s', (byte)'p', (byte)'e'];

            int index = span.IndexOf(pattern);
            if (index >= 4 && index + 16 <= span.Length)
            {
                // ispe box payload starts at index + 4
                // version (1 byte) + flags (3 bytes) at index + 4
                // width (4 bytes big-endian) at index + 8
                // height (4 bytes big-endian) at index + 12
                int width = BinaryPrimitives.ReadInt32BigEndian(span.Slice(index + 8, 4));
                int height = BinaryPrimitives.ReadInt32BigEndian(span.Slice(index + 12, 4));
                if (width > 0 && height > 0)
                {
                    info.Width = width;
                    info.Height = height;
                    return;
                }
            }

            // Fallback: If not in initial buffer, try reading a bit more of the file if needed
            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (fs.Length > span.Length)
                {
                    int extraRead = (int)Math.Min(fs.Length, 32768);
                    byte[] largerBuffer = new byte[extraRead];
                    int read = fs.Read(largerBuffer, 0, extraRead);
                    var largeSpan = largerBuffer.AsSpan(0, read);
                    int idx = largeSpan.IndexOf(pattern);
                    if (idx >= 4 && idx + 16 <= largeSpan.Length)
                    {
                        int width = BinaryPrimitives.ReadInt32BigEndian(largeSpan.Slice(idx + 8, 4));
                        int height = BinaryPrimitives.ReadInt32BigEndian(largeSpan.Slice(idx + 12, 4));
                        if (width > 0 && height > 0)
                        {
                            info.Width = width;
                            info.Height = height;
                        }
                    }
                }
            }
            catch
            {
                // Ignore fallback error
            }
        }

        private static void GetJpegDimensions(string filePath, ReadOnlySpan<byte> span, ImageInfo info)
        {
            // Optional: extract JPEG SOF0/SOF2 marker dimensions if desired
            try
            {
                int i = 2;
                while (i + 9 < span.Length)
                {
                    if (span[i] != 0xFF) break;
                    byte marker = span[i + 1];
                    if (marker == 0xD9 || marker == 0xDA) // EOI or SOS
                        break;

                    int len = (span[i + 2] << 8) | span[i + 3];
                    if (len < 2) break;

                    // SOF0 (0xC0), SOF1 (0xC1), SOF2 (0xC2)
                    if (marker == 0xC0 || marker == 0xC1 || marker == 0xC2)
                    {
                        info.Height = (span[i + 5] << 8) | span[i + 6];
                        info.Width = (span[i + 7] << 8) | span[i + 8];
                        return;
                    }

                    i += 2 + len;
                }
            }
            catch
            {
                // Ignore JPEG parsing error
            }
        }
    }
}
