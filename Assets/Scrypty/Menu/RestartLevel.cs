using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // Function to reload the current level
    public void ReloadCurrentLevel()
    {
        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Reload the current scene
        StoreData.LevelData.ClearedLevels.Clear();
        StoreData.LevelData.RespawnPoint = Vector3.zero;
        SceneManager.LoadScene(currentScene.name);
    }
}
