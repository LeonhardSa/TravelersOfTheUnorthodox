using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityActivityScript : MonoBehaviour
{

    Vector3 minBounds = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
    Vector3 maxBounds = new Vector3(int.MinValue, int.MinValue, int.MinValue);

    Transform parentTransform;

    List<Transform> children = new List<Transform>();

    List<Transform> firstChildren = new List<Transform>();

    private void Start()
    {

        parentTransform = transform.parent.transform;

        GetChildrenWithCollider(parentTransform);

        for (int c = parentTransform.childCount-1; c >= 0; c--)
        {
            firstChildren.Add(parentTransform.GetChild(c));
            parentTransform.GetChild(c).parent = null;
        }
        
        foreach (Transform child in children)
        {
            Bounds childBounds = child.GetComponent<Collider>().bounds;

            if (minBounds.x > child.position.x - childBounds.size.x / 2f) minBounds.x = child.position.x - childBounds.size.x / 2f;
            if (minBounds.y > child.position.y - childBounds.size.y / 2f) minBounds.y = child.position.y - childBounds.size.y / 2f;
            if (minBounds.z > child.position.z - childBounds.size.z / 2f) minBounds.z = child.position.z - childBounds.size.z / 2f;

            if (maxBounds.x < child.position.x + childBounds.size.x / 2f) maxBounds.x = child.position.x + childBounds.size.x / 2f;
            if (maxBounds.y < child.position.y + childBounds.size.y / 2f) maxBounds.y = child.position.y + childBounds.size.y / 2f;
            if (maxBounds.z < child.position.z + childBounds.size.z / 2f) maxBounds.z = child.position.z + childBounds.size.z / 2f;
        }

        transform.localScale = maxBounds - minBounds;
        transform.position = minBounds + transform.localScale / 2;

        foreach (Transform child in firstChildren)
        {
            child.parent = parentTransform;
        }

        firstChildren.Remove(this.transform);
    }

    private void OnBecameVisible()
    {
        foreach (Transform child in firstChildren)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void OnBecameInvisible()
    {
        foreach (Transform child in firstChildren)
        {
            child.gameObject.SetActive(false);
        }
    }

    void GetChildrenWithCollider(Transform root)
    {
        for (int c = 0; c < root.transform.childCount; c++)
        {
            if (!root.GetChild(c).name.Contains(transform.name))
            {
                if (root.GetChild(c).GetComponent<Collider>())
                {
                    children.Add(root.GetChild(c));
                }

                GetChildrenWithCollider(root.GetChild(c));
            }
        }
    }
}