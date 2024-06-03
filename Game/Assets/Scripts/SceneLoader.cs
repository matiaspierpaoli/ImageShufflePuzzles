using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWithString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithInt(int sceneIndexer)
    {
        SceneManager.LoadScene(sceneIndexer);
    }

    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
