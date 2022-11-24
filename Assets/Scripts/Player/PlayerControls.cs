using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
public class PlayerControls : MonoBehaviour
{
    public Rigidbody reggy;
    public float speed;
    Vector2 movementDir = Vector2.zero;
    InputAction move;
    InputAction fire;
    InputAction interact;
    InputAction look;
    InputAction reload;
    InputAction swapMod;
    InputAction heal;
    InputAction sprint;
    public PlayerInput playerContr;


    [SerializeField] AudioSource walkAudioSource;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float sprintSpeedIncrease = 15.0f;
    public float originalSpeed;

    [SerializeField] GameObject camera;
    [SerializeField] LayerMask interactibleLayer;
    [SerializeField] float interactRange;
    [SerializeField] GameObject interactText;
    


    // Start is called before the first frame update
    void Awake()
    {
        originalSpeed = speed;
        playerContr = new PlayerInput();
        
        reggy = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        movementDir = move.ReadValue<Vector2>();
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = originalSpeed + sprintSpeedIncrease;
        }
        else
        {
            speed = originalSpeed;
        }
    }

    private void FixedUpdate()
    {
        reggy.velocity = transform.right * movementDir.x * speed + transform.forward * movementDir.y * speed;
        if (reggy.velocity != new Vector3(0, 0, 0))
        {
            walkAudioSource.mute = false;
        } else
        {
            walkAudioSource.mute = true;
        }
    }
    private void OnEnable()
    {
        move = playerContr.Player.Move;
        fire = playerContr.Player.Fire;
        interact = playerContr.Player.Interact;
        look = playerContr.Player.Look;
        reload = playerContr.Player.Reload;
        swapMod = playerContr.Player.SwapMod;
        heal = playerContr.Player.Heal;
        sprint = playerContr.Player.Sprint;


        //fire.performed += Fire;         <-- do firing method here

        //heal.performed += Heal;         <-- do healing method here
        //swapMod.performed += SwapMod;   <-- do swap mod method here
        //reload.performed += Reload;     <-- do reload method here

        //interact.performed += ctx => InteractWithObject();

        move.Enable();
        fire.Enable();
        interact.Enable();
        look.Enable();
        reload.Enable();
        swapMod.Enable();
        heal.Enable();
        sprint.Enable();

    }
    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }

    
}
