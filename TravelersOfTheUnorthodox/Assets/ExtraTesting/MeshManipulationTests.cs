using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshManipulationTests : MonoBehaviour
{

    public bool customScaling = false;
    public bool inside = false;

    public List<Vector3> scaling = new List<Vector3>();
    List<Vector3> oldScaling = new List<Vector3>();

    [SerializeField]
    private List<Transform> objects = new List<Transform>();

    private List<Vector3> objectsPosCalcCopy = new List<Vector3>();
    private List<Quaternion> objectsRotationCalcCopy = new List<Quaternion>();
    private List<Vector3> objectsScaleCalcCopy = new List<Vector3>();

    private List<Mesh> objectsMesh = new List<Mesh>();
    private List<Vector3[]> objectsVertices = new List<Vector3[]>();
    private List<Vector3[]> objectsVerticesCalcCopy = new List<Vector3[]>();
    private Dictionary<Vector3, Vector3> calculatedVertices = new Dictionary<Vector3, Vector3>();

    public Vector3[] roomBounds = new Vector3[8];

    [Serializable]
    public struct MeshSetting
    {
        public string key;
        public MeshBoundSetting setup;
    }
    public MeshSetting[] meshSettings;

    Bounds bounds;
    Vector3 offset;

    int count;

    Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {

        Invoke("ScanRoom", Time.fixedDeltaTime);

        playerCam = Camera.main.transform;

        oldScaling.Clear();
        for (int n = 0; n < scaling.Count; n++)
        {
            oldScaling.Add(scaling[n]);
        }

        bounds = GetComponent<BoxCollider>().bounds;

        count = 0;
        foreach (MeshSetting roomBound in meshSettings)
        {
            roomBounds[count] = Vector3.Scale(bounds.extents, roomBound.setup.boundDirection);
            count++;
        }

        //Debug.Log("Coll: " + (transform.position + (Quaternion.Inverse(transform.rotation) * (bounds.center - transform.position))));
    }

    // Update is called once per frame
    void Update()
    {
        //DO NOT TOUCH THIS CODE!!!
        //Moving Objects Version
        /*

        for (int c = 0; c < objects.Count; c++)
        {
            offset = Vector3.zero;

            for (int v = 0; v < roomBounds.Length; v++)
            {
                //Vector3 globalRoomBound = bounds.center + roomBounds[v];

                Vector3 objToBoundsAbs = (bounds.center + roomBounds[v]) - objectsPosCalcCopy[c];
                objToBoundsAbs.x = Mathf.Abs(objToBoundsAbs.x);
                objToBoundsAbs.y = Mathf.Abs(objToBoundsAbs.y);
                objToBoundsAbs.z = Mathf.Abs(objToBoundsAbs.z);

                Vector3 objToBoundsPct; //"1" is being close
                objToBoundsPct.x = (bounds.size.x - Mathf.Abs(objToBoundsAbs.x)) / bounds.size.x;
                objToBoundsPct.y = (bounds.size.y - Mathf.Abs(objToBoundsAbs.y)) / bounds.size.y;
                objToBoundsPct.z = (bounds.size.z - Mathf.Abs(objToBoundsAbs.z)) / bounds.size.z;

                offset.x += (bounds.size.x - Mathf.Abs(objToBoundsAbs.x)) * scaling[v].x * (objToBoundsPct.y * objToBoundsPct.z) * Mathf.Sign(roomBounds[v].x);
                offset.y += (bounds.size.y - Mathf.Abs(objToBoundsAbs.y)) * scaling[v].y * (objToBoundsPct.x * objToBoundsPct.z) * Mathf.Sign(roomBounds[v].y);
                offset.z += (bounds.size.z - Mathf.Abs(objToBoundsAbs.z)) * scaling[v].z * (objToBoundsPct.x * objToBoundsPct.y) * Mathf.Sign(roomBounds[v].z);

                if (objects[c].name == "Sphere") Debug.Log(objToBoundsPct);
            }

            objects[c].position = objectsPosCalcCopy[c] - offset;
        }

        */

        if (!inside) return;

        if (customScaling)
        {
            count = 0;
            foreach (MeshSetting roomBound in meshSettings)
            {
                Vector3 newScaling = Vector3.zero;

                Vector3 playerMinGlobal = bounds.center + Vector3.Scale(bounds.extents, roomBound.setup.playerMinPosFromCenter);
                Vector3 playerMaxGlobal = bounds.center + Vector3.Scale(bounds.extents, roomBound.setup.playerMaxPositionFromCenter);

                Vector3 minMaxDiffNormal = (playerMaxGlobal - playerMinGlobal).normalized;
                float playerPosDiff = Vector3.Project(playerCam.position - playerMinGlobal, minMaxDiffNormal).magnitude;
                playerPosDiff *= Mathf.Sign(Vector3.Dot(minMaxDiffNormal, playerCam.position - playerMinGlobal));

                float minMaxDiff = (playerMaxGlobal - playerMinGlobal).magnitude;

                newScaling.x = Mathf.Clamp(playerPosDiff, 0, minMaxDiff);
                newScaling.y = Mathf.Clamp(playerPosDiff, 0, minMaxDiff);
                newScaling.z = Mathf.Clamp(playerPosDiff, 0, minMaxDiff);

                newScaling.x = Remap(newScaling.x, 0, minMaxDiff, roomBound.setup.boundMinScaling.x, roomBound.setup.boundMaxScaling.x);
                newScaling.y = Remap(newScaling.y, 0, minMaxDiff, roomBound.setup.boundMinScaling.y, roomBound.setup.boundMaxScaling.y);
                newScaling.z = Remap(newScaling.z, 0, minMaxDiff, roomBound.setup.boundMinScaling.z, roomBound.setup.boundMaxScaling.z);

                scaling[count] = newScaling;
                count++;
            }
        }

        for (int m = 0; m < objectsMesh.Count; m++)
        {

            for (int c = 0; c < objectsVertices[m].Length; c++)
            {
                {
                    offset = Vector3.zero;

                    Vector3 globalVertexPos = (objectsPosCalcCopy[m] + objectsRotationCalcCopy[m] * Vector3.Scale(objectsVerticesCalcCopy[m][c], objectsScaleCalcCopy[m]));

                    for (int v = 0; v < roomBounds.Length; v++)
                    {
                        Vector3 vertexToBoundAbs = (bounds.center + roomBounds[v]) - globalVertexPos;
                        vertexToBoundAbs.x = Mathf.Abs(vertexToBoundAbs.x);
                        vertexToBoundAbs.y = Mathf.Abs(vertexToBoundAbs.y);
                        vertexToBoundAbs.z = Mathf.Abs(vertexToBoundAbs.z);

                        Vector3 objToBoundsPct; //"1" is being close
                        objToBoundsPct.x = (bounds.size.x - Mathf.Abs(vertexToBoundAbs.x)) / bounds.size.x;
                        objToBoundsPct.y = (bounds.size.y - Mathf.Abs(vertexToBoundAbs.y)) / bounds.size.y;
                        objToBoundsPct.z = (bounds.size.z - Mathf.Abs(vertexToBoundAbs.z)) / bounds.size.z;

                        offset.x += (bounds.size.x - Mathf.Abs(vertexToBoundAbs.x)) * scaling[v].x * (objToBoundsPct.y * objToBoundsPct.z) * Mathf.Sign(roomBounds[v].x);
                        offset.y += (bounds.size.y - Mathf.Abs(vertexToBoundAbs.y)) * scaling[v].y * (objToBoundsPct.x * objToBoundsPct.z) * Mathf.Sign(roomBounds[v].y);
                        offset.z += (bounds.size.z - Mathf.Abs(vertexToBoundAbs.z)) * scaling[v].z * (objToBoundsPct.x * objToBoundsPct.y) * Mathf.Sign(roomBounds[v].z);
                    }

                    globalVertexPos = globalVertexPos - offset;
                    Vector3 unscaledVertexPos = Quaternion.Inverse(objectsRotationCalcCopy[m]) * (globalVertexPos - objectsPosCalcCopy[m]);
                    objectsVertices[m][c] = Vector3.Scale(unscaledVertexPos, new Vector3(1 / objectsScaleCalcCopy[m].x, 1 / objectsScaleCalcCopy[m].y, 1 / objectsScaleCalcCopy[m].z));

                    //calculatedVertices.Add(objectsVerticesCalcCopy[m][c], objectsVertices[m][c]);
                }


            }

            if (objects[m].GetComponent<MeshCollider>() && AreVector3ListsEqual(scaling, oldScaling))
            {
                objectsMesh[m].vertices = objectsVertices[m];
                objectsMesh[m].RecalculateBounds();

                bool isTrigger = objects[m].GetComponent<MeshCollider>().isTrigger;
                Destroy(objects[m].GetComponent<MeshCollider>());
                objects[m].gameObject.AddComponent<MeshCollider>();
                objects[m].gameObject.GetComponent<MeshCollider>().convex = isTrigger;
                objects[m].gameObject.GetComponent<MeshCollider>().isTrigger = isTrigger;
            }

            calculatedVertices.Clear();

        }

        if (AreVector3ListsEqual(scaling, oldScaling))
        {
            oldScaling.Clear();
            for (int n = 0; n < scaling.Count; n++)
            {
                oldScaling.Add(scaling[n]);
            }
        }
    }

    internal void ScanRoom()
    {
        foreach(MeshFilter obj in FindObjectsOfType<MeshFilter>())
        {
            if (bounds.Contains(obj.transform.position))
            {
                if(obj.gameObject != this.gameObject)
                {
                    objects.Add(obj.transform);
                    objectsPosCalcCopy.Add(obj.transform.position);
                    objectsRotationCalcCopy.Add(obj.transform.rotation);
                    objectsScaleCalcCopy.Add(obj.transform.localScale);

                    objectsMesh.Add(obj.GetComponent<MeshFilter>().mesh);
                    objectsVertices.Add(obj.GetComponent<MeshFilter>().mesh.vertices);
                    objectsVerticesCalcCopy.Add((Vector3[])obj.GetComponent<MeshFilter>().mesh.vertices.Clone());
                }
            }
        }
    }

    internal void ClearRoomInformation()
    {
        objects.Clear();
        objectsPosCalcCopy.Clear();
        objectsScaleCalcCopy.Clear();

        objectsMesh.Clear();
        objectsVertices.Clear();
        objectsVerticesCalcCopy.Clear();
    }


    public bool AreVector3ListsEqual(List<Vector3> vec1List, List<Vector3> vec2List)
    {

        if (vec1List.Count != vec2List.Count) return false;

        for (int n = 0; n < vec1List.Count; n++)
        {
            if (vec1List[n] != vec2List[n]) return true;
        }

        return false;
    }

    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}