using System.Collections.Generic;
using System.IO;

namespace vzr_common
{
    public class Util
    {
        public static void MoveFile(string source, string destination)
        {
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            File.Move(source, destination);
        }

        public static void MoveFiles(string folderSource, string folderDestination)
        {
            string[] files = Directory.GetFiles(folderSource);
            foreach (string file in files)
            {
                string dest = Path.Combine(folderDestination, Path.GetFileName(file));
                MoveFile(file, dest);
            }
        }

        public static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination, true);
        }

        public static void CoypFiles(string source, string destination, string fileExtension = ".xhtml")
        {
            IEnumerable<string> files = Directory.EnumerateFiles(source, "*" + fileExtension);
            foreach (string file in files)
            {
                CopyFile(file, Path.Combine(destination, Path.GetFileName(file)));
            }
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}