using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string currentSceneName;

    public void Initialize()
    {
        Debug.Log("SceneLoader initialized.");
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            yield return SceneManager.UnloadSceneAsync(currentSceneName);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentSceneName = sceneName;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        Debug.Log($"Scene '{sceneName}' loaded and set as active.");
    }
}