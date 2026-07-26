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
        Level nextRoom = roomQueue.Dequeue();
        List<GameObject> enemies = new List<GameObject>();
        for(int i =0; i < UnityEngine.Random.Range(1, 5); i++)
        {
            enemies.Add(magicPewPewCultistEnemyPrefab);
        }
        nextRoom.Init(enemies, CompleteLevel);
    }

    private void CompleteLevel()
    {
        shopManager.EnableShop();
    }

    public void OnDebug(InputValue inputValue)
    {
        Debug.Log("Debug key pressed, completing level");
        CompleteLevel();
    }
}
