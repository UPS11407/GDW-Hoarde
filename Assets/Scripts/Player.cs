using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHP;
    float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        if(currentHP <= 0)
        {
            //Die
        }
    }

    /// <summary>
    /// Returns the player's current health
    /// </summary>
    public float GetHP()
    {
        return currentHP;
    }

    /// <summary>
    /// Reduces the player's health by dmg
    /// </summary>
    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
    }

    /// <summary>
    /// Adds heal to player's currentHP. Does not exceed max hp
    /// </summary>
    public void HealHP(float heal)
    {
        currentHP += heal;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
