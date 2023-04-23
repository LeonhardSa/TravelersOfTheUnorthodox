using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    PortingObjectManager pom;

    GameObject pickUp;
    public GameObject portingParent;
    public float Range = 5f;
    public float Force = 50f;

    private GameObject portingObj;

    private bool hitRenderFrame = false;

    private void Start()
    {
        pom = GetComponent<PortingObjectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(portingObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Range * transform.parent.lossyScale.y, LayerMask.GetMask("Ground", "Walls", "Barrier", "PickUps")))
                {
                    Debug.Log(hit.transform.name);
                    if(hit.transform.gameObject.layer == 12 && hit.transform.tag.Contains("Portable"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                DropDownObject();
            }
        }        

        if (portingObj != null)
        {
            if (Vector3.Distance(portingObj.transform.position, portingParent.transform.position) > 0.1f)
            {
                Vector3 moveDir = portingParent.transform.position - portingObj.transform.position;
                portingObj.GetComponent<Rigidbody>().AddForce(moveDir * 40, ForceMode.Force);
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Range * transform.parent.lossyScale.y, LayerMask.GetMask("PickUps", "RenderFrame")))
            {
                if(hit.transform.gameObject.layer == 13)
                {
                    hitRenderFrame = true;

                    if (!pom.objsInPortal.Contains(portingObj.transform))
                    {
                        Debug.Log("Portable Object " + portingObj.name + " trough portal: " + this.name);

                        //create mimic object
                        GameObject clone = Instantiate(portingObj.gameObject);
                        clone.name = portingObj.name + "Clone";
                        clone.tag = "PortableClone/" + portingObj.tag.Split('/')[1];

                        Transform[] pair = new Transform[2];
                        pair[0] = clone.transform;
                        pair[1] = portingObj.transform;

                        pom.objsInPortal.Add(clone.transform);
                        pom.objsInPortal.Add(portingObj.transform);

                        pom.portingObjects.Add(portingObj.transform, new PortingObject(portingObj.transform, clone.transform, hit.transform, hit.transform.GetComponent<PortalPortingNew>().receiver.transform));
                    }
                }
                else if(!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Range * transform.parent.lossyScale.y, LayerMask.GetMask("RenderFrame")) && hitRenderFrame)
                {
                    hitRenderFrame = false;

                    foreach (KeyValuePair<Transform, PortingObject> portObj in pom.portingObjects)
                    {
                        if (portObj.Value.objOriginal == portingObj.transform)
                        {
                            Debug.Log("Carries Original");

                            pom.objsInPortal.Remove(portObj.Value.objOriginal);
                            pom.objsInPortal.Remove(portObj.Value.objClone);

                            pom.portingObjects.Remove(portObj.Value.objOriginal);

                            GameObject.Destroy(portObj.Value.objClone.gameObject, 0f);

                            break;
                        }
                    }
                }
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            Rigidbody pickUpRB = pickUpObj.GetComponent<Rigidbody>();
            pickUpRB.isKinematic = false;
            if(pickUpObj.GetComponent<PickUpGravity>()) pickUpObj.GetComponent<PickUpGravity>().canMove = false;
            else pickUpRB.useGravity = false;
            pickUpRB.drag = 10;

            pickUpRB.transform.tag = "Porting/" + pickUpRB.transform.tag.Split('/')[1];

            portingParent.transform.localPosition = Vector3.forward * (pickUpRB.GetComponent<MeshRenderer>().bounds.extents.magnitude + 1.5f) / transform.parent.lossyScale.y;

            pickUpRB.transform.parent = portingParent.transform;
            portingObj = pickUpObj;
        }
    }

    void DropDownObject()
    {
        Rigidbody pickUpRB = portingObj.GetComponent<Rigidbody>();
        if (portingObj.GetComponent<PickUpGravity>()) portingObj.GetComponent<PickUpGravity>().canMove = true;
        else pickUpRB.useGravity = true;
        pickUpRB.drag = 0;
        pickUpRB.transform.tag = "Portable/" + pickUpRB.transform.tag.Split('/')[1];

        portingObj.transform.parent = null;
        portingObj = null;

        portingParent.transform.localPosition = Vector3.forward * 2f;
    }
}
