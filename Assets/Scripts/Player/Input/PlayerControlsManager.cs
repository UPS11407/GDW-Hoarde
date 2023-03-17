using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.VersionControl;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    public const string RebindsKey = "rebinds";

    InfoBoxText infoBoxText;

    Player player;
    WeaponManager weaponManager;

    public GameObject flashlight;

    public PauseMenu pauseMenu;
    public GameObject HUD;

    public GameObject inventoryMenu;

    public Inventory inventory;

    public PlayerInput playerInput;

    //playeqr's speed
    public float moveSpeed = 6f;
    //player's sensitivity
    public float mouseSensitivity = 6f;

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
    bool canSprint;

    public float staminaToRun = 1;

    public bool movementLock;

    public Vector3 runPosition;
    public Vector3 walkPosition;

    public Vector3 runRotation;
    public Vector3 walkRotation;

    public Vector3 pistolRunPosition;
    public Vector3 pistolWalkPosition;

    public Vector3 pistolRunRotation;
    public Vector3 pistolWalkRotation;

    Vector3 startPos;
    Vector3 startRot;

    public float timeToUnreadyPistol;
    public float timeToReadyPistol;

    public float timeToUnreadyRifle;
    public float timeToReadyRifle;

    float timeToUnreadyWeapon;
    float timeToReadyWeapon;

    float timeToLerp;
    float maxLerpTime;

    bool sprintingAnim;

    private void Awake()
    {
        infoBoxText = GameObject.Find("Info Box").GetComponent<InfoBoxText>();
        inventory.UpdateWeaponType();

        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        weaponManager = GetComponent<WeaponManager>();

        InitPlayerActions();

        GetRebinds();

        Physics.gravity = new Vector3(0, -22.0f, 0);

        timeToLerp = maxLerpTime + 1;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;

        inventory.ToggleVisibleSlots(false);
    }

    private void OnDisable()
    {
        DeinitPlayerActions();
    }
    
    public void ToggleMenu()
    {
        if (inventory.weaponModCanvas.activeSelf) CloseMenu();
        else OpenMenu();
    }

    public void OpenMenu()
    {
        enableLook = false;
        weaponManager.guns[weaponManager.activeGun].canShoot = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        inventory.ToggleWeaponModCanvas(true);
        inventory.ToggleVisibleSlots(true);
        HUD.SetActive(false);
        inventory.UpdateWeaponType();
    }

    public void CloseMenu()
    {
        enableLook = true;
        weaponManager.guns[weaponManager.activeGun].canShoot = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        inventory.ToggleVisibleSlots(false);
        inventory.ToggleWeaponModCanvas(false);
        inventory.UpdateWeaponStats();
        weaponManager.UpdateWeaponStats();
        infoBoxText.ToggleSelf(false);
        HUD.SetActive(true);
    }

    void Update()
    {
        #region Sprint Stuff
        speed = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).magnitude;

        if (enableLook) DoLook(mouseSensitivity);

        if (speed >= 5.5 && IsGrounded())
        {
            audioSource.mute = false;
        }
        else audioSource.mute = true;


        if (!jumping && playerInput.actions["Jump"].inProgress && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce * _rb.mass, ForceMode.Impulse);
            StartCoroutine(WaitForJump());
        }

        if (sprinting && speed > 7 && IsGrounded())
        {
            player.TakeStamina(staminaToRun * Time.smoothDeltaTime);
            ResetSprintVariables(false);
            if (!sprintingAnim)
            {
                UpdateAnimation(true);
            }
        }
        if (sprinting && speed > 7 && !IsGrounded())
        {
            player.TakeStamina(staminaToRun / 1.5f * Time.smoothDeltaTime);
            ResetSprintVariables(true);
            if (!sprintingAnim)
            {
                UpdateAnimation(true);
            }
        }
        else if (sprinting && speed < 1 && resetWHileSprinting)
        {
            player.timeSinceUsedStamina = 0;
            resetWHileSprinting = false;
            UpdateAnimation(false);
        }

        if (sprinting && (!(player.stamina > 0) || !canSprint))
        {
            sprinting = false;
            canSprint = false;
            UpdateAnimation(false);
        }

        if (!sprinting && canSprint && playerInput.actions["Sprint"].inProgress && IsGrounded())
        {
            sprinting = true;
            ResetSprintVariables(false);
            UpdateAnimation(true);
        }

        if (sprinting && canSprint)
        {
            Camera.main.fieldOfView = quickFOV;
        }
        else
        {
            Camera.main.fieldOfView = 60.0f;
        }

        if (sprinting)
        {
            weaponManager.guns[weaponManager.activeGun].canShoot = false;
            weaponManager.guns[weaponManager.activeGun].canSwap = false;
        }
        else
        {
            weaponManager.guns[weaponManager.activeGun].canSwap = true;
        }

        if (sprinting && !weaponManager.guns[weaponManager.activeGun].canReload)
        {
            sprinting = false;
            canSprint = false;
        }

        Sprint();
        #endregion



        if (timeToLerp > maxLerpTime && weaponManager.guns[weaponManager.activeGun].canReload && !sprinting)
        {
            weaponManager.guns[weaponManager.activeGun].canShoot = true;
            weaponManager.guns[weaponManager.activeGun].canSwap = true;
        }
        else
        {
            DoRunAnimation(sprinting);
            timeToLerp += Time.deltaTime;
            weaponManager.guns[weaponManager.activeGun].canShoot = false;
            weaponManager.guns[weaponManager.activeGun].canSwap = false;
        }

        if (weaponManager.activeGun == 1 || weaponManager.activeGun == 2)
        {
            timeToReadyWeapon = timeToReadyRifle;
            timeToUnreadyWeapon = timeToReadyRifle;
        }
        else
        {
            timeToReadyWeapon = timeToReadyPistol;
            timeToUnreadyWeapon = timeToReadyPistol;
        }
    }

    void DoRunAnimation(bool sprinting)
    {
        if ((weaponManager.activeGun == 1 || weaponManager.activeGun == 2) && sprinting && speed > 7)
        {
            weaponManager.guns[weaponManager.activeGun].transform.localPosition = Vector3.Lerp(startPos, runPosition, GetLerpTime());
            weaponManager.guns[weaponManager.activeGun].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, runRotation, GetLerpTime()));
            sprintingAnim = true;
        }
        else if (weaponManager.activeGun == 0 && sprinting && speed > 7)
        {
            weaponManager.guns[weaponManager.activeGun].transform.localPosition = Vector3.Lerp(startPos, pistolRunPosition, GetLerpTime());
            weaponManager.guns[weaponManager.activeGun].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, pistolRunRotation, GetLerpTime()));
            sprintingAnim = true;
        }
        else if (weaponManager.activeGun == 1 || weaponManager.activeGun == 2)
        {
            weaponManager.guns[weaponManager.activeGun].transform.localPosition = Vector3.Lerp(startPos, walkPosition, GetLerpTime());
            weaponManager.guns[weaponManager.activeGun].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, walkRotation, GetLerpTime()));
            sprintingAnim = false;
        }
        else if (weaponManager.activeGun == 0)
        {
            weaponManager.guns[weaponManager.activeGun].transform.localPosition = Vector3.Lerp(startPos, pistolWalkPosition, GetLerpTime());
            weaponManager.guns[weaponManager.activeGun].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, pistolWalkRotation, GetLerpTime()));
            sprintingAnim = false;
        }
    }

    float GetLerpTime()
    {
        return timeToLerp / maxLerpTime;
    }

    void DoLook(float sensitivity)
    {
        Vector2 lookDir = playerInput.actions["Look"].ReadValue<Vector2>();

        if (lookDir == Vector2.zero)
        {
            return;
        }

        lookDir = lookDir * (sensitivity / 100);

        yRotation += lookDir.x;
        xRotation -= lookDir.y;
        xRotation = Mathf.Clamp(xRotation, -85, 85);

        cameraParent.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }

    void DoWalk()
    {
        Vector2 moveDir = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 moveX = moveDir.y * new Vector3(cameraParent.forward.x, 0, cameraParent.forward.z).normalized * moveSpeed;
        Vector3 moveZ = moveDir.x * new Vector3(cameraParent.right.x, 0, cameraParent.right.z).normalized * moveSpeed;

        if (moveDir.y > 0 && player.stamina > 0 && weaponManager.guns[weaponManager.activeGun].canReload)
        {
            canSprint = true;
        }
        else
        {
            canSprint = false;
        }

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
        if (!movementLock)
        {
            DoWalk();
        }
        
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
            audioSource.pitch = 1.0f;
            resetSprint = true;
        }
        else if (resetSprint)
        {
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
        playerInput.actions["Sprint"].started += ctx => DoSprintStuff(true);
        playerInput.actions["Sprint"].canceled += ctx => DoSprintStuff(false);
        playerInput.actions["SwapWeapon"].performed += ctx => weaponManager.SwapWeapon();
        playerInput.actions["Pause"].performed += ctx => pauseMenu.RunPause();
        playerInput.actions["Melee"].performed += ctx => player.QuickMelee();
        playerInput.actions["Flashlight"].performed += ctx => ToggleFlashlight();
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
        playerInput.actions["Sprint"].performed -= ctx => DoSprintStuff(true);
        playerInput.actions["Sprint"].canceled -= ctx => DoSprintStuff(false);
        playerInput.actions["SwapWeapon"].performed += ctx => weaponManager.SwapWeapon();
        playerInput.actions["Pause"].performed -= ctx => pauseMenu.RunPause();
        playerInput.actions["Melee"].performed -= ctx => player.QuickMelee();
        playerInput.actions["Flashlight"].performed -= ctx => ToggleFlashlight();
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
        else if (collision.gameObject.layer == 6)
        {
            onStairs = false;
        }
        else 
        {
            onStairs = false;
        }
    }

    void ResetSprintVariables(bool keyPress)
    {
        player.timeSinceUsedStamina = 0;
        resetWHileSprinting = true;
        if (keyPress)
        {
            if (IsGrounded())
            {
                sprinting = true;
            }
        }

        else
        {
            sprinting = true;
        }
    }

    void DoSprintStuff(bool keyPress)
    {
        if (keyPress)
        {
            ResetSprintVariables(true);
            UpdateAnimation(true);
        }
        else
        {
            sprinting = false;
            UpdateAnimation(false);
        }
    }

    void UpdateAnimation(bool sprinting)
    {
        var rotation = weaponManager.guns[weaponManager.activeGun].transform.localRotation;

        if (weaponManager.activeGun == 0)
        {
            startRot = new Vector3(rotation.eulerAngles.x - 360, rotation.eulerAngles.y, rotation.eulerAngles.z);
        }
        else
        {
            startRot = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y - 360, rotation.eulerAngles.z);
        }
        
        startPos = weaponManager.guns[weaponManager.activeGun].transform.localPosition;

        maxLerpTime = sprinting ? timeToUnreadyWeapon : timeToReadyWeapon;
        timeToLerp = 0;
    }

    void ToggleFlashlight()
    {
        if (flashlight.activeSelf)
        {
            flashlight.SetActive(false);
        }
        else
        {
            flashlight.SetActive(true);
        }
    }
}
