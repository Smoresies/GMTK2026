using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGameUiManager : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private GameObject endOfGameUiPanel;
    [SerializeField]
    private Button goToMenuButton;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject winGameText;
    [SerializeField]
    private GameObject loseGameText;

    void Awake()
    {
        winGameText.SetActive(false);
        loseGameText.SetActive(false);
        endOfGameUiPanel.SetActive(false);
        goToMenuButton.onClick.AddListener(GoToMenu);
        playerController.OnDeath += OnLose;
        levelManager.OnWinGame += OnWin;
    }

    private void OnLose()
    {
        loseGameText.SetActive(true);
        ShowMenu();
    }

    private void OnWin()
    {
        ShowMenu();
        winGameText.SetActive(true);
    }

    private void ShowMenu()
    {
        endOfGameUiPanel.SetActive(true);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
