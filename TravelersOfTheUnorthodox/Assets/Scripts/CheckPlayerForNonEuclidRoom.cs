using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerForNonEuclidRoom : MonoBehaviour
{
    MeshManipulationTests mmt;

    private void Start()
    {
        mmt = gameObject.GetComponentInParent<MeshManipulationTests>();

        if (mmt.inside) this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player")) mmt.inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player")) mmt.inside = false;
    }
}
