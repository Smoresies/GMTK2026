using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
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

    [SerializeField]
    private AudioSource buttonClick;

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

    private IEnumerator PlaySoundAndContinueRoutine()
    { 
        buttonClick.pitch *= Random.Range(0.9f, 1.1f);
        buttonClick.Play();
        // buttonClick.pitch = 0.85f;

        // Option 1: Wait until the audio source stops playing (safe if pitch changes)
        yield return new WaitWhile(() => buttonClick.isPlaying);

        // Option 2: Alternatively, wait for the exact length of the clip in seconds
        // yield return new WaitForSeconds(soundEffect.length);

        // Continue your code here after the sound finishes
        Debug.Log("Sound finished! Continuing code...");

        startMenuUI.SetActive(false);
        levelManager.StartGame();
    }

    public void OnStartButtonPressed()
    {
        StartCoroutine(PlaySoundAndContinueRoutine());
    }
}
