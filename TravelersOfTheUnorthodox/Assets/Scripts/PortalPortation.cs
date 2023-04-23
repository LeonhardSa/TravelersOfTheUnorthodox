using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPortation : MonoBehaviour
{
	//public Transform player;
	public Transform receiver;
	private List<Transform[]> clippingObjs = new List<Transform[]>();

	private HashSet<Transform> objsInPortal = new HashSet<Transform>();

	private float oldDotProduct = 0f;

	private PortingObjectManager pom;

    private void Start()
    {

		pom = FindObjectOfType<PortingObjectManager>();

	}

    // Update is called once per frame
    void Update()
	{
		
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Portable" || (other.tag == "Porting"))
		{
			if (!objsInPortal.Contains(other.transform))
			{
				Debug.Log("Portable Object " + other.name + " entered portal: " + this.name);

				//create mimic object
				GameObject clone = Instantiate(other.gameObject);
				clone.name = other.name + "Clone";
				clone.tag = "PortableClone";

				Transform[] pair = new Transform[2];
				pair[0] = clone.transform;
				pair[1] = other.transform;
				
				/*
				clippingObjs.Add(pair);
				receiver.GetComponent<PortalPortation>().clippingObjs.Add(pair);
				*/

				//Set transforms of mimic object
				clone.transform.rotation = other.transform.rotation;

				clone.transform.position = other.transform.position + receiver.position - transform.position;

				clone.transform.localScale = new Vector3(other.transform.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														 other.transform.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														 other.transform.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);

				objsInPortal.Add(clone.transform);
				objsInPortal.Add(other.transform);
				/*
				receiver.GetComponent<PortalPortation>().objsInPortal.Add(clone.transform);
				receiver.GetComponent<PortalPortation>().objsInPortal.Add(other.transform);
				*/

				//pom.clippingObjs.Add(pair);

				pom.portingObjects.Add(other.transform, new PortingObject(other.transform, clone.transform, transform, receiver.transform));

			}
		}
	}

	void OnTriggerExit(Collider other)
	{

		Debug.Log(other.name + " left Portal: " + this.name);

		if (other.tag == "Player")
		{
			oldDotProduct = 0f;
		}

		if (other.tag == "Porting")
		{
			for (int p = 0; p < clippingObjs.Count; p++)
			{
				if (clippingObjs[p][1] == other.transform)
				{
					Debug.Log("Took Original");

					FindObjectOfType<PortingObjectManager>().portingObjects.Remove(clippingObjs[p][1]);

					objsInPortal.Remove(clippingObjs[p][0]);
					objsInPortal.Remove(clippingObjs[p][1]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][0]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][1]);

					GameObject.Destroy(clippingObjs[p][0].gameObject, 0f);

					clippingObjs.RemoveAt(p);
				}
				else if (clippingObjs[p][0] == other.transform)
				{
					Debug.Log("Took Copy");

					FindObjectOfType<PortingObjectManager>().portingObjects.Remove(clippingObjs[p][1]);

					objsInPortal.Remove(clippingObjs[p][0]);
					objsInPortal.Remove(clippingObjs[p][1]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][0]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][1]);

					GameObject.Destroy(clippingObjs[p][1].gameObject, 0f);

					clippingObjs.RemoveAt(p);
				}
			}
		}
		else if (other.tag == "Portable")
		{
			for (int p = 0; p < clippingObjs.Count; p++)
			{
				if (clippingObjs[p][1] == other.transform)
				{
					Debug.Log("Took Original");

					FindObjectOfType<PortingObjectManager>().portingObjects.Remove(clippingObjs[p][1]);

					objsInPortal.Remove(clippingObjs[p][0]);
					objsInPortal.Remove(clippingObjs[p][1]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][0]);
					receiver.GetComponent<PortalPortation>().objsInPortal.Remove(clippingObjs[p][1]);

					GameObject.Destroy(clippingObjs[p][0].gameObject, 0f);

					clippingObjs.RemoveAt(p);
				}
			}
		}
	}


	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Portable")
		{
			Transform otherTransfrom = other.gameObject.GetComponent<Transform>();

			Vector3 portalToOther = otherTransfrom.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToOther);

			// If this is true: The object has moved across the portal
			if (dotProduct >= 0f && oldDotProduct < 0f)
			{
				// Teleport it!
				float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
				rotationDiff += 180;
				otherTransfrom.Rotate(Vector3.up, rotationDiff);

				/*
				var angleA = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
				var angleB = Mathf.Atan2(receiver.forward.x, receiver.forward.z) * Mathf.Rad2Deg;

				// get the signed difference in these angles
				var angleDiff = Mathf.DeltaAngle(angleB, angleA);

				Quaternion portalRotationalDifference = Quaternion.AngleAxis(angleDiff, Vector3.up);
				Vector3 newOtherDirection = portalRotationalDifference * otherTransfrom.forward;
				otherTransfrom.rotation = Quaternion.LookRotation(newOtherDirection, Vector3.up);
				*/

				otherTransfrom.localScale = new Vector3(otherTransfrom.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														otherTransfrom.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														otherTransfrom.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToOther;
				otherTransfrom.position = receiver.position + positionOffset;
				//playerIsOverlapping = false;
			}

			oldDotProduct = dotProduct;
		}
		/*
		else if(other.tag == "Portable" || other.tag == "Porting")
        {
			for (int p = 0; p < clippingObjs.Count; p++)
			{
				if (clippingObjs[p][0].tag == "Porting" && once == true)
				{
					clippingObjs[p][1].rotation = clippingObjs[p][0].rotation;

					clippingObjs[p][1].position = clippingObjs[p][0].position + receiver.position - transform.position;

					clippingObjs[p][1].localScale = new Vector3(clippingObjs[p][0].localScale.x * transform.lossyScale.x / receiver.lossyScale.x,
																clippingObjs[p][0].localScale.y * transform.lossyScale.y / receiver.lossyScale.y,
																clippingObjs[p][0].localScale.z * transform.lossyScale.z / receiver.lossyScale.z);

					once = false;

					GameObject clone = Instantiate(clippingObjs[p][0].gameObject);

					clippingObjs[p][0] = clippingObjs[p][1];

					clippingObjs[p][1] = clone.transform;
					
				}
				else
				{
					
					once = true;

					clippingObjs[p][0].rotation = clippingObjs[p][1].rotation;

					clippingObjs[p][0].position = clippingObjs[p][1].position + receiver.position - transform.position;

					clippingObjs[p][0].localScale = new Vector3(clippingObjs[p][1].localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
																clippingObjs[p][1].localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
																clippingObjs[p][1].localScale.z * receiver.lossyScale.z / transform.lossyScale.z);
					
				}
			}
		}*/
	}
}