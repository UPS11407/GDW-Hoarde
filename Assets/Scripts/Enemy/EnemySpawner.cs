using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public int maxEnemies = 10;
    public float spawnDelayMin;
    public float spawnDelayMax;
    public float enemySpeed = 2.0f;

    public GameObject enemyCollection;
    public List<Transform> spawnLocationsFirst;
    public List<Transform> spawnLocationsSecond;
    public GameObject player;

    bool canSpawn = true;
    int enemyIndex = 0;

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
        canSpawn = false;
        yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));

        for (int i = 0; i < Random.Range(2,5); i++)
        {
            if (player.transform.transform.position.y > 5f)
            {
                SpawnEnemy(spawnLocationsSecond[i]);
            }
            else
            {
                SpawnEnemy(spawnLocationsFirst[i]);
            }
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