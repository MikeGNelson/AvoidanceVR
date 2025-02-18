using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneOpenerEditor : Editor
{
    // Add a menu item to open "EyeTest"
    [MenuItem("Tools/Scenes/EyeTest")]
    static void OpenEyeTest()
    {
        OpenScene("Assets/Scenes/EyeTest.unity");
    }

    // Add a menu item to open "Lobby Quest"
    [MenuItem("Tools/Scenes/Lobby Quest")]
    static void OpenLobbyQuest()
    {
        OpenScene("Assets/Scenes/Lobby Quest.unity");
    }

    // Add a menu item to open "Lobby"
    [MenuItem("Tools/Scenes/Lobby")]
    static void OpenLobby()
    {
        OpenScene("Assets/Scenes/Lobby.unity");
    }

    // Add a menu item to open "MainScene"
    [MenuItem("Tools/Scenes/MainScene")]
    static void OpenMainScene()
    {
        OpenScene("Assets/Scenes/MainScene.unity");
    }

    // Method to open the specified scene
    static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
