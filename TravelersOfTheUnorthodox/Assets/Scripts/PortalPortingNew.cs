using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPortingNew : MonoBehaviour
{
	[HideInInspector]
	public Transform receiver;

	public bool onlyPlayer;

	public bool uniquePortal;

	private float oldDotProduct = 0f;

	private PortingObjectManager pom;

	private Transform transformInPortal;

	// Start is called before the first frame update
	private void Start()
	{
		pom = FindObjectOfType<PortingObjectManager>();
	}

    private void FixedUpdate()
    {
        if (transformInPortal != null)
        {
			TeleportationCheck(transformInPortal);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Contains("Player"))
			transformInPortal = other.gameObject.GetComponent<Transform>();

		if (other.tag.Contains("Normal") && uniquePortal || other.tag.Contains("Unique") && onlyPlayer)
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
				clone.tag = "PortableClone/"+other.tag.Split('/')[1];

				Transform[] pair = new Transform[2];
				pair[0] = clone.transform;
				pair[1] = other.transform;

				pom.objsInPortal.Add(clone.transform);
				pom.objsInPortal.Add(other.transform);

				pom.portingObjects.Add(other.transform, new PortingObject(other.transform, clone.transform, transform, receiver.transform));

			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			oldDotProduct = 0f;
			transformInPortal = null;
		}

		if (other.tag.Contains("Porting"))
		{
			//Why...
			if (other.GetComponent<PickUpGravity>())
			{
				Vector3 rotationDiff = transform.eulerAngles - receiver.eulerAngles;

				other.GetComponent<PickUpGravity>().gravityDirection = Quaternion.Euler(rotationDiff - Vector3.up * 180) * other.GetComponent<PickUpGravity>().gravityDirection;
			}

			foreach (KeyValuePair<Transform, PortingObject> portObj in pom.portingObjects)
			{
				if (portObj.Value.objOriginal == other.transform)
				{
					Debug.Log("Took Original");

					pom.objsInPortal.Remove(portObj.Value.objOriginal);
					pom.objsInPortal.Remove(portObj.Value.objClone);

					pom.portingObjects.Remove(portObj.Value.objOriginal);

					GameObject.Destroy(portObj.Value.objClone.gameObject, 0f);

					break;
				}
				else if (portObj.Key == other.transform)
				{
					Debug.Log("Took Copy");

					pom.objsInPortal.Remove(portObj.Value.objOriginal);
					pom.objsInPortal.Remove(portObj.Value.objClone);

					pom.portingObjects.Remove(portObj.Value.objOriginal);

					GameObject.Destroy(portObj.Value.objOriginal.gameObject, 0f);

					break;
				}
			}
		}
        else if(other.tag.Contains("Portable"))
        {
			//Why...
			if (other.GetComponent<PickUpGravity>())
			{
				Vector3 rotationDiff = transform.eulerAngles - receiver.eulerAngles;

				other.GetComponent<PickUpGravity>().gravityDirection = Quaternion.Euler(rotationDiff - Vector3.up * 180) * other.GetComponent<PickUpGravity>().gravityDirection;
			}

			foreach (KeyValuePair<Transform, PortingObject> portObj in pom.portingObjects)
			{
				if (portObj.Key == other.transform)
				{
					Debug.Log("Took Copy");

					pom.objsInPortal.Remove(portObj.Value.objOriginal);
					pom.objsInPortal.Remove(portObj.Value.objClone);

					pom.portingObjects.Remove(portObj.Value.objOriginal);

					GameObject.Destroy(portObj.Value.objOriginal.gameObject, 0f);

					break;
				}
			}
		}
	}

	private void TeleportationCheck(Transform otherTransform)
    {
		if (otherTransform.tag == "Player")
		{
			Vector3 portalToOther = otherTransform.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToOther.normalized);

			// If this is true: The object has moved across the portal
			if (dotProduct >= 0f && oldDotProduct < 0f || dotProduct <= 0f && oldDotProduct > 0f)
			{
				/*
				dotProduct = 0;
				
				// Teleport it!
				float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);

				//Still line is commented for Demo
				//otherTransform.Rotate(receiver.eulerAngles.x - transform.eulerAngles.x, receiver.eulerAngles.y - transform.eulerAngles.y - 180, receiver.eulerAngles.z - transform.eulerAngles.z);
				otherTransform.Rotate(transform.eulerAngles.x - receiver.eulerAngles.x, transform.eulerAngles.y - receiver.eulerAngles.y - 180, transform.eulerAngles.z - receiver.eulerAngles.z);

				Debug.Log(receiver.eulerAngles.y);

				otherTransform.localScale = new Vector3(otherTransform.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														otherTransform.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														otherTransform.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToOther;

				positionOffset.x *= (receiver.lossyScale.x / transform.lossyScale.x);
				positionOffset.y *= (receiver.lossyScale.y / transform.lossyScale.y);
				positionOffset.z *= (receiver.lossyScale.z / transform.lossyScale.z);

				otherTransform.position = receiver.position + Quaternion.Euler(0, receiver.eulerAngles.y - transform.eulerAngles.y, 0) * positionOffset;
				*/

				dotProduct = 0;

				Vector3 rotationDiff = transform.eulerAngles - receiver.eulerAngles;

				Vector3 positionOffset = portalToOther;

				otherTransform.localScale = new Vector3(otherTransform.localScale.x * receiver.lossyScale.x / transform.lossyScale.x,
														otherTransform.localScale.y * receiver.lossyScale.y / transform.lossyScale.y,
														otherTransform.localScale.z * receiver.lossyScale.z / transform.lossyScale.z);

				positionOffset.x *= (receiver.lossyScale.x / transform.lossyScale.x);
				positionOffset.y *= (receiver.lossyScale.y / transform.lossyScale.y);
				positionOffset.z *= (receiver.lossyScale.z / transform.lossyScale.z);

				positionOffset = Quaternion.Euler(rotationDiff) * positionOffset;
				positionOffset = Quaternion.AngleAxis(180f, Vector3.up) * positionOffset;

				
				otherTransform.position = receiver.position + positionOffset;

				Vector3 playerRotation = otherTransform.eulerAngles;

				otherTransform.eulerAngles = rotationDiff - Vector3.up * 180;
				otherTransform.Rotate(playerRotation);
			}

			oldDotProduct = dotProduct;
		}
	}
}