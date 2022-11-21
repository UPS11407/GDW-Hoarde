using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public float spawnDelay;
    public float spawnRange = 100.0f;
    public int maxEnemies = 10;
    public float enemySpeed = 2.0f;

    public GameObject enemyCollection;
    public Transform spawnLocation;
    public GameObject player;

    float timer = 0.0f;
    int enemyIndex = 0;

    private void Awake()
    {
        if(player == null)
        {
            Debug.LogError("ERROR: Player object no defined on " + gameObject.name);
            this.enabled = false;
        }

        if(enemyCollection == null)
        {
            Debug.LogError("ERROR: Enemy collection object not defined on " + gameObject.name);
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < spawnRange)
        {
            if (enemyCollection.transform.childCount <= maxEnemies)
            {
                if (Time.realtimeSinceStartup >= timer)
                {
                    timer = Time.realtimeSinceStartup + spawnDelay;
                    SpawnEnemy();
                }
            }
        }
    }

    void SpawnEnemy()
    {
        enemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnLocation.position, Quaternion.Euler(0, 0, 0), enemyCollection.transform);
        enemy.GetComponent<EnemyBase>()._speed = enemySpeed;
    }
}
