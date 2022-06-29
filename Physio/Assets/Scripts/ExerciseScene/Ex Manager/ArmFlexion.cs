using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmFlexion : MonoBehaviour {

    public GameObject leftShoulder;
    public GameObject leftElbow;
    public GameObject leftWrist;

    public GameObject rightShoulder;
    public GameObject rightElbow;
    public GameObject rightWrist;

    private Vector3 elbowShoulder;
    private Vector3 elbowWrist;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (State.isTherapyOnGoing) {
            if (State.exercise.isLeftArm()) {
                elbowShoulder = leftShoulder.transform.position - leftElbow.transform.position;
                elbowWrist = leftWrist.transform.position - leftElbow.transform.position;
            }
            else if (!State.exercise.isLeftArm()) {
                elbowShoulder = rightShoulder.transform.position - rightElbow.transform.position;
                elbowWrist = rightWrist.transform.position - rightElbow.transform.position;
            }

            float elbowAngle = Vector3.Angle(elbowShoulder, elbowWrist);


            if (State.currentTarget == 2 && elbowAngle > 130) {
                Debug.Log(elbowAngle + " " + State.currentTarget);
                State.compensationInCurrentRep = true;
                State.exercise.incOutOfPath();
            }

            if (State.currentTarget == 3 && elbowAngle > 110) {
                Debug.Log(elbowAngle + " " + State.currentTarget);
                State.compensationInCurrentRep = true;
                State.exercise.incOutOfPath();
            }
        }        
    }
}
