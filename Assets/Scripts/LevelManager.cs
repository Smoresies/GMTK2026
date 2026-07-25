using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private List<GameObject> levels;
    private GameObject currentLevel;

    void Awake()
    {
        RandomUtils.ShuffleList(levels, true);
        SetNextRoom();
    }

    private void SetNextRoom()
    {
        Destroy(currentLevel);
        GameObject nextRoom = levels[0];
        levels.RemoveAt(0);
        currentLevel = Instantiate(nextRoom, Vector3.zero, Quaternion.identity);
    }

    private void CompleteLevel()
    {
        shopManager.EnableShop();
        SetNextRoom();
    }

    public void OnDebug(InputValue inputValue)
    {
        Debug.Log("Debug key pressed, completing level");
        CompleteLevel();
    }
}
