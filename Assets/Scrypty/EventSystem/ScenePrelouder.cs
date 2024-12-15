using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreloader : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private bool canChangeScene = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PreloadScene(sceneName));
    }

    // Coroutine to preload the scene
    private IEnumerator PreloadScene(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Check if the scene has been fully loaded
            if (asyncOperation.progress >= 0.90f)
            {
                // Scene is preloaded, you can now activate it
                break;
            }
            
            Debug.Log("Scene is loading: " + asyncOperation.progress);

            yield return null;
        }
        
        Debug.Log("Scene is loaded: " + asyncOperation.progress);
    }
    
    public void ChangeSceneAfterPreload()
    {
        if (!canChangeScene)
        {
            return;
        }
        Debug.Log("Changing scene after preload");
        StartCoroutine(ChangeScene());
        //SceneManager.CreateScene(sceneName);
    }

    // Function to change the scene after it has been preloaded
    private IEnumerator ChangeScene()
    {
        if(!canChangeScene)
        {
            yield return null;
        }
        while (asyncOperation != null && asyncOperation.progress < 0.9f)
        {
            Debug.Log("Scene is loading: " + asyncOperation.progress);
            yield return null;
        }

        if (asyncOperation != null)
        {
            Debug.Log("Scene is loaded: " + asyncOperation.progress);
            asyncOperation.allowSceneActivation = true;
        }
    }
    
    public void setCanChangeScene(bool canChangeScene)
    {
        this.canChangeScene = canChangeScene;
    }
}