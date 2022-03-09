using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompensationMatrix : MonoBehaviour {

    public BodyWrapper patient;

    public Camera compCam;
    public Camera ther;

    public GameObject leftHip; 
    public GameObject rightHip;

    public GameObject leftShoulder;
    public GameObject rightShoulder;

    public GameObject spineBase;
    public GameObject spineMid;
    public GameObject spineShoulder;

    public GameObject shoulderBar;
    public GameObject spineBar;

    public GameObject shoulderToggle;
    public GameObject spineToggle;

    public GameObject shoulderSlider;
    public GameObject spineSlider;

    public GameObject shoulderArrow;
    public GameObject spineLeftArrow;
    public GameObject spineRightArrow;

    public GameObject shoulders;
    public GameObject spine;

    private bool hasRegistredShoulderCompensation;
    private bool hasRegistredSpineCompensation;

    public Toggle incRepToggle;

    private float arrowOffset = 0.1f;

    // Use this for initialization
    void Start () {
        shoulderToggle.GetComponent<Toggle>().isOn = State.registerShoulderComp;
        spineToggle.GetComponent<Toggle>().isOn = State.registerSpineComp;
	}
	
    private void setBarColorToRed(GameObject bar) {
        bar.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
    }

    private void setBarColorToGreen(GameObject bar) {
        bar.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
    }

    // Update is called once per frame
    void Update () {
        if (leftHip.transform.position == rightHip.transform.position)
            return;

        if (!State.exercise.isCompleted())
        {
            //shoulder
            bool shoulderToggleIsOn = shoulderToggle.GetComponent<Toggle>().isOn;
            if (shoulderToggleIsOn) State.registerShoulderComp = true;
            else State.registerShoulderComp = false;
            if (shoulderToggleIsOn)
            {
                shoulderBar.transform.position = new Vector3(spineBase.transform.position.x, spineShoulder.transform.position.y, shoulderBar.transform.position.z);
                //shoulderToggle.transform.position = new Vector3(shoulderToggle.transform.position.x, shoulderBar.transform.position.y, shoulderToggle.transform.position.z);

                bool hasShoulderCompensation = Math.Abs(leftShoulder.transform.position.y - rightShoulder.transform.position.y) > shoulderSlider.GetComponent<Slider>().value;
                if (hasShoulderCompensation)
                {
                    State.compensationInCurrentRep = true;

                    setBarColorToRed(shoulderBar);
                    shoulderArrow.SetActive(true);

                    if (leftShoulder.transform.position.y > rightShoulder.transform.position.y)
                    {
                        if (!hasRegistredShoulderCompensation && State.isTherapyOnGoing)
                        {
                            State.exercise.incLeftShoulderComp();
                            State.exercise.incTries();
                            //DrawLine(patient.spineBasePos, patient.spineMidPos, new Color(1, 0, 0, 1));
                            //DrawCompSkeleton(patient, new Color(1, 0, 0, 1), "lshoulder");
                        }
                        shoulderArrow.transform.position = new Vector3(leftShoulder.transform.position.x - arrowOffset, leftShoulder.transform.position.y + arrowOffset, leftShoulder.transform.position.z);
                    }
                    else
                    {
                        if (!hasRegistredShoulderCompensation && State.isTherapyOnGoing)
                        {
                            State.exercise.incRightShoulderComp();
                            State.exercise.incTries();
                            //DrawCompSkeleton(patient, new Color(0, 0, 1, 1), "rshoulder");
                        }
                        shoulderArrow.transform.position = new Vector3(rightShoulder.transform.position.x + arrowOffset, rightShoulder.transform.position.y + arrowOffset, rightShoulder.transform.position.z);
                    }
                    hasRegistredShoulderCompensation = true;
                }
                else
                {
                    setBarColorToGreen(shoulderBar);
                    shoulderArrow.SetActive(false);
                    hasRegistredShoulderCompensation = false;
                }
            }

            //spine
            bool spineToggleIsOn = spineToggle.GetComponent<Toggle>().isOn;
            if (spineToggleIsOn) State.registerSpineComp = true;
            else State.registerSpineComp = false;
            if (spineToggleIsOn)
            {
                spineBar.transform.position = new Vector3(spineBase.transform.position.x, spineMid.transform.position.y, spineBar.transform.position.z);
                //spineToggle.transform.position = new Vector3(spineBar.transform.position.x, spineToggle.transform.position.y, spineToggle.transform.position.z);

                bool hasSpineCompensation = Math.Abs(spineShoulder.transform.position.x - spineBase.transform.position.x) > spineSlider.GetComponent<Slider>().value;
                if (hasSpineCompensation)
                {
                    State.compensationInCurrentRep = true;

                    setBarColorToRed(spineBar);

                    if (spineShoulder.transform.position.x > spineBase.transform.position.x)
                    {
                        if (!hasRegistredSpineCompensation && State.isTherapyOnGoing)
                        {
                            State.exercise.incSpineComp();
                            State.exercise.incTries();
                            //DrawCompSkeleton(patient, new Color(0, 1, 0, 1), "spine");
                        }
                        spineRightArrow.SetActive(true);
                        spineRightArrow.transform.position = new Vector3(spineBar.transform.position.x + arrowOffset, spineBar.transform.position.y, spineBar.transform.position.z);

                    }
                    else
                    {
                        if (!hasRegistredSpineCompensation && State.isTherapyOnGoing)
                        {
                            State.exercise.incSpineComp();
                            State.exercise.incTries();
                            //DrawCompSkeleton(patient, new Color(0, 1, 0, 1), "spine");
                        }
                        spineLeftArrow.SetActive(true);
                        spineLeftArrow.transform.position = new Vector3(spineBar.transform.position.x - arrowOffset, spineBar.transform.position.y, spineBar.transform.position.z);
                    }
                    hasRegistredSpineCompensation = true;

                }
                else
                {
                    setBarColorToGreen(spineBar);
                    spineLeftArrow.SetActive(false);
                    spineRightArrow.SetActive(false);
                    hasRegistredSpineCompensation = false;
                }
            }
        }

        else
        {
            shoulderBar.SetActive(false);
            spineBar.SetActive(false);
            shoulderToggle.SetActive(false);
            spineToggle.SetActive(false);
            shoulderArrow.SetActive(false);
            spineLeftArrow.SetActive(false);
            spineRightArrow.SetActive(false);
        }
    }

    public void shoulderToggleBar() {
        shoulders.SetActive(shoulderToggle.GetComponent<Toggle>().isOn);
    }

    public void spineToggleBar() {
        spine.SetActive(spineToggle.GetComponent<Toggle>().isOn);
    }


    public void DrawCompSkeleton(BodyWrapper patient, Color color, String tag)
    {
        DrawLine(patient.headPos, patient.neckPos, color, tag);
        DrawLine(patient.neckPos, patient.spineShoulderPos, color, tag);
        DrawLine(patient.spineShoulderPos, patient.spineBasePos, color, tag);
        DrawLine(patient.spineMidPos, patient.spineBasePos, color, tag);
        DrawLine(patient.spineShoulderPos, patient.spineMidPos, color, tag);
        DrawLine(patient.spineShoulderPos, patient.leftShoulderPos, color, tag);
        DrawLine(patient.leftShoulderPos, patient.leftElbowPos, color, tag);
        DrawLine(patient.leftElbowPos, patient.leftWristPos, color, tag);
        DrawLine(patient.leftWristPos, patient.leftHandTipPos, color, tag);
        //DrawLine(patient.leftHandPos, patient.leftHandTipPos, color, tag);
        //DrawLine(patient.leftWristPos, patient.leftThumbPos, color, tag);
        DrawLine(patient.spineBasePos, patient.leftHipPos, color, tag);

        DrawLine(patient.spineShoulderPos, patient.rightShoulderPos, color, tag);
        DrawLine(patient.rightShoulderPos, patient.rightElbowPos, color, tag);
        DrawLine(patient.rightElbowPos, patient.rightWristPos, color, tag);
        DrawLine(patient.rightWristPos, patient.rightHandTipPos, color, tag);
        //DrawLine(patient.rightHandPos, patient.rightHandTipPos, color, tag);
        //DrawLine(patient.rightWristPos, patient.rightThumbPos, color, tag);
        DrawLine(patient.spineBasePos, patient.rightHipPos, color, tag);

    }






    public void DrawLine(Vector3 start, Vector3 end, Color color /*,float duration = 0.2f*/, string tag)
    {
        GameObject myLine = new GameObject();
        myLine.tag = tag;
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //lr.SetColors(color, color);
        lr.startColor = color;
        lr.endColor = color;
        //lr.SetWidth(0.01f, 0.01f);
        lr.startWidth = 0f;
        lr.endWidth = 0f;
        //lr.SetWidth(0f, 0f);
        lr.SetPosition(0, start + new Vector3(0, 0, -6));
        lr.SetPosition(1, end + new Vector3(0, 0, -6));
        compCam.transform.localPosition =   new Vector3(ther.transform.localPosition.x + 6, ther.transform.localPosition.y, ther.transform.localPosition.z);
        
        //GameObject.Destroy(myLine, duration);
    }
}
