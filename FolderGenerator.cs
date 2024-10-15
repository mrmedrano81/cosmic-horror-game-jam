using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class CreateFolders : EditorWindow
{
    private static string projectName = "PROJECT_NAME";
    [MenuItem("Assets/Create Default Folders")]

    private static void SetUpFolders()
    {
        CreateFolders window = ScriptableObject.CreateInstance<CreateFolders>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
        window.ShowPopup();
    }

    private static void CreateAllFolders()
    {
        List<string> folders = new List<string>
             {
             "Animations",
             "Audio",
             "Editor",
             "Materials",
             "Meshes",
             "Prefabs",
             "Scripts",
             "Scenes",
             "Shaders",
             "Textures",
             "UI"
             };
        foreach (string folder in folders)
        {
            if (!Directory.Exists("Assets/" + folder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/" + folder);
            }
        }
        List<string> animationFolders = new List<string>
             {
             "PlayerAnimations",
             "EntityAnimations",
             "EnvironmentAnimations"
             };
        List<string> uiFolders = new List<string>
             {
             "Assets",
             "Fonts",
             "Icon"
             };
        List<string> audioFolders = new List<string>
             {
             "Music",
             "InGameSFX",
             "UISFX"
             };
        List<string> prefabFolders = new List<string>
             {
             "PlayerPrefabs",
             "EntityPrefabs",
             "EnemyPrefabs",
             "EffectsPrefabs",
             "SystemPrefabs",
             "OtherPrefabs"
             };
        List<string> scriptFolders = new List<string>
             {
             "PlayerScripts",
             "EntityScripts",
             "ManagerScripts",
             "SystemScripts",
             "UIScripts",
             "Utils"
             };
        List<string> textureFolders = new List<string>
             {
             "EntitySprites",
             "TerrainSprites",
             "UISprites",
             "MenuSprites"
             };

        foreach (string subfolder in animationFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/Animations/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/Animations/" + subfolder);
            }
        }

        foreach (string subfolder in uiFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/UI/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/UI/" + subfolder);
            }
        }

        foreach (string subfolder in audioFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/Audio/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/Audio/" + subfolder);
            }
        }

        foreach (string subfolder in prefabFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/Prefabs/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/Prefabs/" + subfolder);
            }
        }

        foreach (string subfolder in scriptFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/Scripts/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/Scripts/" + subfolder);
            }
        }

        foreach (string subfolder in textureFolders)
        {
            if (!Directory.Exists("Assets/" + projectName + "/Textures/" + subfolder))
            {
                Directory.CreateDirectory("Assets/" + projectName + "/Textures/" + subfolder);
            }
        }

        AssetDatabase.Refresh();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Insert the Project name used as the root folder");
        projectName = EditorGUILayout.TextField("Project Name: ", projectName);
        this.Repaint();
        GUILayout.Space(70);
        if (GUILayout.Button("Generate!"))
        {
            CreateAllFolders();
            this.Close();
        }
    }
}
