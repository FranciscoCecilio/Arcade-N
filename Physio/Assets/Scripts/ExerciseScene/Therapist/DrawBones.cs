using UnityEngine;
using System.Collections;
using System;

/*
 * DESCRIPTION
 * 
 * This script will draw a line between two joints.
 * 
 * With this script we aim to obtain a simple representation of the skeleton by using
 * line renderers. This was done by defining the two joints which will connect the line.
 * 
 * Attach this script to the GameObjects in LineRenderer.
 */

public class DrawBones : MonoBehaviour {

	private LineRenderer linerenderer;
	
	public Transform origin;
	public Transform destination;

    void Start()
	{
		linerenderer = GetComponent<LineRenderer> ();
    }

    void LateUpdate()
	{
        linerenderer.SetPosition (0, origin.position);
		linerenderer.SetPosition (1, destination.position);
	}

}
