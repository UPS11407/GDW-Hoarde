using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{ 
    //player's speed
    public float moveSpeed = 10f;
    //player's sensitivity
    public float mouseSensitivity = 75f;

    //variable for jump force
    public float jumpForce = 5f;
    // check the distance b/w player and environment/ground
    public float distanceToGround = 0.001f;

    // we want to set the layer in the inspector
    public LayerMask groundMask;

    //collider and rigidbody variables
    private CapsuleCollider _col;
    private Rigidbody _rb;

    public PlayerInput playerControls;

    AudioSource audioSource;

    InputAction move;
    InputAction fire;
    InputAction interact;
    InputAction look;
    InputAction reload;
    InputAction swapMod;
    InputAction heal;
    InputAction sprint;
    InputAction jump;
    InputAction crouch;

    float quickFOV = 75.0f;
    bool jumping;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;

    }

    private void Awake()
    {
        playerControls = new PlayerInput();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        interact = playerControls.Player.Interact;
        look = playerControls.Player.Look;
        reload = playerControls.Player.Reload;
        swapMod = playerControls.Player.SwapMod;
        heal = playerControls.Player.Heal;
        sprint = playerControls.Player.Sprint;
        jump = playerControls.Player.Jump;
        crouch = playerControls.Player.Crouch;

        sprint.performed += Sprint;

        //fire.performed += Fire;         <-- do firing method here
        //interact.performed += Interact; <-- do interact method here
        //heal.performed += Heal;         <-- do healing method here
        //swapMod.performed += SwapMod;   <-- do swap mod method here
        //reload.performed += Reload;     <-- do reload method here


        move.Enable();
        fire.Enable();
        interact.Enable();
        look.Enable();
        reload.Enable();
        swapMod.Enable();
        heal.Enable();
        sprint.Enable();
        jump.Enable();
        crouch.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        interact.Disable();
        look.Disable();
        reload.Disable();
        swapMod.Disable();
        heal.Disable();
        sprint.Disable();
        jump.Disable();
        crouch.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        DoLook(mouseSensitivity);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 15f;
            Camera.main.fieldOfView = quickFOV;
            audioSource.pitch = 1.0f;
        }
        else
        {
            Camera.main.fieldOfView = 60.0f;
            moveSpeed = 10f;
            audioSource.pitch = 0.66f;
        }
        if (_rb.velocity.magnitude >= 9)
        {
            audioSource.mute = false;
        }
        else audioSource.mute = true;
        

        if (!jumping && jump.inProgress && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(WaitForJump());
        }

        //Debug.Log(jumping);
    }

    private void FixedUpdate()
    {
        DoWalk();
    }

    bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x,
            _col.bounds.min.y, _col.bounds.max.z);
        //grounded is a variable which is going to store the result
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom,
            distanceToGround, groundMask, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    void DoLook(float sensitivity)
    {
        Vector2 lookDir = look.ReadValue<Vector2>();
        Vector3 rotationVar = new Vector3(-lookDir.y, lookDir.x, 0);
        transform.eulerAngles += rotationVar * Time.deltaTime * (sensitivity / 3);
    }

    void DoWalk()
    {
        Vector2 moveDir = move.ReadValue<Vector2>();
        Vector3 moveX = moveDir.y * new Vector3(transform.forward.x, 0, transform.forward.z).normalized * moveSpeed;
        Vector3 moveZ = moveDir.x * new Vector3(transform.right.x, 0, transform.right.z).normalized * moveSpeed;

        if (moveDir == Vector2.zero)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        else
        {
            _rb.velocity = moveX + moveZ + new Vector3(0, _rb.velocity.y, 0);
        }

        
    }

    void Sprint(InputAction.CallbackContext context)
    {
        
    }

    IEnumerator WaitForJump()
    {
        jumping = true;

        yield return new WaitForSeconds(0.05f);

        jumping = false;
    }
}
