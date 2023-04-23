using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionMatrixSetup : MonoBehaviour
{

    //Cam Projection Matrix
    public float left = -0.2F;
    public float right = 0.2F;
    public float top = 0.2F;
    public float bottom = -0.2F;

    public float x = 0f;
    public float y = 0f;
    public float a = 0f;
    public float b = 0f;
    public float c = 0f;
    public float d = 0f;
    public float e = 0f;

    Camera cam;
    Matrix4x4 camM;

    private void Start()
    {
        cam = Camera.main;
        camM = cam.projectionMatrix;
    }

    //Cam Projection Matrix
    void LateUpdate()
    {
       
        Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
        cam.projectionMatrix = m;
    }

    Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x + camM[0, 0];
        m[0, 1] = 0;
        m[0, 2] = a + camM[0, 2];
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y + camM[1, 1];
        m[1, 2] = b + camM[1, 2];
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c + camM[2, 2];
        m[2, 3] = d + camM[2, 3];
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e + camM[3, 2];
        m[3, 3] = 0;
        return m;
    }
}
