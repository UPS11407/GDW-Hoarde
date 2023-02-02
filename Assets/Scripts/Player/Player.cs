using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    public float maxHP;
    public Image hpImage;
    public Image healChargeBar;
    public float maxHealCharge = 4.0f;

    public GameObject camera;
    public LayerMask interactibleMask;
    public GameObject interactText;
    public float interactRange;
    public GameObject healText;


    public AudioSource audioPlayer;
    float healCharge;
    float currentHP;

    float meleeDelay = 1.0f;
    float meleeTime;
    float meleeDamage = 4.0f;
    void Start()
    {
        currentHP = maxHP;
        UpdateHealthDisplay();
    }

    private void Update()
    {
        if(healCharge == maxHealCharge)
        {
            healText.SetActive(true);
        }
        else
        {
            healText.SetActive(false);
        }

        if(currentHP <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneControl.ChangeScene("Death");
        }
        CheckIfInteractible();
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
        //audioClip;
        //audioPlayer.Stop();
        currentHP -= dmg;
        audioPlayer.Play();
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

    /// <summary>
    /// Interact with closest object the player is looking at within the interact range.
    /// </summary>
    public void InteractWithObject()
    {
        RaycastHit rayhit;

        Physics.Raycast(origin: Camera.main.transform.position, direction: Camera.main.transform.forward, out rayhit, maxDistance: interactRange, layerMask: 1 << 8);
        //Debug.DrawRay(start: Camera.main.transform.position, dir: Camera.main.transform.forward * 10, Color.red, 60);

        if (rayhit.collider != null)
        {
            rayhit.collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }

    void CheckIfInteractible()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, interactRange, interactibleMask))
        {
            interactText.SetActive(true);
            interactText.GetComponent<TextMeshProUGUI>().text = $"(E) - {hit.transform.name}";
        }
        else
        {
            interactText.SetActive(false);
        }
    }

    public void QuickMelee()
    {
        RaycastHit hit;
        if (Time.time > meleeTime + meleeDelay)
        {
            meleeTime = Time.time;
            Debug.Log("Punch");
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4.0f))
            {

                if (hit.transform.tag == "Enemy")
                {

                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(meleeDamage);
                    


                }
            }
        }
        

    }
    

}
