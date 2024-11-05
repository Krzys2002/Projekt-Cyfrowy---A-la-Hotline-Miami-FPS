using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLouder : MonoBehaviour
{
    // scene to load
    public string sceneToLoad;
    
    // load scene
    public void LoadScene()
    {
        // check if scene to load is set
        if(sceneToLoad != null)
        {
            // load scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
