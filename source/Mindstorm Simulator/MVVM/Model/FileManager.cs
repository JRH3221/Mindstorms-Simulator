﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Diagnostics;

namespace Mindstorm_Simulator.MVVM.Model
{
    public enum Code { SUCCESS = 0, WARNING = 1, ERROR = 2 };
    public class FileManager
    {
        public static int CurrentID = 0;

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

            public string Directory {  get; set; }
            public string ProjectName { get; set; }
            public int ProjectID { get; set; }
            public Type ProjectType { get; set; }
            public Language ProjectLanguage { get; set; }

            public Project(string? directory = null, string? projectName = null, int? ID = null, Type? type = null, Language? language = null)
            {
                Directory = directory ?? "";
                ProjectName = projectName ?? "";
                ID = ID ?? -1; // if a project is loaded with -1 it will be removed
                ProjectType = type ?? Type.Soccer;
                ProjectLanguage = language ?? Language.Python;
            }
        }

        public static List<Project> KnownProjects = new List<Project>();

        private const string _StorageDirectory = "Data";
        private const string _ProjectsList = $"{_StorageDirectory}\\Projects.json";

        public static void PickProjectFile()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".mnds"; // Default file extension
            dialog.Filter = "Mindstorm Simulator Project (.mnds)|*.mnds"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
            }
        }

        public static void PickNewProjectDirectory()
        {
            
        }

        /// <summary>
        /// Load all project data safely.
        /// </summary>
        /// <returns>SUCCESS if normal, WARNING if a file was missing (not much of a problem just possible missing data), ERROR if there is an exception, in this case data loading does not occur.</returns>
        public static Code LoadProjectFiles()
        {
            // check for files and create them if missing. If there is a fatal error, early return and don't load
            // data from files
            Code response = CheckDirectories();
            if (response != Code.SUCCESS) return response;

            // Load knownProjects
            string knownProjectsString = File.ReadAllText(_ProjectsList);
            try
            {
                KnownProjects = JsonSerializer.Deserialize<List<Project>>(knownProjectsString) ?? new List<Project>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                response = Code.WARNING;
            }

            return response;
        }

        public static Code SaveProjectFiles()
        {
            Code response = CheckDirectories();

            string knownProjectsString = JsonSerializer.Serialize(KnownProjects);
            File.WriteAllText(_ProjectsList, knownProjectsString);

            return response;
        }

        private static Code CheckDirectories()
        {
            Code response = Code.SUCCESS;

            try
            {
                if (!Directory.Exists(_StorageDirectory))
                {
                    Directory.CreateDirectory(_StorageDirectory);
                    response = Code.WARNING;
                }

                if (!File.Exists(_ProjectsList))
                {
                    File.Create(_ProjectsList);
                    response = Code.WARNING;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                response = Code.ERROR;
            }
            

            return response;
        }
    }
}
