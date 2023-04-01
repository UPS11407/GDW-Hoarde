using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxSpawner : MonoBehaviour
{
    public GameObject ammoBox;
    public GameObject[] locations;
    public Transform parent;
    bool spawning = false;
    List<GameObject> validSpawnLocations = new List<GameObject>();

    private void Update()
    {
        if (!spawning)
        {
            foreach (GameObject loca in locations)
            {
                if (loca.activeInHierarchy == true)
                {
                    //Debug.Log(loca.name);
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
        yield return new WaitForSeconds(90.0f);
        spawning = false;
        GameObject obj = Instantiate(ammoBox, locations[index].transform.position, locations[index].transform.rotation, parent);
        obj.GetComponent<AmmoBox>().spawnLocation = validSpawnLocations[index];
        obj.name = "Ammo Box";
        validSpawnLocations.Clear();
    }
}
