using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortingObjectManager : MonoBehaviour
{

    public Dictionary<Transform, PortingObject> portingObjects = new Dictionary<Transform, PortingObject>();

    public HashSet<Transform> objsInPortal = new HashSet<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (portingObjects.Count != 0)
        {

            foreach (KeyValuePair<Transform, PortingObject> portingObj in portingObjects)
            {

                if (portingObj.Value.objClone.tag.Contains("Porting"))
                {
                    Debug.Log("OG-tag: '" + portingObj.Value.objOriginal.tag + "'.. CLONE-tag: '" + portingObj.Value.objClone.tag + "'");

                    Vector3 portalRotationDiff = portingObj.Value.portalReceiver.eulerAngles - portingObj.Value.portalTransmitter.eulerAngles;

                    Vector3 objToPortalPositionDiff = portingObj.Value.objClone.position - portingObj.Value.portalReceiver.position;

                    Vector3 portalScalingDiff = new Vector3(portingObj.Value.portalTransmitter.lossyScale.x / portingObj.Value.portalReceiver.lossyScale.x,
                                                            portingObj.Value.portalTransmitter.lossyScale.y / portingObj.Value.portalReceiver.lossyScale.y,
                                                            portingObj.Value.portalTransmitter.lossyScale.z / portingObj.Value.portalReceiver.lossyScale.z);

                    objToPortalPositionDiff.x *= portalScalingDiff.x;
                    objToPortalPositionDiff.y *= portalScalingDiff.y;
                    objToPortalPositionDiff.z *= portalScalingDiff.z;

                    objToPortalPositionDiff = Quaternion.Euler(portalRotationDiff) * objToPortalPositionDiff;
                    objToPortalPositionDiff = Quaternion.AngleAxis(180f, Vector3.up) * objToPortalPositionDiff;

                    portingObj.Value.objOriginal.eulerAngles = portalRotationDiff - Vector3.up * 180;
                    portingObj.Value.objOriginal.Rotate(portingObj.Value.objClone.eulerAngles);

                    portingObj.Value.objOriginal.position = portingObj.Value.portalTransmitter.position + objToPortalPositionDiff;

                    portingObj.Value.objOriginal.localScale = new Vector3(portingObj.Value.objClone.lossyScale.x * portalScalingDiff.x,
                                                                          portingObj.Value.objClone.lossyScale.y * portalScalingDiff.y,
                                                                          portingObj.Value.objClone.lossyScale.z * portalScalingDiff.z);
                }
                else
                {
                    Vector3 portalRotationDiff = portingObj.Value.portalTransmitter.eulerAngles - portingObj.Value.portalReceiver.eulerAngles;

                    Vector3 objToPortalPositionDiff = portingObj.Value.objOriginal.position - portingObj.Value.portalTransmitter.position;

                    Vector3 portalScalingDiff = new Vector3(portingObj.Value.portalReceiver.lossyScale.x / portingObj.Value.portalTransmitter.lossyScale.x,
                                                            portingObj.Value.portalReceiver.lossyScale.y / portingObj.Value.portalTransmitter.lossyScale.y,
                                                            portingObj.Value.portalReceiver.lossyScale.z / portingObj.Value.portalTransmitter.lossyScale.z);

                    objToPortalPositionDiff.x *= portalScalingDiff.x;
                    objToPortalPositionDiff.y *= portalScalingDiff.y;
                    objToPortalPositionDiff.z *= portalScalingDiff.z;

                    objToPortalPositionDiff = Quaternion.Euler(portalRotationDiff) * objToPortalPositionDiff;
                    objToPortalPositionDiff = Quaternion.AngleAxis(180f, Vector3.up) * objToPortalPositionDiff;

                    portingObj.Value.objClone.eulerAngles = portalRotationDiff - Vector3.up * 180;
                    portingObj.Value.objClone.Rotate(portingObj.Value.objOriginal.eulerAngles);

                    portingObj.Value.objClone.position = portingObj.Value.portalReceiver.position + objToPortalPositionDiff;

                    portingObj.Value.objClone.localScale = new Vector3(portingObj.Value.objOriginal.lossyScale.x * portalScalingDiff.x,
                                                                       portingObj.Value.objOriginal.lossyScale.y * portalScalingDiff.y,
                                                                       portingObj.Value.objOriginal.lossyScale.z * portalScalingDiff.z);
                }
            }
        }
    }
}