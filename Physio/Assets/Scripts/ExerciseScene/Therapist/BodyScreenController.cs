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
    public GameObject pauseMessage;
    public GameObject quitButton;
    public GameObject restartButton;

    public GameObject nextButton;
    public GameObject patientCanvas;

    public TMP_Text leaned;
    public TMP_Text shoulderLift;
    public TMP_Text outOfPath;
    public TMP_Text exerciseName;

    public Camera worldCamera;

    public float fadeSpeedTest;
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
        // Fade out
        StartCoroutine(FadeOutObject(startStuff, fadeSpeedTest));
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
        pauseMessage.SetActive(true);
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
        pauseMessage.SetActive(false);
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

    // fades object (start stuff)
    private IEnumerator FadeOutObject(GameObject objectToFade, float fadeSpeed){
        /*while(objectToFade.GetComponent<Renderer>().material.color.a > 0){
            Color objectColor = objectToFade.GetComponent<Renderer>().material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            objectToFade.GetComponent<Renderer>().material.color = objectColor;
            yield return null;
        }*/
        while(objectToFade.transform.localScale.x < 3){
            Vector3 scale = objectToFade.transform.localScale;
            float newScale = scale.x + (fadeSpeed * Time.deltaTime);

            scale = new Vector3(newScale, newScale, newScale);
            objectToFade.transform.localScale = scale;
            yield return null;
        }
        Debug.Log("Chegou aqui");
        objectToFade.transform.localScale = new Vector3(1,1,1);
        objectToFade.SetActive(false);
    }
}
