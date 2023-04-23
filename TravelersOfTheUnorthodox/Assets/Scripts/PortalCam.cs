using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{

    private Transform playerCam;

    [HideInInspector]
    public Transform portal;
    [HideInInspector]
    public Transform otherPortal;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 portalRotationDiff = portal.eulerAngles - otherPortal.eulerAngles;


        Vector3 portalScalingDiff = new Vector3(portal.localScale.x / otherPortal.localScale.x,
                                                portal.localScale.y / otherPortal.localScale.y,
                                                portal.localScale.z / otherPortal.localScale.z);

        Vector3 positionOffset = playerCam.position - otherPortal.position;

        positionOffset.x *= portalScalingDiff.x;
        positionOffset.y *= portalScalingDiff.y;
        positionOffset.z *= portalScalingDiff.z;

        positionOffset = Quaternion.Euler(portalRotationDiff) * positionOffset;
        positionOffset = Quaternion.AngleAxis(180f, otherPortal.up) * positionOffset;

        transform.position = portal.position + positionOffset;

        transform.eulerAngles = portalRotationDiff - Vector3.up * 180;
        transform.Rotate(playerCam.eulerAngles);

        Vector3 camToPortal = portal.position - transform.position;
        GetComponent<Camera>().nearClipPlane = Vector3.Project(camToPortal, transform.forward * camToPortal.magnitude).magnitude - 1f - portal.Find("portalgate").GetComponent<MeshCollider>().bounds.extents.z - portal.Find("portalgate").GetComponent<MeshCollider>().bounds.extents.magnitude * (1 - Mathf.Abs(Vector3.Dot(portal.forward, transform.forward)));
        GetComponent<Camera>().nearClipPlane = Mathf.Clamp(GetComponent<Camera>().nearClipPlane, 0.01f, float.MaxValue);


        /*
        //Set new Position
        Vector3 positionOffset = playerCam.position - otherPortal.position;

        //Scale appropriately
        Vector3 portalScalingDiff = new Vector3(portal.localScale.x / otherPortal.localScale.x,
                                                portal.localScale.y / otherPortal.localScale.y,
                                                portal.localScale.z / otherPortal.localScale.z);

        positionOffset.x *= portalScalingDiff.x;
        positionOffset.y *= portalScalingDiff.y;
        positionOffset.z *= portalScalingDiff.z;

        //Rotate Offset
        Vector3 portalRotationDiff = portal.eulerAngles - otherPortal.eulerAngles;

        positionOffset = Quaternion.Euler(portalRotationDiff) * positionOffset;
        positionOffset = Quaternion.AngleAxis(180f, portal.up) * positionOffset;

        transform.position = portal.position + positionOffset;

        //Set New Rotation

        Vector3 portalPlayerRotationDiff = playerCam.eulerAngles - otherPortal.eulerAngles;

        transform.rotation = portal.rotation;
        transform.Rotate(portalPlayerRotationDiff);
        transform.Rotate(Vector3.up, 180);

        //Calculate and Set new Clipping Plane
        Vector3 camToPortal = portal.position - transform.position;
        GetComponent<Camera>().nearClipPlane = Vector3.Project(camToPortal, transform.forward * camToPortal.magnitude).magnitude - 1f - portal.Find("portalgate").GetComponent<MeshCollider>().bounds.extents.z - portal.Find("portalgate").GetComponent<MeshCollider>().bounds.extents.magnitude * (1 - Mathf.Abs(Vector3.Dot(portal.forward, transform.forward)));
        GetComponent<Camera>().nearClipPlane = Mathf.Clamp(GetComponent<Camera>().nearClipPlane, 0.01f, float.MaxValue);
        */
    }
}