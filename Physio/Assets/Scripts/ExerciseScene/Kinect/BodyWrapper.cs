using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class BodyWrapper : MonoBehaviour {
    private Body body;

    public float scale = 1.5f;

    public Vector3 offset = new Vector3(0, 0, 0);

    //Head
    public Vector3 headPos = new Vector3(0, 0, 0);
    public Vector3 neckPos = new Vector3(0, 0, 0);

    //Spine
    public Vector3 spineShoulderPos = new Vector3(0, 0, 0);
    public Vector3 spineMidPos = new Vector3(0, 0, 0);
    public Vector3 spineBasePos = new Vector3(0, 0, 0);

    //Left
    public Vector3 leftShoulderPos = new Vector3(0, 0, 0);
    public Vector3 leftElbowPos = new Vector3(0, 0, 0);
    public Vector3 leftWristPos = new Vector3(0, 0, 0);
    public Vector3 leftHandPos = new Vector3(0, 0, 0);
    public Vector3 leftThumbPos = new Vector3(0, 0, 0);
    public Vector3 leftHandTipPos = new Vector3(0, 0, 0);

    public Vector3 leftHipPos = new Vector3(0, 0, 0);
    /*public Vector3 leftKneePos = new Vector3(0, 0, 0);
    public Vector3 leftAnklePos = new Vector3(0, 0, 0);
    public Vector3 leftFootPos = new Vector3(0, 0, 0);*/

    //Right
    public Vector3 rightShoulderPos = new Vector3(0, 0, 0);
    public Vector3 rightElbowPos = new Vector3(0, 0, 0);
    public Vector3 rightWristPos = new Vector3(0, 0, 0);
    public Vector3 rightHandPos = new Vector3(0, 0, 0);
    public Vector3 rightThumbPos = new Vector3(0, 0, 0);
    public Vector3 rightHandTipPos = new Vector3(0, 0, 0);

    public Vector3 rightHipPos = new Vector3(0, 0, 0);
    /*public Vector3 rightKneePos = new Vector3(0, 0, 0);
    public Vector3 rightAnklePos = new Vector3(0, 0, 0);
    public Vector3 rightFootPos = new Vector3(0, 0, 0);*/

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(body != null) {
            Dictionary<JointType, Windows.Kinect.Joint> joints = body.Joints;

            //Head
            headPos = new Vector3(joints[JointType.Head].Position.X, joints[JointType.Head].Position.Y, joints[JointType.Head].Position.Z);
            neckPos = new Vector3(joints[JointType.Neck].Position.X, joints[JointType.Neck].Position.Y, joints[JointType.Neck].Position.Z);

            //Spine
            spineShoulderPos = new Vector3(joints[JointType.SpineShoulder].Position.X, joints[JointType.SpineShoulder].Position.Y, joints[JointType.SpineShoulder].Position.Z);
            spineMidPos = new Vector3(joints[JointType.SpineMid].Position.X, joints[JointType.SpineMid].Position.Y, joints[JointType.SpineMid].Position.Z);
            spineBasePos = new Vector3(joints[JointType.SpineBase].Position.X, joints[JointType.SpineBase].Position.Y, joints[JointType.SpineBase].Position.Z);

            //Left Arm
            leftHandPos = new Vector3(joints[JointType.HandLeft].Position.X, joints[JointType.HandLeft].Position.Y, joints[JointType.HandLeft].Position.Z);
            leftWristPos = new Vector3(joints[JointType.WristLeft].Position.X, joints[JointType.WristLeft].Position.Y, joints[JointType.WristLeft].Position.Z);
            leftElbowPos = new Vector3(joints[JointType.ElbowLeft].Position.X, joints[JointType.ElbowLeft].Position.Y, joints[JointType.ElbowLeft].Position.Z);
            leftShoulderPos = new Vector3(joints[JointType.ShoulderLeft].Position.X, joints[JointType.ShoulderLeft].Position.Y, joints[JointType.ShoulderLeft].Position.Z);
            leftThumbPos = new Vector3(joints[JointType.ThumbLeft].Position.X, joints[JointType.ThumbLeft].Position.Y, joints[JointType.ThumbLeft].Position.Z);
            leftHandTipPos = new Vector3(joints[JointType.HandTipLeft].Position.X, joints[JointType.HandTipLeft].Position.Y, joints[JointType.HandTipLeft].Position.Z);

            //Right Arm
            rightHandPos = new Vector3(joints[JointType.HandRight].Position.X, joints[JointType.HandRight].Position.Y, joints[JointType.HandRight].Position.Z);
            rightWristPos = new Vector3(joints[JointType.WristRight].Position.X, joints[JointType.WristRight].Position.Y, joints[JointType.WristRight].Position.Z);
            rightElbowPos = new Vector3(joints[JointType.ElbowRight].Position.X, joints[JointType.ElbowRight].Position.Y, joints[JointType.ElbowRight].Position.Z);
            rightShoulderPos = new Vector3(joints[JointType.ShoulderRight].Position.X, joints[JointType.ShoulderRight].Position.Y, joints[JointType.ShoulderRight].Position.Z);
            rightThumbPos = new Vector3(joints[JointType.ThumbRight].Position.X, joints[JointType.ThumbRight].Position.Y, joints[JointType.ThumbRight].Position.Z);
            rightHandTipPos = new Vector3(joints[JointType.HandTipRight].Position.X, joints[JointType.HandTipRight].Position.Y, joints[JointType.HandTipRight].Position.Z);

            //Right Leg
            rightHipPos = new Vector3(joints[JointType.HipRight].Position.X, joints[JointType.HipRight].Position.Y, joints[JointType.HipRight].Position.Z);
            /*rightKneePos = new Vector3(joints[JointType.KneeRight].Position.X, joints[JointType.KneeRight].Position.Y, joints[JointType.KneeRight].Position.Z);
            rightAnklePos = new Vector3(joints[JointType.AnkleRight].Position.X, joints[JointType.AnkleRight].Position.Y, joints[JointType.AnkleRight].Position.Z);
            rightFootPos = new Vector3(joints[JointType.FootRight].Position.X, joints[JointType.FootRight].Position.Y, joints[JointType.FootRight].Position.Z);*/

            //Left Leg
            leftHipPos = new Vector3(joints[JointType.HipLeft].Position.X, joints[JointType.HipLeft].Position.Y, joints[JointType.HipLeft].Position.Z);
            /*leftKneePos = new Vector3(joints[JointType.KneeLeft].Position.X, joints[JointType.KneeLeft].Position.Y, joints[JointType.KneeLeft].Position.Z);
            leftAnklePos = new Vector3(joints[JointType.AnkleLeft].Position.X, joints[JointType.AnkleLeft].Position.Y, joints[JointType.AnkleLeft].Position.Z);
            leftFootPos = new Vector3(joints[JointType.FootLeft].Position.X, joints[JointType.FootLeft].Position.Y, joints[JointType.FootLeft].Position.Z);*/

            float torsoHeight = Vector3.Distance(headPos, neckPos) + Vector3.Distance(neckPos, spineShoulderPos) + Vector3.Distance(spineShoulderPos, spineMidPos) + Vector3.Distance(spineMidPos, spineBasePos) + Vector3.Distance(spineBasePos, (leftHipPos + rightHipPos) / 2);
            /*float leftLegHeight = Vector3.Distance(leftHipPos, leftKneePos) + Vector3.Distance(leftKneePos, leftAnklePos) + Vector3.Distance(leftAnklePos, leftFootPos);
            float rightLegHeight = Vector3.Distance(rightHipPos, rightKneePos) + Vector3.Distance(rightKneePos, rightAnklePos) + Vector3.Distance(rightAnklePos, rightFootPos);*/
            float totalHeight = torsoHeight /*+ (leftLegHeight + rightLegHeight) / 2*/;

            if(totalHeight > 0) { //normalize skeleton size
                //Head
                headPos = scale * headPos / totalHeight + offset;
                neckPos = scale * neckPos / totalHeight + offset;

                //Spine
                spineShoulderPos = scale * spineShoulderPos / totalHeight + offset;
                spineMidPos = scale * spineMidPos / totalHeight + offset;
                spineBasePos = scale * spineBasePos / totalHeight + offset;

                //Left Arm
                leftHandPos = scale * leftHandPos / totalHeight + offset;
                leftWristPos = scale * leftWristPos / totalHeight + offset;
                leftElbowPos = scale * leftElbowPos / totalHeight + offset;
                leftShoulderPos = scale * leftShoulderPos / totalHeight + offset;
                leftThumbPos = scale * leftThumbPos / totalHeight + offset;
                leftHandTipPos = scale * leftHandTipPos / totalHeight + offset;

                //Right Arm
                rightHandPos = scale * rightHandPos / totalHeight + offset;
                rightWristPos = scale * rightWristPos / totalHeight + offset;
                rightElbowPos = scale * rightElbowPos / totalHeight + offset;
                rightShoulderPos = scale * rightShoulderPos / totalHeight + offset;
                rightThumbPos = scale * rightThumbPos / totalHeight + offset;
                rightHandTipPos = scale * rightHandTipPos / totalHeight + offset;

                //Right Leg
                rightHipPos = scale * rightHipPos / totalHeight + offset;
                /*rightKneePos = scale * rightKneePos / totalHeight;
                rightAnklePos = scale * rightAnklePos / totalHeight;
                rightFootPos = scale * rightFootPos / totalHeight;*/

                //Left Leg
                leftHipPos = scale * leftHipPos / totalHeight + offset;
                /*leftKneePos = scale * leftKneePos / totalHeight;
                leftAnklePos = scale * leftAnklePos / totalHeight;
                leftFootPos = scale * leftFootPos / totalHeight;*/



            }

        }
    }

    public void setBody(Body body) {
        this.body = body;
    }

    public ulong getId() {
        if(this.body != null) {
            return this.body.TrackingId;
        }
        return 0;
    }
}
