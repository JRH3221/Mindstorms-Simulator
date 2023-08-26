using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace MindstormSimulator.MVVM.Model
{
    public class FileManager
    {
        public struct Project
        {
            public enum Type
            {
                Soccer = 0,
                Rescue = 1
            }
            public enum Language
            {
                Python = 0,
                Cpp = 1
            }

            public string Directory { get; set; }
            public string ProjectName { get; set; }
            public int ProjectID { get; set; }
            public Type ProjectType { get; set; }
            public Language ProjectLanguage { get; set; }

            public Project(string directory = "", string projectName = "", int ID = -1, Type type = Type.Soccer, Language language = Language.Python)
            {
                Directory = directory ?? "";
                ProjectName = projectName ?? "";
                ProjectID = ID; // if a project is loaded with -1 it will be removed
                ProjectType = type;
                ProjectLanguage = language;
            }
        }

        public static List<Project> Projects = new();
        private static StorageFolder _StorageDirectory = null;
        private const bool msixPackaged = false;

        private const string _ProjectsList = "Projects.json";
        public static async Task LoadProjectFiles()
        {
            // this warning is fine, the if else if only here to make switching packaged modes easier, depending on the mode true/false won't be available
#pragma warning disable CS0162 // Unreachable code detected
            if (!msixPackaged)
            {
                _StorageDirectory = await GetFolder();
            }
            else
            {
                _StorageDirectory = ApplicationData.Current.LocalFolder;
            }
#pragma warning restore CS0162 // Unreachable code detected
            Debug.WriteLine(_StorageDirectory.Path);

            if (await CheckFiles())
            {
                StorageFile projectsListFile = await GetFile(_ProjectsList);
                string jsonString = await FileIO.ReadTextAsync(projectsListFile);

                try
                {
                    Projects = JsonSerializer.Deserialize<List<Project>>(jsonString);
                }
                catch { }
            }
        }

        private static async Task<bool> CheckFiles()
        {
            bool passed = true;
            if(await _StorageDirectory.TryGetItemAsync(_ProjectsList) == null)
            {
                passed = await CreateFile(_ProjectsList) && passed;
            }

            return passed;
        }

        private static async Task<StorageFolder> GetFolder()
        {
            StorageFolder appData = await StorageFolder.GetFolderFromPathAsync(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            if(await appData.TryGetItemAsync("MindstormSimulator") == null)
            {
                return await appData.CreateFolderAsync("MindstormSimulator");
            }
            else
            {
                return (StorageFolder)await appData.TryGetItemAsync("MindstormSimulator");
            }
        }

        /// <summary>
        /// WARNING, this method does not do checking, please use CheckFiles() before running this!
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>The storagefile by the same name as fileName</returns>
        private static async Task<StorageFile> GetFile(string fileName)
        {
            return await _StorageDirectory.GetFileAsync(fileName);
        }

        private static async Task<bool> CreateFile(string fileName)
        {
            await _StorageDirectory.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            int attemps = 0;
            const int maxAttemps = 10;

            while(await _StorageDirectory.TryGetItemAsync(fileName) == null && attemps < maxAttemps)
            {
                await Task.Delay(100);
                attemps++;
            }

            return (await _StorageDirectory.TryGetItemAsync(fileName) != null); // return true if file exists
        }
    }
}
