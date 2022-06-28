using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// This class should be renamed to ButtonController since all it does is handle buttons
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

    SoundManager soundManager; // if the user clicks on the music buttons, we tell SoundManager
    public GameObject musicOnButton;
    public GameObject musicOffButton;

    public GameObject quitButton;
    public GameObject restartButton;
    public GameObject nextButton;

    bool isEditingOnStart;

    void Start()
    {
        isEditingOnStart = true;
        // Enable the correct music button according the settings (TODO: Do the same for the voice assistant)
        if(SessionInfo.isMusicOn()){
            musicOnButton.SetActive(true);
            musicOffButton.SetActive(false);
        }
        else{
            musicOnButton.SetActive(false);
            musicOffButton.SetActive(true); 
        }

        // If we are running the 1st exercise of a sequence and we have not run this exercise type in this session, then we want to Setup/Edit
        if(SequenceManager.ShouldOpenExerciseEdition() == true){
            exerciseEdition.SetActive(true);
        }   
        else{
            // simulate the normal Start (except we didn't EDIT or pressed Start button)
            StartTherapy();
        }
    }
    
    public void Update() {
        if(State.exercise.isCompleted()) {
            StopTherapy();
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseButton.activeSelf && !unPauseButton.activeSelf){
                PauseTherapy();
            }
            else if(!pauseButton.activeSelf && unPauseButton.activeSelf){
                UnPauseTherapy();
            }
        }
    }

    // Called by Start button
    public void StartTherapy() {
        isEditingOnStart = false;
        // init timeVis
        timeVis.init();
        // Deactivate button
        startButton.SetActive(false);
        // Fade out the exercise Edition
        if(exerciseEdition.activeSelf) 
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

    public void TurnMusicOn(){
        // Find SoundManager if its null
        if(soundManager == null){
            GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
            if(soundManagerObj == null){
                Debug.LogError("ERROR: could not find a SoundManager in this scene!");
            }
            else{
                soundManager = soundManagerObj.GetComponent<SoundManager>();
            }
        }
        soundManager.PlayMusicAgain();
        SessionInfo.setMusic(true);
        musicOnButton.SetActive(true);
        musicOffButton.SetActive(false);
    }

    public void TurnMusicOff(){
        // Find SoundManager if its null
        if(soundManager == null){
            GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
            if(soundManagerObj == null){
                Debug.LogError("ERROR: could not find a SoundManager in this scene!");
            }
            else{
                soundManager = soundManagerObj.GetComponent<SoundManager>();
            }
        }
        soundManager.MuteMusic();
        SessionInfo.setMusic(false);
        musicOnButton.SetActive(false);
        musicOffButton.SetActive(true);
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

    // TODO: Add a dialogue before actually jumping. 
    // TODO: Also, we should save the Exercise beforestarting another one.
    public void Next()
    {
        SequenceManager.nextExercise();
    }

    // fades object (exercise edition stuff)
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
