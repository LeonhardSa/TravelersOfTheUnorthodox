using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFrameSetup : MonoBehaviour
{

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Dot(transform.forward, (playerTransform.position - transform.position).normalized) > 0)
        {
            GetComponent<Renderer>().material.SetFloat("_Cull", 1);
        }
        else
        {
            GetComponent<Renderer>().material.SetFloat("_Cull", 2);
        }
    }
}
