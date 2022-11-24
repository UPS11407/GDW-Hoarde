using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxHP;
    public Image hpImage;
    public Image healChargeBar;
    public float maxHealCharge = 4.0f;
    public float interactRange;

    PlayerInputs playerInputs;
    InputAction heal;
    InputAction interact;
    float healCharge;
    float currentHP;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        heal = playerInputs.Player.Heal;
        heal.Enable();
        heal.performed += ctx => HealHP(maxHP * 0.3f, true);
        interact = playerInputs.Player.Interact;
        interact.Enable();
        interact.performed += ctx => InteractWithObject();
    }

    private void OnDisable()
    {
        heal.Disable();
        interact.Disable();
    }

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
    /// Interact with closest object the player is looking at within the interact range.
    /// </summary>
    void InteractWithObject()
    {
        RaycastHit rayhit;

        Physics.Raycast(origin: Camera.main.transform.position, direction: Camera.main.transform.forward, out rayhit, maxDistance: interactRange, layerMask: 1 << 8);
        //Debug.DrawRay(start: Camera.main.transform.position, dir: Camera.main.transform.forward * 10, Color.red, 60);

        if(rayhit.collider != null)
        {
            rayhit.collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }

    /// <summary>
    /// Adds val to the current heal charge bar. Does not exceed max charge
    /// </summary>
    public void AddHealCharge(float val)
    {
        healCharge += val;

        if(healCharge > maxHealCharge)
        {
            healCharge = maxHealCharge;
        }

        UpdateHealthDisplay();
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
    public void HealHP(float heal, bool useCharge)
    {
        if (useCharge)
        {
            if(healCharge == maxHealCharge)
            {
                healCharge = 0;
                currentHP += heal;

                if (currentHP > maxHP)
                {
                    currentHP = maxHP;
                }
            }
        }
        else
        {
            currentHP += heal;

            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
        }

        UpdateHealthDisplay();
    }

    void UpdateHealthDisplay()
    {
        hpImage.fillAmount = currentHP / maxHP;
        healChargeBar.fillAmount = healCharge / maxHealCharge;
    }
}
