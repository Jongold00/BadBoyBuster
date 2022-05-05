using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float poopCD = 3f;

    [SerializeField]
    private Transform poopSpawn;
    [SerializeField]
    private GameObject poopPrefab;
    [SerializeField]
    private InGameUIHandler uiHandler;
    [SerializeField]
    private CinemachineBrain m_Brain;

    [SerializeField]
    private GameObject fartSoundPrefab;

    [SerializeField]
    private GameObject whineSoundPrefab;

    private CharacterController controller;
    private PlayerControls playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private Animator animator;

    private bool canPoop = true;
    private bool IsPooping = false;

    private InputAction moveAction;

    private void Awake()
    {
        playerInput = InputManager.inputActions;

    }

    private void OnEnable()
    {        
        moveAction = playerInput.Player.Movement;
        moveAction.Enable();

        playerInput.Player.Jump.performed += DoJump;
        playerInput.Player.Jump.Enable();

        playerInput.Player.Poop.performed += DoPoop;
        playerInput.Player.Poop.Enable();

        playerInput.Player.Menu.performed += OpenMenu;
        playerInput.Player.Menu.Enable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = Camera.main.transform;
        uiHandler = FindObjectOfType<InGameUIHandler>();
    }

    void Update()
    {
        //Check if the player is on the ground
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Movement of the player
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        move.Normalize();
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction
        if (move == Vector3.zero)
        {
            if (!IsPooping)
            {
                float targetAngle = cameraTransform.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);                
            }
        }

        if (Time.deltaTime == 0f)
        {
            m_Brain.enabled = false;
        }
        else
        {
            m_Brain.enabled = true;
        }
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (groundedPlayer)
        {
            animator.SetTrigger("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void DoPoop(InputAction.CallbackContext obj)
    {
        if (canPoop)
        {
            IsPooping = true;
            playerInput.Disable();
            animator.SetTrigger("Poop");
            Instantiate(poopPrefab, poopSpawn.position, new Quaternion(0, 0, 0, 0));
            canPoop = false;
            Instantiate(fartSoundPrefab);
            Invoke("PoopCooldown", poopCD);
            Invoke("Unfreeze", 3.5f);
        }
    }

    private void OpenMenu(InputAction.CallbackContext obj)
    {
        uiHandler.OnGamePause();
        Cursor.lockState = CursorLockMode.None;
    }

    void PoopCooldown()
    {
        canPoop = true;
    }

    void Unfreeze()
    {
        playerInput.Enable();
        IsPooping = false;
    }

    public void HitByCar()
    {
        Instantiate(whineSoundPrefab);
        playerInput.Disable();
        animator.SetTrigger("Car");
        Invoke("Unfreeze", 3.5f);
    }
}
