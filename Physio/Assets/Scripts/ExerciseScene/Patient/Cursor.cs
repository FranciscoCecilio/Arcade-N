using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    public GameObject cursor;
    public GameObject secondaryCursor;

    public GameObject leftHand;
    public GameObject rightHand;

    ///private Vector3 offset = new Vector3(0, -1, 0);
	
	// Update is called once per frame
	void Update () {
        secondaryCursor.SetActive(State.hasSecondaryCursor);

        if(State.exercise.isLeftArm()) {
            cursor.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
            secondaryCursor.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
        }
        else if (!State.exercise.isLeftArm()) {
            cursor.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
            secondaryCursor.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
        }
    }
}
