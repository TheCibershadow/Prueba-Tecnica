using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Inputs
    private PlayerInput playerInput;
    private CharacterController characterController;

    //salto y movimiento
    public float speed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float jumpHeight = 1.0f;
    private Vector3 playerVelocity;
    


    //Apuntar
    public Camera cam;
    private float camRotation = 0f;
    public float camSensitivityX = 30f;
    public float camSensitivyY = 30f;
    [SerializeField] private GameObject playerWeapon;
    

    //Disparar
    public WeaponBehaviour weaponHolding;
    public bool weaponOnPlayer = false;
    [SerializeField] private GameObject firePoint;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }


    //habilitar Input System
    private void OnEnable()
    {
        playerInput.Input.Enable();
    }

    private void OnDisable()
    {
        playerInput.Input.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }



    private void Update()
    {
        HandleRotation();     
    }

    //manejo del movimiento del personaje
    private void HandleMovement()
    {
        //variables de salto
        groundedPlayer = characterController.isGrounded;
        bool isJumpKeyHeld = playerInput.Input.Jump.ReadValue<float>() > 0.1f;

        //variables de movimiento
        Vector2 movementInput = playerInput.Input.Movement.ReadValue<Vector2>();
        Vector3 movementDirection = Vector3.zero;
        movementDirection.x = movementInput.x;
        movementDirection.z = movementInput.y;

        //colocar movimiento y velocidad al personaje
        characterController.Move(transform.TransformDirection(movementDirection) * speed * Time.deltaTime);


        // parar velocidad de salto
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // habilitar salto 
        if (isJumpKeyHeld && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (isJumpKeyHeld)
        {
            Debug.Log("Oprimiendo salto");
        }

        //saltar / generar gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }


    //manejo del apuntado del personaje 
    private void HandleRotation()
    {
        Vector2 rotationInput = playerInput.Input.Point.ReadValue<Vector2>();
        float pointX = rotationInput.x;
        float pointY = rotationInput.y;

        //calcular rotación de camara
        camRotation -= (pointY * Time.deltaTime) * camSensitivyY;
        camRotation = Mathf.Clamp(camRotation, -80f, 80f);

        //aplicar valores a camara
        cam.transform.localRotation = Quaternion.Euler(camRotation, 0, 0);

        //rotar jugador 
        transform.Rotate(Vector3.up * (pointX * Time.deltaTime) * camSensitivityX);

    }
}
