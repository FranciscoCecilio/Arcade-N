using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class CreateCapsule : MonoBehaviour {

	public GameObject linerenderer; 
	 
	private Vector3 initial;
	private Vector3 final;
	private Vector3 center;

	private float centerx;
	private float centery;
	private float centerz;

	private LineRenderer positions;

	public float scaleCapsulex;
	private float scaleCapsuley;
	public float scaleCapsulez;
	public float sizeJoints; 

	// Use this for initialization
	void Start () {

		positions = linerenderer.GetComponent<LineRenderer> ();

	}
		
	//LateUpdate is called after all Update functions have been called.  
	void LateUpdate () {

		initial=positions.GetPosition (0);
		final=positions.GetPosition (1); 

		centerx = initial.x + (final.x - initial.x) / 2;
		centery = initial.y + (final.y - initial.y) / 2;
		centerz = initial.z + (final.z - initial.z) / 2; 

		center=new Vector3(centerx,centery,centerz);
	
		gameObject.transform.position = new Vector3 (center.x, center.y, center.z);
 
		//Orientation of the Vector
		Vector3 diff=final-initial; 
		gameObject.transform.up = diff;

		//Set the capsule's scale
		scaleCapsuley= (float)Vector3.Distance (initial, final);

		//Multiplying by 2 because is the size of the line (Inspector-Positions-Size)
		scaleCapsuley = scaleCapsuley * 2; 
		scaleCapsuley = scaleCapsuley - sizeJoints*0.2f; 


		gameObject.transform.localScale= new Vector3(scaleCapsulex, scaleCapsuley, scaleCapsulez);


	}
}


