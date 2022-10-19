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
        /* FileManager ärver ifrån IFileManager som specificerar vilka funktioner som får ingå*/
        public string Read(string filePath)
        {
            using var sr = new StreamReader(filePath);
            return sr.ReadToEnd();
            /*Tar in en sökväg för att kunna läsa ifrån en fil och returnerar detta som ett strängvärde.*/
        }

        public void Save(string filePath, string content)
        {
            var text = Read(filePath);
            text += content;

            using var sw = new StreamWriter(filePath);
            sw.WriteLine(text);
            /* Tar in en sökväg samt datan som ska sparas i form av en sträng. Först läser funktionen datan som redan finns i filen och sparar i variabeln text.
             Därefter lägger den till den data som kommit in med variabeln content och skriver sedan över det som finns i filen*/
        }

        
    }
}
