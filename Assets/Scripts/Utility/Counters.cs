using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counters : MonoBehaviour
{
    int kills = 0;
    float time = 0;
    string killedBy = "NULL";

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }

    public void AddKill(int val)
    {
        kills += val;
    }

    public int GetKills()
    {
        return kills;
    }

    public string GetKilledBy()
    {
        return killedBy;
    }

    public void SetKilledBy(string killer)
    {
        killedBy = killer;
    }

    public float GetTime()
    {
        return time;
    }
}
