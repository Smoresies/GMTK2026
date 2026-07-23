using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> levels;

    void Awake()
    {
        RandomUtils.ShuffleList(levels, true);
    }
}
