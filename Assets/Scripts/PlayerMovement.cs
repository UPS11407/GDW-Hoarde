using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    //player's speed
    public float moveSpeed = 10f;
    //player's rotation speed
    public float rotateSpeed = 75f;

    //variable for jump force
    public float jumpForce = 5f;
    // check the distance b/w player and environment/ground
    public float distanceToGround = 0.1f;

    // we want to set the layer in the inspector
    public LayerMask groundMask;

    //private float variables for input
    private float vertInput;
    private float horInput;

    //collider and rigidbody variables
    private CapsuleCollider _col;
    private Rigidbody _rb;


    float quickFOV = 75.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if shift is pressed, fov change and change speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 15f;
            Camera.main.fieldOfView = quickFOV;
        }
        else
        {
            Camera.main.fieldOfView = 60.0f;
            moveSpeed = 10f;
        }

        //W & S keys 
        vertInput = Input.GetAxis("Vertical") * moveSpeed;


        //A & D keys
        horInput = Input.GetAxis("Horizontal") * rotateSpeed;

        //if space key is down
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
    }

    private void FixedUpdate()
    {
        Vector3 rotationVar = Vector3.up * horInput;
        //controls our angle using quaternions
        Quaternion angleRotation = Quaternion.Euler(rotationVar * Time.fixedDeltaTime);

        _rb.MoveRotation(_rb.rotation * angleRotation);
        
        if (vertInput == 0)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
        else
        {
            _rb.velocity = new Vector3(transform.forward.x, _rb.velocity.y / vertInput, transform.forward.z) * vertInput;
        }
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
}
