using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlowManager : MonoBehaviour
{
    public string nextSceneName;

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogWarning("No se asignó el nombre de la siguiente escena.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}