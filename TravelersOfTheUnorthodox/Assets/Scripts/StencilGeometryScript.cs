using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilGeometryScript : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;

    Transform portalTransform;
    public Transform playerTransform;

    public float viewDepth = 0;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {

        portalTransform = transform.parent.transform;

        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        vertices[2] += Vector3.down + Vector3.forward;
        vertices[3] += Vector3.down + Vector3.forward;
        
        vertices[1] = Vector3.right * 1.5f;
        vertices[0] = Vector3.right * -1.5f;
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        //vertices order clockwise 1023
    }

    void Update()
    {
        Vector3 playerToPortalCornerRightNormalized = portalTransform.position + (portalTransform.right * 1.5f) - playerTransform.position;
        Vector3 playerToPortalCornerLeftNormalized = portalTransform.position + (portalTransform.right * -1.5f) - playerTransform.position;

        playerToPortalCornerRightNormalized.Normalize();
        playerToPortalCornerLeftNormalized.Normalize();

        vertices[3] = Quaternion.Euler(-portalTransform.eulerAngles.x, -portalTransform.eulerAngles.y, -portalTransform.eulerAngles.z) * (new Vector3(playerToPortalCornerRightNormalized.x, 0, playerToPortalCornerRightNormalized.z) * 10f + (portalTransform.right * 1.5f));
        vertices[2] = Quaternion.Euler(-portalTransform.eulerAngles.x, -portalTransform.eulerAngles.y, -portalTransform.eulerAngles.z) * (new Vector3(playerToPortalCornerLeftNormalized.x, 0, playerToPortalCornerLeftNormalized.z) * 10f + (portalTransform.right * -1.5f));

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    void BackCornerPos()
    {

        /*
        Vector2 portalPos = new Vector2(portalTransform.position.x, portalTransform.position.z);
        Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.z);

        float playerToPortal = (portalPos - playerPos).magnitude;

        Vector2 playerToPortalCornerRightNormalized = portalPos + (Vector2)(portalTransform.right * 1.5f) - playerPos;
        float playerToPortalCornerRightDistance = playerToPortalCornerRightNormalized.magnitude;
        playerToPortalCornerRightNormalized.Normalize();

        Vector2 playerToPortalCornerLeftNormalized = portalPos + (Vector2)(portalTransform.right * -1.5f) - playerPos;
        float playerToPortalCornerLeftDistance = playerToPortalCornerLeftNormalized.magnitude;
        playerToPortalCornerLeftNormalized.Normalize();
        */

        /*
        float anglePlayer = Mathf.Acos((Mathf.Pow(3, 2) - Mathf.Pow(playerToPortalCornerRightDistance, 2) - Mathf.Pow(playerToPortalCornerLeftDistance, 2)) / -2 / playerToPortalCornerRightDistance / playerToPortalCornerLeftDistance);
        float anglePortalCornerRight = Mathf.Acos((Mathf.Pow(playerToPortalCornerLeftDistance, 2) - Mathf.Pow(playerToPortalCornerRightDistance, 2) - Mathf.Pow(3, 2)) / -2 / playerToPortalCornerRightDistance / 3);
        float anglePortalCornerLeft = 180 - anglePlayer - anglePortalCornerRight;

        float anglePortalFront = Mathf.Asin(playerToPortalCornerRightDistance * Mathf.Sin(anglePortalCornerRight) / playerToPortal);

        float anglePlayerRight = 180 - anglePortalCornerRight - anglePortalFront;
        float anglePlayerLeft = 180 - anglePortalCornerLeft - (180 - anglePortalFront);

        float playerToBackCornerRight = (playerToPortal + viewDepth) / Mathf.Sin(anglePlayerRight);
        float playerToBackCornerLeft = (playerToPortal + viewDepth) / Mathf.Sin(anglePlayerLeft);

        float portalCornerRightToBackCornerRight = playerToBackCornerRight - playerToPortalCornerRightDistance;
        float portalCornerLeftToBackCornerLeft = playerToBackCornerLeft - playerToPortalCornerLeftDistance;

        (Vector2, Vector2) vectorPair = (playerToPortalCornerRightNormalized * portalCornerRightToBackCornerRight + (Vector2)(portalTransform.right * 1.5f),
                                         playerToPortalCornerLeftNormalized * portalCornerLeftToBackCornerLeft + (Vector2)(portalTransform.right * -1.5f));

        return vectorPair;
        */

        /*
        float anglePortalFront = Mathf.Acos((Mathf.Pow(playerToPortalCornerDistance, 2) - Mathf.Pow(playerToPortal, 2) - Mathf.Pow(1.5f, 2)) / -2 / playerToPortal / 1.5f);
        float anglePortalFrontCorner = Mathf.Asin(playerToPortal * Mathf.Sin(anglePortalFront) / playerToPortalCornerDistance);

        float anglePortalInside = 180 - anglePortalFront;
        float portalCornerToBack = Mathf.Sqrt(Mathf.Pow(1.5f, 2) + Mathf.Pow(viewDepth, 2) - 2 * 1.5f * viewDepth * Mathf.Cos(anglePortalInside));

        float angleBackTowardsPortal = Mathf.Asin(1.5f * Mathf.Sin(anglePortalInside) / portalCornerToBack);
        float angleBackAwayFromPortal = 90 - angleBackTowardsPortal;
        float angleBackCorner = 360 - anglePortalInside - (180 - anglePortalFrontCorner) - 90;

        float portalCornerToBackCorner = Mathf.Sin(angleBackAwayFromPortal) * portalCornerToBack / Mathf.Sin(angleBackCorner);

        return playerToPortalCornerNormalized * portalCornerToBackCorner + portalTransform.right * cornerDirection;
        */
    }
}
