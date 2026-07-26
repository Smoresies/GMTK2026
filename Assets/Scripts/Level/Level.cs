
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    public event Action CompleteLevelEvent;
    private int numEnemiesRemaining;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int minXSpawnLocation;
    [SerializeField]
    private int maxXSpawnLocation;
    [SerializeField]
    private int minYSpawnLocation;
    [SerializeField]
    private int maxYSpawnLocation;
    [SerializeField]
    private float waitForSpawnTime = 2f;

    private GameObject level;

    public void Init(MonoBehaviour host, List<GameObject> enemyPrefabsToSpawn, Action CompleteEventAction)
    {
        numEnemiesRemaining = 0;
        level = Instantiate(prefab);

        CompleteLevelEvent += CompleteEventAction;
        host.StartCoroutine(WaitAndSpawnEnemies(enemyPrefabsToSpawn));
    }

    IEnumerator WaitAndSpawnEnemies(List<GameObject> enemyPrefabsToSpawn)
    {
        Debug.Log("Game started...");

        yield return new WaitForSeconds(waitForSpawnTime);

        foreach (GameObject enemyPrefab in enemyPrefabsToSpawn)
        {
            GameObject enemy = Instantiate(enemyPrefab, level.transform);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if(player.curse2)
                enemyController.Curse2();
            enemy.transform.position = GetRandomSpawnLocation();
            enemyController.OnDeathEvent += EnemyDied;
            numEnemiesRemaining++;
        }
    }


    public void EnemyDied()
    {
        numEnemiesRemaining--;
        if (IsLevelComplete())
        {
            CompleteLevel();
        }
    }

    private bool IsLevelComplete()
    {
        Debug.Log("num enemies remaining = " + numEnemiesRemaining);
        return numEnemiesRemaining <= 0;
    }

    public void CompleteLevel()
    {
        CompleteLevelEvent?.Invoke();
        Destroy(level);
    }

    private Vector2 GetRandomSpawnLocation()
    {
        return new Vector2(UnityEngine.Random.Range(minXSpawnLocation, maxXSpawnLocation), UnityEngine.Random.Range(minYSpawnLocation, maxYSpawnLocation));
    }
}
