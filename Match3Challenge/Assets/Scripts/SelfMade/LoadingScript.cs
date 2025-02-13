using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType{
    Boot
}

public class LoadingScript : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;
    
    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync(sceneType.ToString()));
    }

    IEnumerator LoadSceneAsync(string sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
