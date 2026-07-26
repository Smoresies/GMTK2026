using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private GameObject startMenuUI;
    [SerializeField]
    private LevelManager levelManager;

    private void Awake()
    {
        Time.timeScale = 0;
        startButton.onClick.AddListener(OnStartButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
        startMenuUI.SetActive(true);
    }
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnStartButtonPressed()
    {
        startMenuUI.SetActive(false);
        levelManager.StartGame();
    }
}
