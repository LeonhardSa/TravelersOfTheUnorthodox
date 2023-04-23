using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTestScript : MonoBehaviour
{
    public Transform obj1;
    public Transform obj2;

    private GameObject playerCam;
    private Vector3 o1, o2;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.eulerAngles = playerCam.transform.eulerAngles + obj1.eulerAngles - (obj2.eulerAngles - new Vector3(0, -180, 0));

        this.transform.eulerAngles = obj1.eulerAngles - (obj2.eulerAngles - new Vector3(0, -180, 0));
        this.transform.Rotate(playerCam.transform.eulerAngles);
    }
}
