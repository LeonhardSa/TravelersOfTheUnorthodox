using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamScript : MonoBehaviour
{

    public GameObject target;

    public float cameraHeight = 50f;

    public LayerMask rayMinimapMask;

    // Start is called before the first frame update
    void Start()
    {

        if (target == null) target = FindObjectOfType<CharacterControls>().gameObject;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        bool hitCeiling = Physics.Raycast(target.transform.position, target.transform.up, out hit, cameraHeight, rayMinimapMask);
        transform.position = target.transform.position + target.transform.up * (hitCeiling ? (hit.distance - 1f) : cameraHeight);
        
        Vector3 targetDirection = target.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, float.MaxValue, float.MaxValue);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}