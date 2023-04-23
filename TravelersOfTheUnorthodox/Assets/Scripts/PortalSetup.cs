using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSetup : MonoBehaviour
{

    public GameObject portalConnection;
    GameObject portalTrig;
    Camera cam;
    Material camMat;
    Camera minimapCam;
    Material minimapMat;

    // Start is called before the first frame update
    void Awake()
    {

        portalTrig = transform.Find("PortalTrigger").gameObject;
        portalTrig.GetComponent<PortalPortingNew>().receiver = portalConnection.transform.Find("PortalTrigger").transform;

        cam = transform.Find("Camera").GetComponent<Camera>();
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }

        cam.GetComponent<PortalCam>().portal = portalConnection.transform;
        cam.GetComponent<PortalCam>().otherPortal = this.transform;

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camMat = new Material(Shader.Find("Unlit/ScreenCutoutShader"))
        {
            name = gameObject.name + "FrameMat",
            mainTexture = cam.targetTexture
        };
        this.transform.Find("RenderFrame").GetComponent<Renderer>().material = camMat;

        minimapCam = transform.Find("CameraMinimap").GetComponent<Camera>();
        if (minimapCam.targetTexture != null)
        {
            minimapCam.targetTexture.Release();
        }

        minimapCam.GetComponent<PortalMinimapCam>().portal = portalConnection.transform;
        minimapCam.GetComponent<PortalMinimapCam>().otherPortal = this.transform;

        minimapCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        minimapMat = new Material(Shader.Find("Unlit/ScreenCutoutShader"))
        {
            name = gameObject.name + "MinimapMat",
            mainTexture = minimapCam.targetTexture
        };
        this.transform.Find("StencilGeometry").GetComponent<Renderer>().material = minimapMat;
    }
}