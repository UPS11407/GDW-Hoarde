using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using TMPro;

public class Player : MonoBehaviour
{
    public float maxHP;
    public float maxHealCharge = 4.0f;

    public GameObject camera;
    public LayerMask interactibleMask;
    public GameObject interactText;
    public float interactRange;
    public TMP_Text healText;

    PlayerControlsManager playerControlsManager;
    HurtIndicator hurtIndicator;

    public AudioSource audioPlayer;
    float healCharge;
    float currentHP;

    float meleeDelay = 1.0f;
    float meleeTime;
    [SerializeField] float meleeDamage = 1.0f;
    [SerializeField] float meleeKnockbackStrength = 250.0f;

    string[] bindings;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthDisplay();
        hurtIndicator = GetComponent<HurtIndicator>();
        playerControlsManager = GetComponent<PlayerControlsManager>();
        bindings = new string[2];

        UpdateBindings();
    }

    private void Update()
    {
        if(healCharge == maxHealCharge)
        {
            healText.gameObject.SetActive(true);
        }
        else
        {
            healText.gameObject.SetActive(false);
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
        hurtIndicator.Hurt();
    }

    public void TakeDamageOverTime(float dmg)
    {
        currentHP -= dmg;
        //audioPlayer.Play(); make this play at a rate
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
        GetComponent<HealthUI>().SetHealth(currentHP / maxHP);
    }

    /// <summary>
    /// Interact with closest object the player is looking at within the interact range.
    /// </summary>
    public void InteractWithObject()
    {
        RaycastHit rayhit;

        Physics.Raycast(origin: Camera.main.transform.position, direction: Camera.main.transform.forward, out rayhit, maxDistance: interactRange, layerMask: 1 << 8, QueryTriggerInteraction.Collide);
        //Debug.DrawRay(start: Camera.main.transform.position, dir: Camera.main.transform.forward * 10, Color.red, 60);

        if (rayhit.collider != null)
        {
            rayhit.collider.gameObject.GetComponent<IInteractible>().Interact();
        }
    }

    void CheckIfInteractible()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, interactRange, layerMask: 1 << 8, QueryTriggerInteraction.Collide))
        {
            interactText.SetActive(true);
            interactText.GetComponent<TextMeshProUGUI>().text = $"{bindings[0]} - {hit.transform.name}";
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
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4.0f))
            {

                if (hit.transform.tag == "Enemy")
                {

                    hit.transform.gameObject.GetComponent<EnemyBase>().TakeDamage(meleeDamage);
                    StartCoroutine(Knockback(hit));
                    //hit.transform.gameObject.GetComponent<EnemyBase>().Knockback();



                }
            }
        }
        

    }
    IEnumerator Knockback(RaycastHit hit)
    {
        hit.transform.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        hit.transform.gameObject.GetComponent<Rigidbody>().velocity = (hit.transform.position - gameObject.transform.position).normalized * meleeKnockbackStrength;
        Debug.Log((hit.transform.position - gameObject.transform.position).normalized * meleeKnockbackStrength);
        
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("True");
        //hit.transform.gameObject.GetComponent<EnemyBase>().UpdateSpeed();

        hit.transform.gameObject.GetComponent<NavMeshAgent>().enabled = true;

    }

    public void UpdateBindings()
    {
        bindings[0] = InputControlPath.ToHumanReadableString(
                playerControlsManager.playerInput.actions["Interact"].bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

        bindings[1] = InputControlPath.ToHumanReadableString(
                playerControlsManager.playerInput.actions["Heal"].bindings[0].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

        healText.text = $"Press {bindings[1]} to Heal";
    }
}
