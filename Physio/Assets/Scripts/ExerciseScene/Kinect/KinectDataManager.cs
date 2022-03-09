using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectDataManager : MonoBehaviour {

    private KinectData kinectData;

    public BodyWrapper therapist;
    public BodyWrapper patient;
    public BodyWrapper temp;
    public Vector3 therapistPos;
    public float closeDist = 100f; 

	// Use this for initialization
	void Start () {
        kinectData = new KinectData();
        kinectData.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
            Application.Quit();

        List<Body> activeBodies = kinectData.getActiveBodies();

        if (activeBodies.Count == 1) {
            patient.setBody(activeBodies[0]);
            //State.space = "Social";
            //Debug.Log(patient.spineBasePos);

        }

        if (activeBodies.Count == 2)
        {
            for (int j = 0; j < activeBodies.Count; j++)
            {
                Body body = activeBodies[j];
                temp.setBody(body);

                if (body.TrackingId == patient.getId())
                {
                    continue;
                }
                else
                {
                    therapist.setBody(body);
                    therapistPos = therapist.spineBasePos;
                }
            }
            /*
            // para o heatmap 

            var distMEsquerdaOEsquerda = Vector3.Distance(therapist.leftHandTipPos, patient.leftShoulderPos);
            var distMDireitaOEsquerda = Vector3.Distance(therapist.rightHandTipPos, patient.leftShoulderPos);

            var distMEsquerdaEEsquerda = Vector3.Distance(therapist.leftHandTipPos, patient.leftElbowPos);
            var distMDireitaEEsquerda = Vector3.Distance(therapist.rightHandTipPos, patient.leftElbowPos);

            var distMEsquerdaMEsquerda = Vector3.Distance(therapist.leftHandTipPos, patient.leftHandPos);
            var distMDireitaMEsquerda = Vector3.Distance(therapist.rightHandTipPos, patient.leftHandPos);


            var distMEsquerdaODireita = Vector3.Distance(therapist.leftHandTipPos, patient.rightShoulderPos);
            var distMDireitaODireita = Vector3.Distance(therapist.rightHandTipPos, patient.rightShoulderPos);

            var distMEsquerdaEDireita = Vector3.Distance(therapist.leftHandTipPos, patient.rightElbowPos);
            var distMDireitaEDireita = Vector3.Distance(therapist.rightHandTipPos, patient.rightElbowPos);

            var distMEsquerdaMDireita = Vector3.Distance(therapist.leftHandTipPos, patient.rightHandPos);
            var distMDireitaMDireita = Vector3.Distance(therapist.rightHandTipPos, patient.rightHandPos);

            var distTherapisthands = Vector3.Distance(therapist.rightHandPos, therapist.leftHandPos);
            
            if (distMEsquerdaOEsquerda < 0.3 || distMDireitaOEsquerda < 0.3)
            {
                State.touch = "leftshoulder";
            }
            else if (distMEsquerdaEEsquerda < 0.3 || distMDireitaEEsquerda <0.3)
            {
                State.touch = "leftelbow";
            }
            else if (distMEsquerdaMEsquerda < 0.3 || distMDireitaMEsquerda < 0.3)
            {
                State.touch = "lefthand";
            }
            else if (distMEsquerdaODireita < 0.3 || distMDireitaODireita < 0.3)
            {
                State.touch = "rightshoulder";
            }
            else if (distMEsquerdaEDireita < 0.3 || distMDireitaEDireita < 0.3)
            {
                State.touch = "rightelbow";
            }
            else if (distMEsquerdaMDireita < 0.3 || distMDireitaMDireita < 0.3)
            {
                State.touch = "righthand";
            }
            else if (distTherapisthands < 0.1)
            { State.touch = "none"; }

            */

        }

        if ( activeBodies.Count > 2)
        {
            for(int j = 0; j < activeBodies.Count; j++)
            {
                Body body = activeBodies[j];
                temp.setBody(body);
                if (body.TrackingId == patient.getId())
                {
                    continue;
                }

                if (Vector3.Distance(temp.spineBasePos, therapist.spineBasePos) < 0.2)
                {
                    therapist.setBody(body);
                    therapistPos = therapist.spineBasePos;
                }
                else if (closeDist < Vector3.Distance(new Vector3(0f,0f,0f), temp.spineBasePos))
                {
                    closeDist = Vector3.Distance(new Vector3(0f, 0f, 0f), temp.spineBasePos);
                    therapist.setBody(body);
                    therapistPos = therapist.spineBasePos;

                }
               
            }
        }

        /*if (activeBodies.Count > 1)
        {
            var interDist = Vector3.Distance(patient.spineBasePos, therapist.spineBasePos);
            //Debug.Log( "interdist " +  interDist);

            if ( interDist > 0.9 && interDist < 1.7)
            {
                State.space = "Personal";
            }
            else if ( interDist <= 0.9)
            {
                State.space = "Intimate";
            }
            else
            {
                State.space = "Social";
            }
        }*/

        if (Time.frameCount % 30 == 0) {
            //System.GC.Collect();
        }
        //Debug.Log(kinectData.getActiveBodies().Count);

        //State.space = "Intimate";
    }

    void OnApplicationQuit() {
        kinectData.Close();
        if (!Application.isEditor)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
