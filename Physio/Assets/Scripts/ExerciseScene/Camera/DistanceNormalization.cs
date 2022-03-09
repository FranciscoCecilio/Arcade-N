using UnityEngine;
using System.Collections;

/*
 * DESCRIPTION
 * This script allows to preserve the distance of the camera to the user.
 * 
 * As the user moves towards to the Kinect Sensor (or the opposite) the skeleton 
 * will maintain the same size. There's no zoom in neither zoom out.
 * 
 * This script is applied to each camera and the GameObject used for reference was the spineMidPos.
 */

public class DistanceNormalization : MonoBehaviour {

	public GameObject bodypart;

	private Vector3 distance;

    public float y = 0f;

	void Start()
	{
		distance.z = transform.position.z - bodypart.transform.position.z;
	}

	void LateUpdate ()
	{
		transform.position = bodypart.transform.position + distance;
        transform.position = transform.position + new Vector3(0f, y, 0f);  
	}
}
