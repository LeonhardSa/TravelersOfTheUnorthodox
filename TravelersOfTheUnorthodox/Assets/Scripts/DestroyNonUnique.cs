using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNonUnique : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Normal"))
        {
            Destroy(other.gameObject);
        }
    }
}
