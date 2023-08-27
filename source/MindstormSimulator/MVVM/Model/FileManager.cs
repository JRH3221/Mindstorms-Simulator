using Microsoft.UI.Composition.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Pickers.Provider;
using WinRT.Interop;

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
        public static StorageFolder _StorageDirectory = null;
        private const bool msixPackaged = false;

        private const string _ProjectsList = "Projects.json";

        public static int ID = 0;
        public static async Task<bool> LoadProjectFiles()
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
                    foreach(Project project in Projects)
                    {
                        if(project.ProjectID >= ID)
                        {
                            ID = project.ProjectID + 1;
                        }
                    }
                }
                catch { }
            }
            return true;
        }

        public static async Task<bool> SaveProjectFiles()
        {
            if(await  CheckFiles())
            {
                StorageFile projectsListFile = await GetFile(_ProjectsList);
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonString = JsonSerializer.Serialize(Projects, jsonOptions);
                await FileIO.WriteTextAsync(projectsListFile, jsonString);
            }
            return true;
        }

        public static async Task<Project?> LoadFile(StorageFile file)
        {
            string jsonString = await FileIO.ReadTextAsync(file);
            Project? project = null;
            try
            {
                project = JsonSerializer.Deserialize<Project?>(jsonString);
                if (((Project)project).ProjectID >= ID)
                {
                    ID = ((Project)project).ProjectID + 1;
                }
            }
            catch { }
            return project;
        }

        public static async Task OpenFileDialogue(MainWindow mainWindow)
        {
            FileOpenPicker fileOpenPicker = new()
            {
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { ".mnds" },
            };

            nint windowHandle = WindowNative.GetWindowHandle(mainWindow);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            if (file != null)
            {
                Project? project = await LoadFile(file);
                if(project is not null)
                {
                    if (!Projects.Contains((Project)project))
                    {
                        Projects.Add((Project)project);
                    }

                    mainWindow.LoadProject((Project)project);
                }
            }

            return;
        }

        public static async Task<string> OpenFolderDialogue(object window)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;

            nint windowHandle = WindowNative.GetWindowHandle(window);
            InitializeWithWindow.Initialize(folderPicker, windowHandle);

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if(folder != null)
            {
                return folder.Path;
            }
            return "";
        }

        public static async Task CreateDirectory(string path, string name, Project project) 
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            StorageFolder projectDirectory = await folder.CreateFolderAsync(name, CreationCollisionOption.ReplaceExisting);
            StorageFile projectFile = await projectDirectory.CreateFileAsync("main.mnds");

            string jsonString = JsonSerializer.Serialize(project);
            await FileIO.WriteTextAsync(projectFile, jsonString);
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
