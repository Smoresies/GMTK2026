using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private List<WeightedObject<Level>> listOfLevelsWithWeights;
    [SerializeField]
    private Level firstRoomPrefab;
    [SerializeField]
    private Level lastRoomPrefab;
    [SerializeField][Min(2)]
    private int totalNumberOfRooms = 5;
    private Queue<Level> roomQueue = new Queue<Level>();
    [SerializeField]
    private List<WeightedObject<GameObject>> enemyPrefabList;
    [SerializeField]
    private int minNumEnemies = 2;
    [SerializeField]
    private int maxNumEnemies = 5;
    private int stage = 0;
    void Awake()
    {
        shopManager.OnShopClosed += SetNextRoom;
        roomQueue.Enqueue(firstRoomPrefab);
        for (int i = 0; i < totalNumberOfRooms - 2; i++)
        {
            roomQueue.Enqueue(Utils.GetRandomWeightedObject(listOfLevelsWithWeights).item);
        }
        roomQueue.Enqueue(lastRoomPrefab);
        SetNextRoom();
    }

    private void SetNextRoom()
    {
        Time.timeScale = 1;
        stage++;
        Debug.Log("You on stage: " + stage + " lil cutie");
        Level nextRoom = roomQueue.Dequeue();
        List<GameObject> enemies = new List<GameObject>();
        PlayerController player = FindAnyObjectByType<PlayerController>();
        int enemyNum = (int)(UnityEngine.Random.Range(minNumEnemies, maxNumEnemies + 1) * (player.curse7 ? 1.5f : 1.0f));
        for(int i =0; i < enemyNum; i++)
        {
            enemies.Add(Utils.GetRandomWeightedObject(enemyPrefabList).item);
        }
        nextRoom.Init(this, enemies, CompleteLevel);
    }

    private void CompleteLevel()
    {
        Time.timeScale = 0;
        shopManager.EnableShop();
    }

    public void OnDebug(InputValue inputValue)
    {
        Debug.Log("Debug key pressed, completing level");
        CompleteLevel();
    }
}
