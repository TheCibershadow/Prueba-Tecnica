using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    //Input
    private PlayerInput playerInput;

    //Interaccion visual
    public Camera cam;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private LayerMask whatIsPlayer;
    private bool playerInRange = false;

    //interacción 
    public float interactRange;
    private bool weaponOnPlayer = false;

    //Armas
    [SerializeField] private GameObject holdedWeapon;
    [SerializeField] private GameObject variantWeaponOne, variantWeaponTwo;
    public InputManager inputManager;
    public Sprite crosshair;
    public Image crosshairUI;

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

    private void Start()
    {
        visualCue.SetActive(false);
    }

    private void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, interactRange, whatIsPlayer);
        if (!playerInRange) visualCue.SetActive(false);
        if (playerInRange)
        {
            visualCue.SetActive(true);
            visualCue.transform.LookAt(visualCue.transform.position + cam.transform.forward);
            PickWeaponUp();
        }
    }


    private void PickWeaponUp()
    {
     
        bool isInteractKeyHeld = playerInput.Input.Interact.ReadValue<float>() > 0.1f;

        if (isInteractKeyHeld && !weaponOnPlayer)
        {
            if (holdedWeapon.GetComponent<WeaponBehaviour>() != null)
            {
                WeaponBehaviour weaponBh = holdedWeapon.GetComponent<WeaponBehaviour>();
                holdedWeapon.SetActive(true);
                variantWeaponOne.SetActive(false);
                variantWeaponTwo.SetActive(false);
                crosshairUI.rectTransform.sizeDelta = new Vector2(100, 100);
                crosshairUI.sprite = crosshair;
                inputManager.weaponHolding = weaponBh;
                inputManager.weaponOnPlayer = true;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
