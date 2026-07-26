using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject biiiiigHorrorMonsterEnemyPrefab;
    [SerializeField]
    private GameObject daggerCultistEnemyPrefab;
    [SerializeField]
    private GameObject littleHorrorMonsterEnemyPrefab;
    [SerializeField]
    private GameObject magicPewPewCultistEnemyPrefab;
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
        Level nextRoom = roomQueue.Dequeue();
        List<GameObject> enemies = new List<GameObject>();
        PlayerController player = FindAnyObjectByType<PlayerController>();
        int enemyNum = (int)(UnityEngine.Random.Range(2, 5) * (player.curse7 ? 1.5f : 1.0f));
        for(int i =0; i < enemyNum; i++)
        {
            enemies.Add(magicPewPewCultistEnemyPrefab);
        }
        nextRoom.Init(enemies, CompleteLevel);
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
