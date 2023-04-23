using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureplateScript : MonoBehaviour
{

    public AudioClip clip;

    public GameObject toUnlock;

    public bool activate = false;

    private int objectCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Contains("Player") || collision.transform.tag.Contains("Portable") || collision.transform.tag.Contains("Porting"))
        {
            if (objectCount == 0) AudioSource.PlayClipAtPoint(clip, toUnlock.transform.position);
            objectCount++;
            toUnlock.SetActive(activate);
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag.Contains("Player") || collision.transform.tag.Contains("Portable") || collision.transform.tag.Contains("Porting"))
        {
            objectCount--;
            if (objectCount == 0)
            {
                AudioSource.PlayClipAtPoint(clip, toUnlock.transform.position);
                toUnlock.SetActive(!activate);
                GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
    }
}
