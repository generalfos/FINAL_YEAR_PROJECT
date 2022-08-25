using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

// This file should be placed at Project -> Assets -> Editor directory of a unity project.
#if (UNITY_EDITOR)

    public class BuildScript {

        static string[] SCENES;

        static string APP_NAME = ""; 
        static string BUILD_NUMBER = "";
        static string TARGET_DIR = ""; 

        // Currently PreformAllBuilds only calls PreformWindowsBuild(). 
        // Instead it should take an additional argument of the target platform/s and build to those specified
        // Target will be argument 4 of "executeMethod"
        static void PreformAllBuilds () {
            Debug.Log("Preforming All Builds");

            SCENES = FindEnabledEditorScenes();

            // -executeMethod
            // +1: PreformAllBuilds Function
            // +2: Job Name
            // +3: Build Number
            string[] args = System.Environment.GetCommandLineArgs();

            // Fake For loop to grab args and ensure correct length supplied
            for(int i=0; i<args.Length; i++) {
                if (args[i] == "-executeMethod") {
                    if (i+4 < args.Length) {
                        APP_NAME = args[i+2]; // Set Name for Build
                        BUILD_NUMBER = args[i+3]; // Set Build Number
                        i += 3;
                    } else {
                        System.Console.WriteLine("[BuildScript] Incorrect Parameters for -executeMethod. Expected: -executeMethod Function <JOB_NAME> <BUILD_NUMBER>");
                    }
                }
            }

            // string assets_folder = Application.dataPath;
            //DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            string di = Directory.GetParent(Application.dataPath).FullName;
            TARGET_DIR = Path.Combine(di, "Builds", BUILD_NUMBER);
            
            // Calls to build to Windows
            PreformWindowsBuild();
        }

        static void PreformWindowsBuild () {
            Debug.Log("Preforming Windows Builds");

            string APP_WEX = APP_NAME + ".exe";
            string COMBINE_DIR = Path.Combine(TARGET_DIR, APP_WEX);

            BuildOptions buildOptions = BuildOptions.Development | BuildOptions.StrictMode | BuildOptions.DetailedBuildReport;

            BuildPipeline.BuildPlayer(SCENES, COMBINE_DIR, BuildTarget.StandaloneWindows64, buildOptions);
            BuildPipeline.BuildPlayer(SCENES, COMBINE_DIR, BuildTarget.StandaloneWindows, buildOptions);

            BuildInfoLog(Application.version, TARGET_DIR);    
        }

        private static string[] FindEnabledEditorScenes() {
            // Find all Scenes in Unity that are enabled.
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                if (!scene.enabled) {
                    // Skip loop is scene is not enabled.
                    continue;
                }
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        private static void BuildInfoLog(string log, string logPath) {
            // Create BuildInfoLog
            // Line 1: Version Number
            // Line 2+ To be decided
            string LOG_PATH = Path.Combine(logPath,"BuildInfoLog.log");
            StreamWriter writer = new StreamWriter(LOG_PATH, true);
            writer.WriteLine(log); 
            writer.Close();
        }
    }
#endif