using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public List <GameObject> enemiesList = new List<GameObject>();
    public GameObject winText;
    // Start is called before the first frame update
    void Start()
    {
        winText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int counter = enemiesList.Count-1; counter >= 0; counter--)
        {
            if (enemiesList[counter] == null)
            {
                enemiesList.RemoveAt(counter);
            }

            if (enemiesList.Count == 0)
            {
                winText.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
