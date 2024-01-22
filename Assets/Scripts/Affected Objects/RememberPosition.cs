using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberPosition : MonoBehaviour
{

    private Vector3 initialPos;
    private Quaternion initialRot;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    
    public void ResetPos()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = initialPos;
        transform.rotation = initialRot;
        StartCoroutine(waitForaBit());
    }

    private IEnumerator waitForaBit()
    {
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = false;
    }

}
