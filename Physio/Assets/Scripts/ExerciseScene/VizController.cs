using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VizController : MonoBehaviour
{
    public Camera worldCamera;

    [Header("Information for Therapists")]
    public GameObject shoulderLift;
    public GameObject outOfPath;
    public GameObject leaned;
    //public GameObject therapistProjection; // Delete
    public GameObject leftBG;
    public GameObject rightBG;

    [Header("Timers")]
    public GameObject sessionTime;
    public GameObject restTime;
    public GameObject repTime;
    public GameObject last;

    [Header("Repetitions Visualization")]
    public GameObject success;
    public Text succ; // maybe delete   
    private int SuccPer; // helpful for radial bar
    public Image SuccSlider;
    public Image previousSuccSlider; // maybe delete 
    public Text correctReps; // incremented after a successful movement
    public Text totalReps;

    [Header("Bottom Right")]
    public Button settingsButton;
    public GameObject panel;
    // Switch View and HeatMap - Maybe Delete
    public Button heatButton;
    public Text heatMapText;

    [Header("Angle Viualization - Delete")]
    public GameObject angles;
    public GameObject AmpData;
    public GameObject AmpLeft;
    public GameObject Ampright;
    public string tempDist = "Social";

    [Header("Compensatory Stuff - Delete")]
    public GameObject compMov;
    public GameObject CompText;

    public Image LsholderImange;
    public Image RshoulderImage;
    public Image SImage;

    public Text leftS;
    public Text rightS;
    public Text Spine;



    // Use this for initialization
    void Start()
    {
        correctReps.text = "";
        totalReps.text = "" + State.exercise.getNReps();
        heatButton.interactable = false;
        settingsButton.interactable = true;
        heatMapText.text = "";
        //fillPreviousSuccBar();
        positionElements();
    }

    // Update is called once per frame
    void Update()
    {
        if (State.space != null)
        {
            tempDist = State.space;
        }
        BG();
        Comp();
        TimeViz();
        Success();
        Leaned();
        Path();
        //RestTime();
        Shoulder();
        Angles();
        RepTime();

        if (State.exercise.isCompleted())
        {
            heatButton.interactable = true;
            settingsButton.interactable = false;
            panel.SetActive(false);
        }
        
        //sessionTime.transform.localScale = therapistProjection.transform.localScale;
    }

    public void switchView()
    {
        if (State.space.Equals("Social")) State.space = "Personal";
        else if (State.space.Equals("Personal")) State.space = "Intimate";
        else if (State.space.Equals("Intimate")) State.space = "Social";
    }

    public void switchHeatMap()
    {
        if (State.heatM.Equals("none"))
        {
            State.heatM = "hand";
            heatMapText.text = "Hand";
        }
        else if (State.heatM.Equals("hand")) {
            State.heatM = "elbow";
            heatMapText.text = "Elbow";
        }
        else if (State.heatM.Equals("elbow"))
        {
            State.heatM = "shoulder";
            heatMapText.text = "Shoulder";
        }
        else if (State.heatM.Equals("shoulder"))
        {
            State.heatM = "none";
            heatMapText.text = "";
        }
    }


    void Comp()
    {
        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                compMov.transform.localScale = Vector3.Slerp(compMov.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //compMov.transform.localPosition = Vector3.Slerp(compMov.transform.localPosition, new Vector3(-406.0f, 24.0f, 0.0f), Time.deltaTime * 2);
                CompText.transform.localScale = Vector3.Slerp(CompText.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);

                break;
            case "Social":
                compMov.transform.localScale = Vector3.Slerp(angles.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);
                CompText.transform.localScale = Vector3.Slerp(CompText.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);


                break;
            case "Intimate":
                CompText.transform.localScale = Vector3.Slerp(CompText.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 2);

                break;
            case "Public":

                break;
            case "ERROR":
          
                break;
        }
    }

    void RepTime()
    {
        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                repTime.transform.localScale = Vector3.Slerp(repTime.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                repTime.transform.localPosition = Vector3.Slerp(repTime.transform.localPosition, new Vector3(-420.0f, -203.0f, 0.0f), Time.deltaTime * 2);
                last.transform.localScale = Vector3.Slerp(last.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);
                break;
            case "Social":
                repTime.transform.localScale = Vector3.Slerp(repTime.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);
                last.transform.localScale = Vector3.Slerp(last.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);


                break;
            case "Intimate":
                last.transform.localScale = Vector3.Slerp(last.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 2);
                break;
            case "Public":
                
                break;
            case "ERROR":
                
                break;
        }
    }

    void Angles()
    {
        if (State.leftArmSelected)
        {
            AmpLeft.SetActive(true);
            Ampright.SetActive(false);
        }
        else
        {
            AmpLeft.SetActive(false);
            Ampright.SetActive(true);
        }


        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                angles.transform.localScale = Vector3.Slerp(angles.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                angles.transform.localPosition = Vector3.Slerp(angles.transform.localPosition, new Vector3(-412.0f, -12.0f, 0.0f), Time.deltaTime * 2);
                AmpData.transform.localScale = Vector3.Slerp(AmpData.transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 2);

                break;
            case "Social":
                angles.transform.localScale = Vector3.Slerp(angles.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);
                AmpData.transform.localScale = Vector3.Slerp(AmpData.transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 2);


                break;
            case "Intimate":
                AmpData.transform.localScale = Vector3.Slerp(AmpData.transform.localScale, new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime * 2);

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }
    }

    void Success()
    {
        if (State.exercise.getTries() != 0)
        {
            if (SceneManager.GetActiveScene().name == "Exercise3Scene") SuccPer = (State.exercise.getCorrectReps() * 100) / State.exercise.getNReps();
            else SuccPer = (State.exercise.getCorrectReps() * 100) / State.exercise.getTries();
        }
        

        //succ.text = "" + SuccPer + "%";
        correctReps.text = State.exercise.getCorrectReps().ToString();
        SuccSlider.fillAmount = SuccPer * 0.01f;

        if (SuccPer >= 70)
        {
            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0x4F, 0xFB, 0x7B, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0x4F, 0xFB, 0x7B, 0xFF);
        }
        else if (SuccPer >= 30 && SuccPer < 70)
        {

            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0xF3, 0xFF, 0x24, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0xF3, 0xFF, 0x24, 0xFF);
        }
        else
        {
            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0xF9, 0x53, 0x53, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0xF9, 0x53, 0x53, 0xFF);
        }


        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                success.transform.localScale = Vector3.Slerp(success.transform.localScale, new Vector3(0.6f, 0.6f, 0.6f), Time.deltaTime * 2);
                //success.transform.localPosition = Vector3.Slerp(success.transform.localPosition, new Vector3(261.0f, 218.0f, 0.0f), Time.deltaTime * 2);
                
                break; 
            case "Social": 
                success.transform.localScale = Vector3.Slerp(success.transform.localScale, new Vector3(0.6f, 0.6f, 0.6f), Time.deltaTime * 2);
                //success.transform.localScale = Vector3.Slerp(success.transform.localScale, new Vector3(.9f, .9f, .9f), Time.deltaTime * 2);
                //success.transform.localPosition = Vector3.Slerp(success.transform.localPosition, new Vector3(265f, 190.0f, 0.0f), Time.deltaTime * 2);
               
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }
    }



    void TimeViz()
    {
        switch ( tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                sessionTime.transform.localScale = Vector3.Slerp(sessionTime.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                sessionTime.transform.localPosition = Vector3.Slerp(sessionTime.transform.localPosition, new Vector3(-420.0f, 215.0f, 0.0f), Time.deltaTime * 2);
                restTime.transform.localScale = Vector3.Slerp(restTime.transform.localScale, new Vector3(.5f, .5f, .5f), Time.deltaTime * 2);
                break;
            case "Social":
                //sessionTime.transform.localScale = Vector3.Slerp(sessionTime.transform.localScale, new Vector3(.9f, .9f, .9f), Time.deltaTime * 2);
                //sessionTime.transform.localPosition = Vector3.Slerp(sessionTime.transform.localPosition, new Vector3(-420f, 210.0f, 0.0f), Time.deltaTime * 2);
                //restTime.transform.localScale = Vector3.Slerp(restTime.transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 2);
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }


    }

    void Leaned()
    {

        Spine.text = "" + State.exercise.getSpineComp();

        if (State.registerSpineComp)
        {
            if (State.exercise.getSpineComp() > 10)
            {
                SImage.color = Color.Lerp(SImage.color, new Color32(0xF9, 0x53, 0x53, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else if (State.exercise.getSpineComp() <= 10 && State.exercise.getSpineComp() > 5)
            {
                SImage.color = Color.Lerp(SImage.color, new Color32(0xF3, 0xFF, 0x24, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else
            {
                SImage.color = Color.Lerp(SImage.color, new Color32(0x4F, 0xFB, 0x7B, 0xFF), Mathf.PingPong(Time.time, 1));
            }
        } else
        {
            SImage.color = Color.Lerp(SImage.color, new Color32(0x80, 0x80, 0x80, 0x80), Mathf.PingPong(Time.time, 1));
        }
        


        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                leaned.transform.localScale = Vector3.Slerp(leaned.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //leaned.transform.localPosition = Vector3.Slerp(leaned.transform.localPosition, new Vector3(423f, 174.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Social":
                //leaned.transform.localScale = Vector3.Slerp(leaned.transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }


    }

    void Path()
    {
        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                outOfPath.transform.localScale = Vector3.Slerp(outOfPath.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //outOfPath.transform.localPosition = Vector3.Slerp(outOfPath.transform.localPosition, new Vector3(107f, 174.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Social":
                //outOfPath.transform.localScale = Vector3.Slerp(outOfPath.transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }


    }
    void BG()
    {
        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                leftBG.SetActive(true);
                rightBG.SetActive(true);
                break;
            case "Social":
                //leftBG.SetActive(false);
                //rightBG.SetActive(false);
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }
    }


    void Shoulder()
    {

        leftS.text = "" + (State.exercise.getLeftShoulderComp());
        rightS.text = "" + (State.exercise.getRightShoulderComp());
        
        if (State.registerShoulderComp)
        {
            if (State.exercise.getLeftShoulderComp() > 10)
            {
                LsholderImange.color = Color.Lerp(LsholderImange.color, new Color32(0xF9, 0x53, 0x53, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else if (State.exercise.getLeftShoulderComp() <= 10 && State.exercise.getLeftShoulderComp() > 5)
            {
                LsholderImange.color = Color.Lerp(LsholderImange.color, new Color32(0xF3, 0xFF, 0x24, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else
            {
                LsholderImange.color = Color.Lerp(LsholderImange.color, new Color32(0x4F, 0xFB, 0x7B, 0xFF), Mathf.PingPong(Time.time, 1));
            }

            if (State.exercise.getRightShoulderComp() > 10)
            {
                RshoulderImage.color = Color.Lerp(RshoulderImage.color, new Color32(0xF9, 0x53, 0x53, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else if (State.exercise.getRightShoulderComp() <= 10 && State.exercise.getRightShoulderComp() > 5)
            {
                RshoulderImage.color = Color.Lerp(RshoulderImage.color, new Color32(0xF3, 0xFF, 0x24, 0xFF), Mathf.PingPong(Time.time, 1));
            }
            else
            {
                RshoulderImage.color = Color.Lerp(RshoulderImage.color, new Color32(0x4F, 0xFB, 0x7B, 0xFF), Mathf.PingPong(Time.time, 1));
            }
        } else
        {
            RshoulderImage.color = Color.Lerp(RshoulderImage.color, new Color32(0x80, 0x80, 0x80, 0x80), Mathf.PingPong(Time.time, 1));
            LsholderImange.color = Color.Lerp(LsholderImange.color, new Color32(0x80, 0x80, 0x80, 0x80), Mathf.PingPong(Time.time, 1));
        }
        




        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                shoulderLift.transform.localScale = Vector3.Slerp(shoulderLift.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //shoulderLift.transform.localPosition = Vector3.Slerp(shoulderLift.transform.localPosition, new Vector3(306f, 174.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Social":
                //shoulderLift.transform.localScale = Vector3.Slerp(shoulderLift.transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 2);
                break;
            case "Intimate":
                shoulderLift.transform.localScale = Vector3.Slerp(shoulderLift.transform.localScale, new Vector3(0.4f, 0.4f, 0.4f), Time.deltaTime * 2);
                break;
            case "Public":
                shoulderLift.transform.localScale = Vector3.Slerp(shoulderLift.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime * 2);
                break;
            case "ERROR":
                shoulderLift.transform.localScale = Vector3.Slerp(shoulderLift.transform.localScale, new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime * 2);
                break;
        }


    }

    public void togglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    private void fillPreviousSuccBar()
    {
        string _arm = "";
        if (State.exercise.isLeftArm()) _arm = "left";
        else _arm = "right";

        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/" + _arm + "/" + SceneManager.GetActiveScene().name + ".txt";
        int succPer = 0;
        int correctReps = 0;
        int tries = 0;
        int nReps = 0;

        if (System.IO.File.Exists(filepath))
        {
            string line = "";
            StreamReader reader = new StreamReader(filepath);
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split('=');
                    if (data[0] == "correctReps") correctReps = Int32.Parse(data[1]);
                    else if (data[0] == "tries") tries = Int32.Parse(data[1]);
                    else if (data[0] == "nrReps") nReps = Int32.Parse(data[1]);
                    line = reader.ReadLine();
                }
            }
            if (SceneManager.GetActiveScene().name == "Exercise3Scene") succPer = (correctReps * 100) / nReps;
            else succPer = (correctReps * 100) / tries;
        }
        previousSuccSlider.fillAmount = succPer * 0.01f;
        previousSuccSlider.color = new Color32(0x80, 0x80, 0x80, 0xFF);
    }

    private void positionElements()
    {
        string scene = SceneManager.GetActiveScene().name;
        if ( scene == "Exercise1Scene" || scene == "Exercise2Scene")
        {
            string _arm = "";
            if (State.exercise.isLeftArm()) _arm = "left";
            else _arm = "right";

            string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/" + scene + ".txt";

            if (System.IO.File.Exists(filepath))
            {
                string line = "";
                StreamReader reader = new StreamReader(filepath);
                {
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split('=');
                        if (data[0] == "arm")
                        {
                            if (!data[1].Equals(_arm)) break;
                        }
                        if (data[0] == "pathPosition")
                        {
                            Vector3 pathPosition = StringToVector3(data[1]);
                            GameObject path = GameObject.FindGameObjectWithTag("ExerciseCollider");
                            path.transform.position = worldCamera.ScreenToWorldPoint(pathPosition);
                        }
                        else if (data[0] == "target0")
                        {
                            Vector3 target1Position = StringToVector3(data[1]);
                            GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
                            targets[0].transform.position = worldCamera.ScreenToWorldPoint(target1Position);
                        }
                        else if (data[0] == "target1")
                        {
                            Vector3 target2Position = StringToVector3(data[1]);
                            GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
                            targets[1].transform.position = worldCamera.ScreenToWorldPoint(target2Position);
                        }
                        line = reader.ReadLine();
                    }
                }
            }
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }
        sVector = sVector.Replace(" ", string.Empty);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0].Replace(".", ",")),
            float.Parse(sArray[1].Replace(".", ",")),
            float.Parse(sArray[2].Replace(".", ",")));

        return result;
    }

}
