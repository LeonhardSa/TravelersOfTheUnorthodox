using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPorting : MonoBehaviour
{
	[HideInInspector]
	public Transform receiver;

	public bool uniquePortal;

	private float oldDotProduct = 0f;

	private PortingObjectManager pom;

	// Start is called before the first frame update
	private void Start()
	{
		pom = FindObjectOfType<PortingObjectManager>();
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag.Contains("Normal") && uniquePortal == true)
		{
			Destroy(other.gameObject);
		}
		else if (other.tag.Contains("Portable") || other.tag.Contains("Porting"))
		{

			if (!pom.objsInPortal.Contains(other.transform))
			{
				Debug.Log("Portable Object " + other.name + " entered portal: " + this.name);

				//create mimic object
				GameObject clone = Instantiate(other.gameObject);
				clone.name = other.name + "Clone";
				clone.tag = "PortableClone/" + other.tag.Split('/')[1];

				Transform[] pair = new Transform[2];
				pair[0] = clone.transform;
				pair[1] = other.transform;

				//Set transforms of mimic object
				clone.transform.rotation = other.transform.rotation;

				clone.transform.position = other.transform.position + receiver.position - transform.position;

				clone.transform.localScale = new Vector3(other.transform.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														 other.transform.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														 other.transform.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);

				pom.objsInPortal.Add(clone.transform);
				pom.objsInPortal.Add(other.transform);

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

		if (other.tag.Contains("Porting") || other.tag.Contains("Portable"))
		{
			foreach (KeyValuePair<Transform, PortingObject> portObj in pom.portingObjects)
			{
				if (portObj.Value.objOriginal == other.transform)
				{
					Debug.Log("Took Original");

					pom.objsInPortal.Remove(portObj.Value.objOriginal);
					pom.objsInPortal.Remove(portObj.Value.objClone);

					pom.portingObjects.Remove(portObj.Value.objOriginal);

					GameObject.Destroy(portObj.Value.objClone.gameObject, 0f);
				}
				else if (portObj.Key == other.transform)
				{
					Debug.Log("Took Copy");

					pom.objsInPortal.Remove(portObj.Value.objOriginal);
					pom.objsInPortal.Remove(portObj.Value.objClone);

					pom.portingObjects.Remove(portObj.Value.objOriginal);

					GameObject.Destroy(portObj.Value.objClone.gameObject, 0f);
				}
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" || other.tag.Contains("Portable"))
		{
			Transform otherTransform = other.gameObject.GetComponent<Transform>();

			Vector3 portalToOther = otherTransform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToOther);

			// If this is true: The object has moved across the portal
			if (dotProduct >= 0f && oldDotProduct < 0f)
			{
				// Teleport it!
				float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
				//otherTransform.Rotate(Vector3.up, rotationDiff);

				//Still line is commented for Demo
				//otherTransform.Rotate(receiver.eulerAngles.x - transform.eulerAngles.x, receiver.eulerAngles.y - transform.eulerAngles.y - 180, receiver.eulerAngles.z - transform.eulerAngles.z);
				otherTransform.Rotate(transform.eulerAngles.z + receiver.eulerAngles.z, transform.eulerAngles.y - receiver.eulerAngles.y - 180, transform.eulerAngles.z + receiver.eulerAngles.z);

				/*
				var angleA = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
				var angleB = Mathf.Atan2(receiver.forward.x, receiver.forward.z) * Mathf.Rad2Deg;

				// get the signed difference in these angles
				var angleDiff = Mathf.DeltaAngle(angleB, angleA);

				Quaternion portalRotationalDifference = Quaternion.AngleAxis(angleDiff, Vector3.up);
				Vector3 newOtherDirection = portalRotationalDifference * otherTransform.forward;
				otherTransform.rotation = Quaternion.LookRotation(newOtherDirection, Vector3.up);
				*/

				otherTransform.localScale = new Vector3(otherTransform.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														otherTransform.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														otherTransform.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);


				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToOther;

				positionOffset.x *= (receiver.lossyScale.x / transform.lossyScale.x);
				positionOffset.y *= (receiver.lossyScale.y / transform.lossyScale.y);
				positionOffset.z *= (receiver.lossyScale.z / transform.lossyScale.z);

				otherTransform.position = receiver.position + Quaternion.Euler(0, receiver.eulerAngles.y - transform.eulerAngles.y, 0) * positionOffset;
			}

			oldDotProduct = dotProduct;
		}
	}
}