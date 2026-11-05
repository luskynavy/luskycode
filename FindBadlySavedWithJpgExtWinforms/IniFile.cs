using System;
using System.Collections.Generic;
using System.IO;

namespace ImageMismatchFinder
{
    public class IniFile
    {
        private readonly string _filePath;
        private readonly Dictionary<string, Dictionary<string, string>> _data =
            new(StringComparer.OrdinalIgnoreCase);

        public IniFile(string filePath)
        {
            _filePath = filePath;
            Load();
        }

        public void Load()
        {
            _data.Clear();
            if (!File.Exists(_filePath))
                return;

            string currentSection = "";
            foreach (var rawLine in File.ReadAllLines(_filePath))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith(';') || line.StartsWith('#'))
                    continue;

                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    currentSection = line.Substring(1, line.Length - 2).Trim();
                    if (!_data.ContainsKey(currentSection))
                    {
                        _data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    int separatorIndex = line.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        string key = line.Substring(0, separatorIndex).Trim();
                        string value = line.Substring(separatorIndex + 1).Trim();

                        if (!_data.ContainsKey(currentSection))
                        {
                            _data[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }

                        _data[currentSection][key] = value;
                    }
                }
            }
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using var writer = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8);
                foreach (var section in _data)
                {
                    if (!string.IsNullOrEmpty(section.Key))
                    {
                        writer.WriteLine($"[{section.Key}]");
                    }

                    foreach (var kvp in section.Value)
                    {
                        writer.WriteLine($"{kvp.Key}={kvp.Value}");
                    }

                    writer.WriteLine();
                }
            }
            catch
            {
                // Ignore save errors (e.g. read-only media)
            }
        }

        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (_data.TryGetValue(section, out var sec) && sec.TryGetValue(key, out var val))
            {
                return val;
            }
            return defaultValue;
        }

        public bool GetBool(string section, string key, bool defaultValue = false)
        {
            var val = GetValue(section, key, null!);
            if (val == null) return defaultValue;
            if (bool.TryParse(val, out bool result)) return result;
            if (val == "1") return true;
            if (val == "0") return false;
            return defaultValue;
        }

        public int GetInt(string section, string key, int defaultValue = 0)
        {
            var val = GetValue(section, key, null!);
            if (val != null && int.TryParse(val, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public void SetValue(string section, string key, string value)
        {
            if (!_data.TryGetValue(section, out var sec))
            {
                sec = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                _data[section] = sec;
            }
            sec[key] = value;
        }

        public void SetBool(string section, string key, bool value)
        {
            SetValue(section, key, value.ToString());
        }

        public void SetInt(string section, string key, int value)
        {
            SetValue(section, key, value.ToString());
        }
    }
}
