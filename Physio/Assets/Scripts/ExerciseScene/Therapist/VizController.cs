﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO FC - This script needs to stop using Update to manage the animations. It should instead use Invoke and coroutines. because animations only play on start
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
    public GameObject repetitionsVis;
    //public Text succ; // maybe delete   
    private int SuccPer; // helpful for radial bar
    public Image SuccSlider;
    //public Image previousSuccSlider; // maybe delete 
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
        // reset repetitions texts
        correctReps.text = "";
        totalReps.text = "" + State.exercise.getNReps();
        // scale down repVis, so we enlarge it after to create an animation
        repetitionsVis.transform.localScale = new Vector3 (0.1f,0.1f,0.1f);

        //FC - i dont use any of these
        heatButton.interactable = false;
        settingsButton.interactable = true;
        heatMapText.text = "";
        //fillPreviousSuccBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (State.space != null)
        {
            tempDist = State.space;
        }
        //BG();
        //Comp();
        //TimeViz();
        RepetitionsVis();
        //Leaned();
        //Path();
        //RestTime();
        //Shoulder();
        //Angles();
        //RepTime();

        if (State.exercise.isCompleted())
        {
            heatButton.interactable = true;
            settingsButton.interactable = false;
            panel.SetActive(false);
        }
        
        //sessionTime.transform.localScale = therapistProjection.transform.localScale;
    }

    void RepetitionsVis()
    {
        if (State.exercise.getTries() > 0 &&  State.exercise.getNReps() > 0)
        {
            // FC - Doesnt make sense for grid
            if (SceneManager.GetActiveScene().name == "Exercise3Scene") {
                SuccPer = (State.exercise.getCorrectReps() * 100) / State.exercise.getNReps();
            }
            else{
                SuccPer = (State.exercise.getCorrectReps() * 100) / State.exercise.getNReps();
            }
        }

        //Slider
        correctReps.text = State.exercise.getCorrectReps().ToString();
        SuccSlider.fillAmount = SuccPer * 0.01f;

        if (SuccPer >= 70)
        {
            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0x4F, 0xFB, 0x7B, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0x04, 0xD4, 0x86, 0xFF);
        }
        else if (SuccPer >= 30 && SuccPer < 70)
        {

            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0xF3, 0xFF, 0x24, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0xD4,0xCE,0x04, 0xFF);
        }
        else
        {
            //SuccSlider.color = Color.Lerp(SImage.color, new Color32(0xF9, 0x53, 0x53, 0xFF), Mathf.PingPong(Time.time, 1));
            SuccSlider.color = new Color32(0xD4,0x78,0x04, 0xFF);
        }


        switch (tempDist  /*TherapistPatientTracker.GetInterDist()*/)
        {
            case "Personal":
                repetitionsVis.transform.localScale = Vector3.Slerp(repetitionsVis.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //repetitionsVis.transform.localPosition = Vector3.Slerp(repetitionsVis.transform.localPosition, new Vector3(261.0f, 218.0f, 0.0f), Time.deltaTime * 2);
                
                break; 
            case "Social": 
                repetitionsVis.transform.localScale = Vector3.Slerp(repetitionsVis.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * 2);
                //repetitionsVis.transform.localScale = Vector3.Slerp(repetitionsVis.transform.localScale, new Vector3(.9f, .9f, .9f), Time.deltaTime * 2);
                //repetitionsVis.transform.localPosition = Vector3.Slerp(repetitionsVis.transform.localPosition, new Vector3(265f, 190.0f, 0.0f), Time.deltaTime * 2);
               
                break;
            case "Intimate":

                break;
            case "Public":

                break;
            case "ERROR":

                break;
        }
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

    // This method would paint the circles of the human image, with the shoulder and spine circles
    // TODO: Now, we only to show this image in result screens
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

    // FC - Does nothing because I commented
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
        //previousSuccSlider.fillAmount = succPer * 0.01f;
        //previousSuccSlider.color = new Color32(0x80, 0x80, 0x80, 0xFF);
    }

    public void togglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

}
