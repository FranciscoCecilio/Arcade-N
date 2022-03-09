﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BodyScreenController : MonoBehaviour {

    public GameObject startButton;
    public GameObject stopButton;
    public GameObject quitButton;
    public GameObject restartButton;
    public GameObject nextButton;
    public GameObject patientCanvas;

    public Text leaned;
    public Text shoulderLift;
    public Text outOfPath;
    public Text exerciseName;

    public Camera worldCamera;

    private bool hasWroteReport;

    public void StartTherapy() {
        if (SceneManager.GetActiveScene().name == "Exercise1Scene" || SceneManager.GetActiveScene().name == "Exercise2Scene")
        {
            saveTargetPositions();
            savePathPosition();
        }
        startButton.SetActive(false);
        stopButton.SetActive(true);
        patientCanvas.SetActive(true);
    }

    public void StopTherapy() {
        stopButton.SetActive(false);
        State.exercise.setCompleted(true);
        State.isTherapyOnGoing = false;
        if (SessionInfo.toView() == "RunSequence") nextButton.SetActive(true);
        else
        {
            quitButton.SetActive(true);
            restartButton.SetActive(true);
        }
    }

    public void Quit() {
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void Restart()
    {
        State.exercise.restart();
        State.resetState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next()
    {
        SequenceManager.nextExercise();
    }

    public void saveTargetPositions()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
        Vector3[] targetPositions = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targetPositions[i] = worldCamera.WorldToScreenPoint(targets[i].transform.position);
        }
        State.exercise.saveTargetPositions(targetPositions);
    }

    public void savePathPosition()
    {
        GameObject path = GameObject.FindGameObjectWithTag("ExerciseCollider");
        State.exercise.savePathPosition(worldCamera.WorldToScreenPoint(path.transform.position));
    }

    public void Update() {
        leaned.text = "" + State.exercise.getSpineComp();
        shoulderLift.text = "" + (State.exercise.getLeftShoulderComp() + State.exercise.getRightShoulderComp());
        outOfPath.text = "" + State.exercise.getOutOfPath();
        exerciseName.text = State.exercise.getName();

        if(State.exercise.isCompleted()) {
            StopTherapy();
        }
    }
}