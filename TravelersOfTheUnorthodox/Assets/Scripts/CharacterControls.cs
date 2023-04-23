using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{

    public float walkSpeed = 7.5f;
    public float jumpSpeed = 7f;
    public float gravity = 20.0f;

    Vector3 moveDirection = Vector3.zero;
    public bool canMove = true;

    public LayerMask ground;

    public float lookLimit = 45f;

    float rotation = 0f;

    float speedForward;
    float speedSideways;
    float speedUpward = 0f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        speedForward = walkSpeed * Input.GetAxis("Vertical");
        speedSideways = walkSpeed * Input.GetAxis("Horizontal");

        if (Input.GetButton("Jump") && canMove && IsGrounded())
        {
            speedUpward = jumpSpeed;
        }
        else
        {
            if (!IsGrounded())
            {
                speedUpward -= gravity * Time.fixedDeltaTime;
            }
            else
            {
                speedUpward = 0f;
            }
        }

        moveDirection = (forward * speedForward) + (right * speedSideways) + (speedUpward * transform.up);

        transform.position += Vector3.Scale(moveDirection, transform.localScale) * Time.fixedDeltaTime;
        //rb.velocity = Vector3.Scale(moveDirection, transform.localScale);

        //Player and Camera Rotation
        if (canMove)
        {
            rotation += -Input.GetAxis("Mouse Y") * 1f;
            rotation = Mathf.Clamp(rotation, -lookLimit, lookLimit);
            GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(rotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * 1f, 0);
        }

        // Just don't, please
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position - transform.up * transform.localScale.y, 0.1f, ground);
    }
}
