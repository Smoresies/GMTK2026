using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private List<RoomPrefabWithWeight> listOfPrefabsWithWeights;
    [SerializeField]
    private GameObject firstRoomPrefab;
    [SerializeField]
    private GameObject lastRoomPrefab;
    [SerializeField][Min(2)]
    private int totalNumberOfRooms = 5;
    private GameObject instantiatedLevel;
    private Queue<GameObject> roomQueue = new Queue<GameObject>();

    [Serializable]
    public struct RoomPrefabWithWeight
    {
        public GameObject prefab;
        public int weight;
    }
    void Awake()
    {
        int totalWeight = 0;
        foreach (RoomPrefabWithWeight room in listOfPrefabsWithWeights)
        {
            totalWeight += room.weight;
        }
        roomQueue.Enqueue(firstRoomPrefab);
        for (int i = 0; i < totalNumberOfRooms - 2; i++)
        {
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            foreach (RoomPrefabWithWeight room in listOfPrefabsWithWeights)
            {
                randomWeight -= room.weight;
                if (randomWeight <= 0)
                {
                    roomQueue.Enqueue(room.prefab);
                    break;
                }
            }
        }
        roomQueue.Enqueue(lastRoomPrefab);
        SetNextRoom();
    }

    private void SetNextRoom()
    {
        Destroy(instantiatedLevel);
        GameObject nextRoom = roomQueue.Dequeue();
        instantiatedLevel = Instantiate(nextRoom, Vector3.zero, Quaternion.identity);
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
