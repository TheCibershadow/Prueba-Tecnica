using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float timeToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", timeToDestroy);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    
}
