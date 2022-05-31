using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BodyScreenController : MonoBehaviour {

    public GameObject startButton;
    public GameObject startStuff;

    public GameObject pauseButton;
    public GameObject unPauseButton;
    public GameObject pauseStuff;
    public GameObject quitButton;
    public GameObject restartButton;

    public GameObject nextButton;
    public GameObject patientCanvas;

    public TMP_Text leaned;
    public TMP_Text shoulderLift;
    public TMP_Text outOfPath;
    public TMP_Text exerciseName;

    public Camera worldCamera;

    private bool hasWroteReport;

    void Start()
    {
        startStuff.SetActive(true);
    }
    public void StartTherapy() {
        if (SceneManager.GetActiveScene().name == "Exercise1Scene" || SceneManager.GetActiveScene().name == "Exercise2Scene")
        {
            saveTargetPositions();
            savePathPosition();
        }
        startButton.SetActive(false);
        startStuff.SetActive(false);
        pauseButton.SetActive(true);
        patientCanvas.SetActive(true);
    }

    public void PauseTherapy() {
        unPauseButton.SetActive(true);
        pauseButton.SetActive(false);
        // SHOW quit, restart, next buttons
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        nextButton.SetActive(true);
        // SHOW target edition
        //startStuff.SetActive(true);
        // SHOW information
        //pauseStuff.SetActive(true); 

        State.isTherapyOnGoing = false;
        //State.exercise.setCompleted(true);
        // TODO stop time?
    }

    public void UnPauseTherapy() {
        pauseButton.SetActive(true);
        unPauseButton.SetActive(false);
        // HIDE quit, restart, next buttons
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        nextButton.SetActive(false);
        // HIDE target edition
        //startStuff.SetActive(false);
        // HIDE information
        //pauseStuff.SetActive(false); 

        //patientCanvas.SetActive(false); ?????
        
        State.isTherapyOnGoing = true;
        //State.exercise.setCompleted(true);
        // TODO resume time?
    }

    public void StopTherapy() {
        pauseButton.SetActive(false);
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
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void Restart()
    {
        State.resetState();
        State.exercise.restart();
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
        exerciseName.text = "--"+ State.exercise.getName()+"--";

        if(State.exercise.isCompleted()) {
            StopTherapy();
        }
    }
}
