using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public int maxEnemies = 10;
    public float spawnDelay;
    public float enemySpeed = 2.0f;

    public GameObject enemyCollection;
    public List<Transform> spawnLocations;
    public GameObject player;

    bool canSpawn = true;
    int enemyIndex = 0;
    int locationIndex = 0;

    private void Start()
    {
        spawnLocations.Sort((x, y) => { return (player.transform.position - x.position).sqrMagnitude.CompareTo((player.transform.position - y.position).sqrMagnitude); });
    }

    void SpawnEnemy(Transform spawnLocation)
    {
        enemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnLocation.position, Quaternion.Euler(0, 0, 0), enemyCollection.transform);
        enemy.GetComponent<EnemyBase>()._speed = enemySpeed;
    }

    IEnumerator Spawn()
    {
        locationIndex = Random.Range(0, 3);
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        SpawnEnemy(spawnLocations[locationIndex]);
        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn & enemyCollection.transform.childCount <= maxEnemies)
        {
            StartCoroutine(Spawn());
        }
    }

    /*

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
    */
}