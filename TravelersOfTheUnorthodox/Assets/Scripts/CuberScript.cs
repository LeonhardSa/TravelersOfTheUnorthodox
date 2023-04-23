using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuberScript : MonoBehaviour
{

    public GameObject cube;

    private void Awake()
    {
        Material m = cube.GetComponent<Renderer>().sharedMaterial;

        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material = m;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(cube, this.transform.position + Vector3.up, this.transform.rotation);
        InvokeRepeating(nameof(NewCubeQuestionMark), 10f, 10f);
    }

    void NewCubeQuestionMark()
    {
        if (!Physics.CheckSphere(this.transform.position + Vector3.up * 1.75f, 0.4f))
        {
            Instantiate(cube, this.transform.position + Vector3.up, this.transform.rotation);
        }
    }
}