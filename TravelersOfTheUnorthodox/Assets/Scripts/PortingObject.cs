using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortingObject
{
    public Transform objOriginal;
    public Transform objClone;
    public Transform portalTransmitter;
    public Transform portalReceiver;

    public PortingObject(Transform objOriginal, Transform objClone, Transform portalTransmitter, Transform portalReceiver)
    {
        this.objOriginal = objOriginal;
        this.objClone = objClone;
        this.portalTransmitter = portalTransmitter;
        this.portalReceiver = portalReceiver;
    }
}
