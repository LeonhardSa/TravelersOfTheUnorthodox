using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGravity : MonoBehaviour
{

    private float gravity = 10.0f;
    private float fallingSpeed = 5f;

    public LayerMask ground;

    public Vector3 gravityDirection;

    Rigidbody rb;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gravityDirection.magnitude != 1) gravityDirection.Normalize();

        if (canMove && gravityDirection != Vector3.zero)
        {
            if (!IsGrounded())
            {
                fallingSpeed += gravity * Time.fixedDeltaTime;
            }
            else
            {
                fallingSpeed = 0;
            }

            rb.velocity = gravityDirection * (fallingSpeed != 0 ? fallingSpeed : 5f);
        }
        else
        {
            fallingSpeed = 0;
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, gravityDirection.normalized, out hit, Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z), ground);
    }
}
