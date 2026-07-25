using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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
}
