using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMinimapCam : MonoBehaviour
{

    Transform minimapCam;

    [HideInInspector]
    public Transform portal;
    [HideInInspector]
    public Transform otherPortal;

    // Start is called before the first frame update
    void Start()
    {
        minimapCam = GameObject.Find("MinimapCam").transform;
        transform.rotation = minimapCam.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 MinimapCamPortalOffset = minimapCam.position - otherPortal.position;
        transform.position = portal.position + MinimapCamPortalOffset;
    }
}