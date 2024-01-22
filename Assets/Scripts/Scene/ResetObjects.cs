using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjects : MonoBehaviour
{
    //Input
    private PlayerInput playerInput;

    public Camera cam;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private Transform affectedObjects;
    [SerializeField] private LayerMask whatIsPlayer;
    private bool playerInRange = false;
    List<GameObject> objects;

    //interacción 
    public float interactRange;

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

    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, interactRange, whatIsPlayer);
        if (!playerInRange) visualCue.SetActive(false);
        if (playerInRange)
        {
            visualCue.SetActive(true);
            visualCue.transform.LookAt(visualCue.transform.position + cam.transform.forward);
            ResetObj();
        }
    }


    private void ResetObj()
    {
        bool isInteractKeyHeld = playerInput.Input.Interact.ReadValue<float>() > 0.1f;

        if (isInteractKeyHeld)
        {
            for (int i = 0; i < affectedObjects.transform.childCount; i++)
            {
                RememberPosition resetPos = affectedObjects.GetChild(i).GetComponent<RememberPosition>();
                resetPos.ResetPos();
                
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
