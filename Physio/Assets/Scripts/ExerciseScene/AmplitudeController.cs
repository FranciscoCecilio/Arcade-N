using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmplitudeController : MonoBehaviour {

    public BodyWrapper patient;
    public float maxSXY = 0;
    public float maxSXZ = 0;
    public float maxE = 0;
    public float minSXY = 360;
    public float minSXZ = 360;
    public float minE = 360;
    public Image XYmax;
    public Image XYCurrent;
    public Image XYmin;
    public Image XZmax;
    public Image XZCurrent;
    public Image XZmin;
    public Image Emax;
    public Image ECurrent;
    public Image Emin;

    public Text maxElbow;
    public Text maxSup;
    public Text maxSdown;
    public Text minElbow;
    public Text minSup;
    public Text minSdown;
    public Text cuElbow;
    public Text cuSup;
    public Text cuSdown;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (State.isTherapyOnGoing)
        {
            ShoulderXY();
            ShoulderXZ();
            Elbow();
        }

       
	}



    void Elbow()
    {
        if (State.leftArmSelected)
        {
            AngleBetweenTwoVectors(patient.leftElbowPos - patient.leftShoulderPos, patient.leftElbowPos - patient.leftWristPos, "elbow");
        }
        else
        {
            AngleBetweenTwoVectors(patient.rightElbowPos - patient.rightShoulderPos, patient.rightElbowPos - patient.rightWristPos, "elbow");
        }
        
    }

    void ShoulderXY()
    {
        Vector2 spineshoulder = new Vector2(patient.spineShoulderPos.x, patient.spineShoulderPos.y);
        Vector2 leftshoulder = new Vector2(patient.leftShoulderPos.x, patient.leftShoulderPos.y);
        Vector2 leftelbow = new Vector2(patient.leftElbowPos.x, patient.leftElbowPos.y);
        Vector2 spineBase = new Vector2(patient.spineBasePos.x, patient.spineBasePos.y);
        Vector2 rightshoulder = new Vector2(patient.rightShoulderPos.x, patient.rightShoulderPos.y);
        Vector2 rightelbow = new Vector2(patient.rightElbowPos.x, patient.rightElbowPos.y);

        if (State.leftArmSelected)
        {
            AngleBetweenTwoVectors(spineshoulder - spineBase, leftshoulder - leftelbow, "XY");
        }
        else
        {
            AngleBetweenTwoVectors(spineshoulder - spineBase, rightshoulder - rightelbow, "XY");
        }

        




    }

    void ShoulderXZ()
    {
        Vector2 spineshoulder = new Vector2(patient.spineShoulderPos.x, patient.spineShoulderPos.z);
        Vector2 leftshoulder = new Vector2(patient.leftShoulderPos.x, patient.leftShoulderPos.z);
        Vector2 leftelbow = new Vector2(patient.leftElbowPos.x, patient.leftElbowPos.z);
        Vector2 rightshoulder = new Vector2(patient.rightShoulderPos.x, patient.rightShoulderPos.z);
        Vector2 rightelbow = new Vector2(patient.rightElbowPos.x, patient.rightElbowPos.z);

        if (State.leftArmSelected)
        {
            AngleBetweenTwoVectors(leftshoulder - spineshoulder, leftshoulder - leftelbow, "XZ");
        }
        else
        {
            AngleBetweenTwoVectors(rightshoulder - spineshoulder, rightshoulder - rightelbow, "XZ");
        }

        
    }



    public void AngleBetweenTwoVectors(Vector3 vectorA, Vector3 vectorB, string pos)
    {
        float dotProduct = 0.0f;
        vectorA.Normalize();
        vectorB.Normalize();
        dotProduct = Vector3.Dot(vectorA, vectorB);

        float angle = Mathf.Acos(dotProduct) / Mathf.PI * 180;

         float Cangle = (1 * angle) / 360;


        switch (pos)
        {
            case "XY":

                if (Cangle > maxSXY)
                {
                    XYmax.fillAmount = Cangle;
                    maxSXY = Cangle;
                    maxSup.text = angle.ToString("F2") + "º"; 
                }
                else if(Cangle < minSXY)
                {
                    XYmin.fillAmount = Cangle;
                    minSXY = Cangle;
                    minSup.text = angle.ToString("F2") + "º";
                }
                XYCurrent.fillAmount = Cangle;
                cuSup.text = angle.ToString("F2") + "º";



                break;
            case "XZ":
                if (Cangle > maxSXZ)
                {
                    XZmax.fillAmount = Cangle;
                    maxSXZ = Cangle;
                    maxSdown.text = angle.ToString("F2") + "º";
                }
                else if (Cangle < minSXZ)
                {
                    XZmin.fillAmount = Cangle;
                    minSXZ = Cangle;
                    minSdown.text = angle.ToString("F2") + "º";
                }
                XZCurrent.fillAmount = Cangle;
                cuSdown.text = angle.ToString("F2") + "º";
                break;
            case "elbow":
                if (Cangle > maxE)
                {
                    Emax.fillAmount = Cangle;
                    maxE = Cangle;
                    maxElbow.text = angle.ToString("F2") + "º";
                }
                else if (Cangle < minE)
                {
                    Emin.fillAmount = Cangle;
                    minE = Cangle;
                    minElbow.text = angle.ToString("F2") + "º";
                }
                ECurrent.fillAmount = Cangle;
                cuElbow.text = angle.ToString("F2") + "º";
                break;

        }






  
    }




}
