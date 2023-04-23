using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveMinimapScript : MonoBehaviour
{
    public GameObject target;

    public float cameraHeight = 50f;

    public LayerMask rayMinimapMask;

    public Vector3 maxCamDistance;

    private float distanceMultiplier = 0f;
    private float currentDistance;

    // Start is called before the first frame update
    void Start()
    {

        if (target == null) target = FindObjectOfType<CharacterControls>().gameObject;

        maxCamDistance = new Vector3(-cameraHeight / Mathf.Sqrt(2f), cameraHeight, -cameraHeight / Mathf.Sqrt(2f));
        currentDistance = cameraHeight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        bool hitCeiling = Physics.Raycast(target.transform.position, maxCamDistance, out hit, maxCamDistance.magnitude, rayMinimapMask);

        distanceMultiplier = (hitCeiling ? (hit.distance + 10f) : maxCamDistance.magnitude) / maxCamDistance.magnitude;
        currentDistance = Mathf.Lerp(currentDistance, distanceMultiplier, 0.2f);

        transform.position = target.transform.position + maxCamDistance * currentDistance;
    }
}