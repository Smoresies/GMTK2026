using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<WeightedObject<GameObject>> prefabs;
    [SerializeField]
    private int spawnCount;

}
