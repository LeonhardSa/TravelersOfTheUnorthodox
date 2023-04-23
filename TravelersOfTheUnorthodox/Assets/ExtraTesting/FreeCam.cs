using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public bool freeCam = false;
    bool tappedOut = false;

    float rotationY = 0f;
    float rotationX = 0f;

    Transform parentObj;

    private void Start()
    {
        parentObj = transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            freeCam = !freeCam;

            if (transform.parent != null) transform.parent = null;
            else
            {
                transform.parent = parentObj;

                transform.localPosition = new Vector3(0f, 0.6f, -0.15f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) tappedOut = true;
        else if (Input.GetKeyDown(KeyCode.Mouse0)) tappedOut = false;

        if (freeCam == true && !tappedOut)
        {
            rotationY += -Input.GetAxis("Mouse Y") * 5f;
            rotationX += Input.GetAxis("Mouse X") * 5f;
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * Time.fixedDeltaTime * 10f;
            } 
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.forward * Time.fixedDeltaTime * 10f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * Time.fixedDeltaTime * 10f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * Time.fixedDeltaTime * 10f;
            }
        }
    }
}
