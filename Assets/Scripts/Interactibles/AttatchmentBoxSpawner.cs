using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchmentBoxSpawner : MonoBehaviour
{
    public GameObject attatchmentBox;
    public GameObject[] locations;
    public Transform parent;
    bool spawning = false;
    List<GameObject> validSpawnLocations;
    bool first = true;

    private void Start()
    {
        foreach (GameObject loca in locations)
        {
            if (loca.activeInHierarchy == true)
            {
                validSpawnLocations.Add(loca);
            }
        }

        foreach (GameObject loca in validSpawnLocations)
        {
            var obj = Instantiate(attatchmentBox, loca.transform.position, loca.transform.rotation, parent);
            obj.GetComponent<Chest>().spawnLocation = loca;
            loca.SetActive(false);
        }

        validSpawnLocations.Clear();
    }

    private void Update()
    {
        if (!spawning)
        {
            foreach (GameObject loca in locations)
            {
                if (loca.activeInHierarchy == true)
                {
                    validSpawnLocations.Add(loca);
                }
            }

            if (validSpawnLocations.Count > 0)
            {
                StartCoroutine(RunSpawn(Random.Range(0, validSpawnLocations.Count)));
            }
        }
    }

    IEnumerator RunSpawn(int index)
    {
        spawning = true;
        yield return new WaitForSeconds(120.0f);
        spawning = false;
        var obj = Instantiate(attatchmentBox, validSpawnLocations[index].transform.position, validSpawnLocations[index].transform.rotation, parent);
        obj.GetComponent<Chest>().spawnLocation = validSpawnLocations[index];
        validSpawnLocations[index].SetActive(false);
        validSpawnLocations.Clear();
    }
}
