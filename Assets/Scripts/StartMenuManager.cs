using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnStartButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
