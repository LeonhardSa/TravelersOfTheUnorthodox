using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeshBoundSetting
{
    public Vector3 boundDirection;
    public Vector3 boundMinScaling;
    public Vector3 boundMaxScaling;
    public Vector3 playerMinPosFromCenter;
    public Vector3 playerMaxPositionFromCenter;

    public MeshBoundSetting(Vector3 boundDirection, Vector3 boundMinScaling, Vector3 boundMaxScaling, Vector3 playerMinPosFromCenter, Vector3 playerMaxPositionFromCenter)
    {
        this.boundDirection = boundDirection;
        this.boundMinScaling = boundMinScaling;
        this.boundMaxScaling = boundMaxScaling;
        this.playerMinPosFromCenter = playerMinPosFromCenter;
        this.playerMinPosFromCenter = playerMinPosFromCenter;
        this.playerMaxPositionFromCenter = playerMaxPositionFromCenter;
    }
}
