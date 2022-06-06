using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BodyScreenController : MonoBehaviour {

    public Camera worldCamera;
    public TimeVis timeVis;
    private bool hasWroteReport;
    public float fadeSpeedTest;

    [Header("Buttons")]
    public GameObject startButton;
    public GameObject exerciseEdition;

    public GameObject pauseButton;
    public GameObject unPauseButton;
    public GameObject pauseMessage;

    public GameObject quitButton;
    public GameObject restartButton;
    public GameObject nextButton;


    [Header("Texts")]
    public TMP_Text leaned;
    public TMP_Text shoulderLift;
    public TMP_Text outOfPath;
    public TMP_Text exerciseName;

    bool isEditingOnStart;

    void Start()
    {
        isEditingOnStart = true;
        // If we are running the 1st exercise of a sequence, then we want to Setup/Edit
        if(SequenceManager.GetExerciseIndex() == 1){
            exerciseEdition.SetActive(true);
        }   
        else{
            // simulate the normal Start (except we didn't EDIT or pressed Start button)
            StartTherapy();
        }
    }
    
    public void Update() {
        leaned.text = "" + State.exercise.getSpineComp();
        shoulderLift.text = "" + (State.exercise.getLeftShoulderComp() + State.exercise.getRightShoulderComp());
        outOfPath.text = "" + State.exercise.getOutOfPath();
        exerciseName.text = "--"+ State.exercise.getName()+"--";

        if(State.exercise.isCompleted()) {
            StopTherapy();
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            PauseTherapy();
        }
    }

    // Called by Start button
    public void StartTherapy() {
        
        // Currently, in our programme we don't need to save the path or targets positions on start
        // We only need these values to Update the perferences folder (at the end of th exercise) 
        /*
        if (SceneManager.GetActiveScene().name == "Exercise1Scene" || SceneManager.GetActiveScene().name == "Exercise2Scene")
        {
            
            saveTargetPositions();
            savePathPosition();
            
        }
        */
        isEditingOnStart = false;
        // Initialize timeVis
        timeVis.init();
        // Deactivate button
        startButton.SetActive(false);
        // Fade out the exercise Edition
        StartCoroutine(FadeOutObject(exerciseEdition, fadeSpeedTest));
    }

    // Pause Button - show Finish and Restart Buttons / Start Stuff / UnPause Button
    // Also - stop therapy and timers
    public void PauseTherapy() {
        pauseButton.SetActive(false);
        unPauseButton.SetActive(true);
        // SHOW finish and restart
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        // Stop therapy
        State.isTherapyOnGoing = false;
        // SHOW Start Stuff (to edit path)
        exerciseEdition.SetActive(true);

        //pauseMessage.SetActive(true);
    }

    // UnPause Button
    public void UnPauseTherapy() {
        pauseButton.SetActive(true);
        unPauseButton.SetActive(false);
        // HIDE finish and restart
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        // here we prevent jumping to the exercise during the edition at the start
        if(!isEditingOnStart){
            // Resume therapy
            State.isTherapyOnGoing = true;
            // HIDE Start Stuff (to edit path)
            exerciseEdition.SetActive(false);
        }
        

        //pauseMessage.SetActive(false);
    }

    // Finish Button
    public void StopTherapy() {
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
        SceneManager.LoadScene("MainMenu");
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
        objectToFade.transform.localScale = new Vector3(1,1,1);
        objectToFade.SetActive(false);
    }
}
