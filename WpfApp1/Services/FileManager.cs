using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public interface IFileManager
    {
        public string Read(string filePath);
        public void Save(string filePath, string content);
    }
    public class FileManager : IFileManager
    {
        public string Read(string filePath)
        {
            using var sr = new StreamReader(filePath);
            return sr.ReadToEnd();
        }

        public void Save(string filePath, string content)
        {
            var text = Read(filePath);
            text += content;

            using var sw = new StreamWriter(filePath);
            sw.WriteLineAsync(content);
        }

        
    }
}
