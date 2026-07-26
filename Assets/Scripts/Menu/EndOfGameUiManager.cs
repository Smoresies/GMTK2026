using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGameUiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject endOfGameUiPanel;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private PlayerController playerController;

    void Awake()
    {
        endOfGameUiPanel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        playerController.OnDeath += ShowMenu;
    }

    private void ShowMenu()
    {
        endOfGameUiPanel.SetActive(true);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
