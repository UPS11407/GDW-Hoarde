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
    public List<Transform> spawnLocationsFirst;
    public List<Transform> spawnLocationsSecond;
    public GameObject player;

    bool canSpawn = true;
    int enemyIndex = 0;
    int locationIndex = 0;

    void SpawnEnemy(Transform spawnLocation)
    {
        spawnLocationsFirst.Sort((x, y) => { return (player.transform.position - x.position).sqrMagnitude.CompareTo((player.transform.position - y.position).sqrMagnitude); });
        spawnLocationsSecond.Sort((x, y) => { return (player.transform.position - x.position).sqrMagnitude.CompareTo((player.transform.position - y.position).sqrMagnitude); });
        enemyIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnLocation.position, Quaternion.Euler(0, 0, 0), enemyCollection.transform);
        enemy.GetComponent<EnemyBase>()._speed = enemySpeed;
    }

    IEnumerator Spawn()
    {
        locationIndex = Random.Range(0, 4);
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        if(player.transform.transform.position.y > 9f)
        {
            SpawnEnemy(spawnLocationsSecond[locationIndex]);
        }
        else
        {
            SpawnEnemy(spawnLocationsFirst[locationIndex]);
        }
            
        canSpawn = true;
    }

    void Update()
    {
        if (canSpawn & enemyCollection.transform.childCount <= maxEnemies)
        {
            StartCoroutine(Spawn());
        }
    }
}