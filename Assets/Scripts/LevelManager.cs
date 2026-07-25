using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private List<WeightedObject<GameObject>> listOfPrefabsWithWeights;
    [SerializeField]
    private GameObject firstRoomPrefab;
    [SerializeField]
    private GameObject lastRoomPrefab;
    [SerializeField][Min(2)]
    private int totalNumberOfRooms = 5;
    private GameObject instantiatedLevel;
    private Queue<GameObject> roomQueue = new Queue<GameObject>();

    void Awake()
    {
        roomQueue.Enqueue(firstRoomPrefab);
        for (int i = 0; i < totalNumberOfRooms - 2; i++)
        {
            roomQueue.Enqueue(Utils.GetRandomWeightedObject(listOfPrefabsWithWeights).item);
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
