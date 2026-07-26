using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource menuMusic;
    [SerializeField]
    private AudioSource stageMusic;
    [SerializeField]
    private AudioSource shopMusic;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button shopButton;

    void Start()
    { 
        menuMusic.Play();
        startButton.onClick.AddListener(StartGame);
        shopButton.onClick.AddListener(ResumeGame);
    }

    // When start button is pressed
    public void StartGame()
    {
        menuMusic.Stop();
        stageMusic.Play();
    }

    // Post level completeion
    public void ShopTime()
    {
        stageMusic.Pause();
        shopMusic.Play();
    }

    // Leaving Shop
    public void ResumeGame()
    {
        shopMusic.Stop();
        stageMusic.UnPause();
    }

    // Gameover
    public void EndGame()
    {
        stageMusic.Stop();
        menuMusic.Play();
    }
}
