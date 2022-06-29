using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO : uncomment the update when trying with kinect  
// Cursor is the object that triggers the targets (it always follows the hand of the kinect body)
public class Cursor : MonoBehaviour {

    public GameObject cursor;
    public GameObject secondaryCursor;

    public GameObject leftHand;
    public GameObject rightHand;

    ///private Vector3 offset = new Vector3(0, -1, 0);
	
	// Update is called once per frame
	void Update () {
        secondaryCursor.SetActive(State.hasSecondaryCursor);

        // for testing --------------
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        cursor.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        // for testing --------------

        /*if(State.exercise.isLeftArm()) {
            cursor.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
            secondaryCursor.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
        }
        else if (!State.exercise.isLeftArm()) {
            cursor.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
            secondaryCursor.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
        }*/
    }
}
