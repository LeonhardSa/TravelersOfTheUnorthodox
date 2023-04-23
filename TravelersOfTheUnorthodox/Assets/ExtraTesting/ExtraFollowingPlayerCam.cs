using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFollowingPlayerCam : MonoBehaviour
{

    private Transform playerCam;
    public float zOffset;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main.transform;
        playerCam = Camera.main.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerCam.position + Vector3.forward * zOffset;

        transform.rotation = playerCam.rotation;
    }
}
