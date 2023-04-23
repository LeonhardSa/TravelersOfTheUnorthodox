using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScript : MonoBehaviour
{

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("CheckForDeathplane", 1f, 1f);
    }

    void CheckForDeathplane()
    {
        if (player.transform.position.y <= -15f)
        {
            player.transform.position = Vector3.up;
            player.transform.localScale = Vector3.one;
            player.transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.transform.position = Vector3.zero;
            other.transform.localScale = Vector3.one;
            other.transform.eulerAngles = Vector3.zero;
        }
    }
}