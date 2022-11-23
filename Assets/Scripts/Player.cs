using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHP;
    public Image hpImage;
    float currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthDisplay();
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
        UpdateHealthDisplay();
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

        UpdateHealthDisplay();
    }

    void UpdateHealthDisplay()
    {
        hpImage.fillAmount = currentHP / maxHP;
    }
}
