using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    public const string RebindsKey = "rebinds";

    Player player;
    WeaponManager weaponManager;
    public PauseMenu pauseMenu;

    public PlayerInput playerInput;

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

    public bool enableLook = true;
    bool onStairs = false;
    AudioSource audioSource;

    float quickFOV = 75.0f;
    bool jumping;

    public float speed;

    float xRotation;
    float yRotation;

    public bool sprinting;
    bool resetSprint;
    bool resetWHileSprinting;

    public float staminaToRun = 1;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        weaponManager = GetComponent<WeaponManager>();

        InitPlayerActions();

        GetRebinds();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;
    }

    private void OnDisable()
    {
        DeinitPlayerActions();
    }
    
    public void ToggleMenu()
    {
        if (weaponManager.guns[weaponManager.activeGun].weaponModCanvas.activeSelf) CloseMenu();
        else OpenMenu();
    }

    public void OpenMenu()
    {
        enableLook = false;
        weaponManager.guns[weaponManager.activeGun].canShoot = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        weaponManager.ToggleWeaponModCanvas(true);
    }

    public void CloseMenu()
    {
        enableLook = true;
        weaponManager.guns[weaponManager.activeGun].canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        weaponManager.ToggleWeaponModCanvas(false);
        weaponManager.UpdateWeaponStats();
    }

    void Update()
    {
        speed = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).magnitude;

        if (enableLook) DoLook(mouseSensitivity);

        if (speed >= 5.5 && IsGrounded())
        {
            audioSource.mute = false;
        }
        else audioSource.mute = true;


        if (!jumping && playerInput.actions["Jump"].inProgress && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(WaitForJump());
        }

        Sprint();

        if (sprinting && speed > 1 && IsGrounded())
        {
            player.TakeStamina(staminaToRun * Time.smoothDeltaTime);
            ResetSprintVariables();
        }
        else if (sprinting && speed < 1 && resetWHileSprinting)
        {
            player.timeSinceUsedStamina = 0;
            resetWHileSprinting = false;
        }

        if (sprinting && !(player.stamina > 0))
        {
            sprinting = false;
        }
    }

    void DoLook(float sensitivity)
    {
        Vector2 lookDir = playerInput.actions["Look"].ReadValue<Vector2>();

        if (lookDir == Vector2.zero)
        {
            return;
        }

        lookDir = lookDir * Time.smoothDeltaTime * (sensitivity / 3);

        yRotation += lookDir.x;
        xRotation -= lookDir.y;
        xRotation = Mathf.Clamp(xRotation, -89, 89);

        cameraParent.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }

    void DoWalk()
    {
        Vector2 moveDir = playerInput.actions["Move"].ReadValue<Vector2>();
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

    private void Sprint()
    {
        if (sprinting && player.stamina > 0)
        {
            moveSpeed = 9f;
            Camera.main.fieldOfView = quickFOV;
            audioSource.pitch = 1.0f;
            resetSprint = true;
        }
        else if (resetSprint)
        {
            Camera.main.fieldOfView = 60.0f;
            moveSpeed = 6f;
            audioSource.pitch = 0.66f;
            resetSprint = false;
            if (player.timeSinceUsedStamina > 0.1f)
            {
                player.timeSinceUsedStamina = 0;
            }
        }
    }

    IEnumerator WaitForJump()
    {
        jumping = true;

        yield return new WaitForSeconds(0.05f);

        jumping = false;
    }

    void InitPlayerActions()
    {
        playerInput.actions["Fire"].started += ctx => weaponManager.ToggleFire(true);
        playerInput.actions["Fire"].canceled += ctx => weaponManager.ToggleFire(false);
        playerInput.actions["Interact"].performed += ctx => player.InteractWithObject();
        playerInput.actions["Reload"].performed += ctx => weaponManager.Reload();
        playerInput.actions["SwapMod"].performed += ctx => ToggleMenu();
        playerInput.actions["Heal"].performed += ctx => player.HealHP(player.maxHP * 0.3f, true);
        playerInput.actions["Sprint"].started += ctx => ResetSprintVariables();
        playerInput.actions["Sprint"].canceled += ctx => sprinting = false;
        playerInput.actions["SwapWeapon"].performed += ctx => weaponManager.SwapWeapon();
        playerInput.actions["Pause"].performed += ctx => pauseMenu.RunPause();
        playerInput.actions["Melee"].performed += ctx => player.QuickMelee();
        playerInput.actions.FindActionMap("Menu").FindAction("Pause").performed += ctx => pauseMenu.RunPause();
    }

    void DeinitPlayerActions()
    {
        playerInput.actions["Fire"].started -= ctx => weaponManager.ToggleFire(true);
        playerInput.actions["Fire"].canceled -= ctx => weaponManager.ToggleFire(false);
        playerInput.actions["Interact"].performed -= ctx => player.InteractWithObject();
        playerInput.actions["Reload"].performed -= ctx => weaponManager.Reload();
        playerInput.actions["SwapMod"].performed -= ctx => ToggleMenu();
        playerInput.actions["Heal"].performed -= ctx => player.HealHP(player.maxHP * 0.3f, true);
        playerInput.actions["Sprint"].performed -= ctx => ResetSprintVariables();
        playerInput.actions["Sprint"].canceled -= ctx => sprinting = false;
        playerInput.actions["SwapWeapon"].performed += ctx => weaponManager.SwapWeapon();
        playerInput.actions["Pause"].performed -= ctx => pauseMenu.RunPause();
        playerInput.actions["Melee"].performed -= ctx => player.QuickMelee();
        playerInput.actions.FindActionMap("Menu").FindAction("Pause").performed -= ctx => pauseMenu.RunPause();
    }

    public void ReinitPlayerActions()
    {
        DeinitPlayerActions();

        InitPlayerActions();
    }

    public void GetRebinds()
    {
        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

        if (string.IsNullOrEmpty(rebinds)) { return; }

        playerInput.actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void SaveBindings()
    {
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(RebindsKey, rebinds);
    }

    public void ClearBindings()
    {
        PlayerPrefs.SetString(RebindsKey, string.Empty);

        playerInput.actions.LoadBindingOverridesFromJson(string.Empty);

        player.UpdateBindings();
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stairs")
        {
            onStairs = true;
        }
        else
        {
            onStairs = false;
        }
    }

    void ResetSprintVariables()
    {
        resetWHileSprinting = true;
        sprinting = true;
    }
}
