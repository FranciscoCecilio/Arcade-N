using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
 
// This script makes the Play button in the Editor jump to the 1st scene in the Building Settings 
[InitializeOnLoad]
public class AutoPlayModeSceneSetup
{
    static AutoPlayModeSceneSetup()
    {
        // Ensure at least one build scene exist.
        if (EditorBuildSettings.scenes.Length == 0)
            return;
 
        // Set Play Mode scene to first scene defined in build settings.
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[1].path);
    }
}