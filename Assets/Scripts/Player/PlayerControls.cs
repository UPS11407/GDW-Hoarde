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
    private InputAction move;
    private InputAction interact;
    private InputAction fire;
    private InputAction reload;
    public PlayerInputs playerContr;


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
        playerContr = new PlayerInputs();
        
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
        
        CheckIfInteractible();
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
        move.Enable();
        interact = playerContr.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
        /* 
        fire = playerContr.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
        reload = playerContr.Player.Reload;
        reload.Enable();
        reload.performed += Reload; 
        */
    }
    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }
    

    

    private void Interact(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, interactRange, interactibleLayer))
        {
            Debug.Log(hit.transform.name);
            var interactible = hit.transform.gameObject.GetComponent<IInteractible>();
            if (interactible == null) {
                Debug.Log("noninteractible");
                return;
            }
            
            interactible.Interact();


        }
    }

    void CheckIfInteractible()
    {
        RaycastHit hit;
        if (interactText != null)
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, interactRange, interactibleLayer))
            {
                interactText.SetActive(true);
            }
            else
            {
                interactText.SetActive(false);
            }
        }
        
    }
    
}
