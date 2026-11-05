using System;
using System.IO;

namespace ImageMismatchFinder
{
    public class ImageFileItem
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Folder { get; set; } = string.Empty;
        public string RealType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FileSizeFormatted => FormatFileSize(FileSize);
        public int Width { get; set; }
        public int Height { get; set; }
        public string Dimensions
        {
            get
            {
                if (Width > 0 && Height > 0)
                    return $"{Width} x {Height}";
                if (string.Equals(RealType, "Empty", StringComparison.OrdinalIgnoreCase))
                    return "-";
                return "Unknown";
            }
        }
        public DateTime DateModified { get; set; }
        public string DateFormatted => DateModified.ToString("yyyy-MM-dd HH:mm:ss");

        public static string FormatFileSize(long bytes)
        {
            if (bytes < 0) return "0 B";
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{(bytes / 1024.0):F1} KB";
            if (bytes < 1024 * 1024 * 1024) return $"{(bytes / (1024.0 * 1024.0)):F2} MB";
            return $"{(bytes / (1024.0 * 1024.0 * 1024.0)):F2} GB";
        }
    }
}
