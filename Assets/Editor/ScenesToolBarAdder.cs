using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesToolBarAdder : Editor
{
    [MenuItem("Scenes/Bootstrap")]
    private static void LoadBootstrap()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Bootstrap.unity");
    }

    [MenuItem("Scenes/Brick Editor")]
    private static void LoadBrickEditor()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/BrickEditor.unity");
    }

    [MenuItem("Scenes/Main Menu")]
    private static void LoadMainMenu()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scenes/Play Mode")]
    private static void LoadPlayMode()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/PlayMode.unity");
    }
}
