using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{ 
    //player's speed
    public float moveSpeed = 6f;
    //player's sensitivity
    public float mouseSensitivity = 75f;

    //variable for jump force
    public float jumpForce = 5f;
    // check the distance b/w player and environment/ground
    private float distanceToGround = 0.001f;

    // we want to set the layer in the inspector
    public LayerMask groundMask;

    //collider and rigidbody variables
    private CapsuleCollider _col;
    private Rigidbody _rb;

    public Transform cameraParent;

    public PlayerInput playerControls;

    public bool enableLook = true;

    AudioSource audioSource;

    InputAction move;
    InputAction jump;
    InputAction look;


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
        jump = playerControls.Player.Jump;
        look = playerControls.Player.Look;

        move.Enable();
        jump.Enable();
        look.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        look.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableLook) DoLook(mouseSensitivity);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 9f;
            Camera.main.fieldOfView = quickFOV;
            audioSource.pitch = 1.0f;
        }
        else
        {
            Camera.main.fieldOfView = 60.0f;
            moveSpeed = 6f;
            audioSource.pitch = 0.66f;
        }
        if (_rb.velocity.magnitude >= 5.5 && IsGrounded())
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

    public bool IsGrounded()
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
        Vector3 currentRotation = rotationVar * Time.deltaTime * (sensitivity / 3);

        if ((cameraParent.eulerAngles + currentRotation).x <= 80 && (cameraParent.eulerAngles + currentRotation).x >= -1)
        {
            cameraParent.eulerAngles += currentRotation;
        }
        else if ((cameraParent.eulerAngles + currentRotation).x <= 361 && (cameraParent.eulerAngles + currentRotation).x >= 290)
        {
            cameraParent.eulerAngles += currentRotation;
        }
        else return;


    }

    void DoWalk()
    {
        Vector2 moveDir = move.ReadValue<Vector2>();
        Vector3 moveX = moveDir.y * new Vector3(cameraParent.forward.x, 0, cameraParent.forward.z).normalized * moveSpeed;
        Vector3 moveZ = moveDir.x * new Vector3(cameraParent.right.x, 0, cameraParent.right.z).normalized * moveSpeed;

        if (moveDir == Vector2.zero)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        else
        {
            _rb.velocity = moveX + moveZ + new Vector3(0, _rb.velocity.y, 0);
        }
    }

    public void Sprint(InputAction.CallbackContext context, bool keyDown)
    {
        
    }

    IEnumerator WaitForJump()
    {
        jumping = true;

        yield return new WaitForSeconds(0.05f);

        jumping = false;
    }
}
