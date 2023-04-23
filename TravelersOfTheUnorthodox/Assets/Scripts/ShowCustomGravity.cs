using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCustomGravity : MonoBehaviour
{
    PickUpGravity pg;

    private void Start()
    {
        pg = transform.parent.GetComponent<PickUpGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
        transform.LookAt(transform.position + pg.gravityDirection);
        transform.localScale = transform.parent.transform.localScale;
    }
}