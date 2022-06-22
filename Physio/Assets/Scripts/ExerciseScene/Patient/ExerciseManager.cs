using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Horizontal and Vertical exercise manager
public class ExerciseManager : MonoBehaviour {

    [Header("Cursors")]
    public GameObject cursor;
    public bool hasSecondaryCursor;

    [Header("Time")]
    public Text avgTime;
    public Text lastrepTime;

    [Header("Exercise")]
    public ExercisePreferencesSetup preferencesScript;
    GameObject exerciseBoxGroup;
    public GameObject leftExerciseBox;
    public GameObject rightExerciseBox;

    GameObject targets;
    public GameObject leftTargets;
    public GameObject rightTargets;

    [Header("Other Objects")]
    public NarrativeExerciseScreen narrativeScript;
    public GameObject exercisedFinishedMsg;
    public GameObject pathSize;
    public Toggle restartRepToggle;
    public string exerciseName;

    // AUDIO
    private AudioClip beep;
    private AudioSource audioSource;


    // Booleans
    private bool reversePath;
    private bool isGroing;
    private bool hasRegisteredOutOfPath;
    private bool isBlinking;
    private bool showArrows = false;

    private GameObject[] _arrows;
    private int repCounter;
    private int lastrep = 0;

    // Sound Manager
    SoundManager soundManager;

    private void Start() {
        init();
    }

    private void OnDisable() {
        CancelInvoke();
    }

    void init() {
        // setup the variables
        State.hasSecondaryCursor = hasSecondaryCursor;
        State.currentTarget = 0;
        State.hasStartedExercise = false;
        reversePath = false;

        //set the correct targets
        setArea();

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

        Renderer renderer = targets.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.r = color.b = 0;
        color.g += 1;

        renderer.material.color = color;
    }

    // Disable the incorrect targets
    private void setArea() { 
        if (State.exercise.isLeftArm()) {
            activate(true);
            exerciseBoxGroup = leftExerciseBox;
            targets = leftTargets;
        }
        else if (!State.exercise.isLeftArm()) {
            activate(false);
            exerciseBoxGroup = rightExerciseBox;
            targets = rightTargets;
        }
    
        //Detect arrows
        _arrows = GameObject.FindGameObjectsWithTag("Arrow");
        for (int i = 0; i < _arrows.Length; i++)
        {
            _arrows[i].SetActive(false);
        }
        showArrows = false;
    }
    
    private void activate(bool left) {
        leftTargets.SetActive(left);
        leftExerciseBox.SetActive(left);
        rightTargets.SetActive(!left);
        rightExerciseBox.SetActive(!left);
    }

    void Update () {
        /*GameObject[] target = GameObject.FindGameObjectsWithTag("TargetCollider");
        Debug.Log(target[0].transform.position.ToString());*/

        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);

        if(State.isTherapyOnGoing) {
            // Makes the correct target to blink
            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }
            //Show arrows
            if (!showArrows)
            {
                for (int i = 0; i < _arrows.Length; i++)
                {
                    _arrows[i].SetActive(true);
                }
                showArrows = true;
            }
            // Raycasts from hand position
            if (Physics.Raycast(landingRay, out hit)) {
                if (hit.collider.tag == "ExerciseCollider") { // is inside of the exercise area
                    if (State.hasStartedExercise) {
                        foreach (Transform exerciseBox in exerciseBoxGroup.transform) {
                            exerciseBox.gameObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
                        }
                    }
                }
                if (hit.collider.tag == "TargetCollider") { // has hit a target
                    bool hasHitTheCurrentTarget = hit.transform.position == targets.transform.GetChild(State.currentTarget).transform.position;

                    if (hasHitTheCurrentTarget) {

                        hasRegisteredOutOfPath = false;

                        if (!State.hasStartedExercise) {
                            State.hasStartedExercise = true;
                        }

                        targets.transform.GetChild(State.currentTarget).gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        if (State.currentTarget == (targets.transform.childCount - 1)) {
                            reversePath = true;
                            State.currentTarget--;
                        }

                        else if (reversePath && State.currentTarget == 0) {
                            State.currentTarget = 0;
                            State.exercise.incTries();
                            State.exercise.incCorrectReps();
                            // updates the session progression 
                            narrativeScript.IncNarrativePerc();
                            
                            reversePath = false;

                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;

                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;

                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);

                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";

                            lastrep = State.sessionTimeInt;

                            // Finished the exercise > Play Animation > Play next exercise
                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps()) { // done all the needed reps
                                // Save the Exercise Preferences
                                preferencesScript.SaveEverything();
                                // Reset State stuff
                                State.exercise.setTotalTime(State.sessionTimeInt);
                                State.exercise.setCompleted(true);
                                Debug.Log("Reseted state after completed ex.");
                                State.isTherapyOnGoing = false; //???
                                State.resetState();
                                // Show congratulations
                                StartCoroutine(showExerciseFinishedMessage());
                                
                            }
                        }

                        else if (!reversePath) {
                            State.currentTarget++;
                        }

                        else if (reversePath) {
                            State.currentTarget--;
                        }

                        soundManager.PlayOneShot("beep");
                    }
                }
            }
            else { // the cursor isn't inside of the area
                if (State.hasStartedExercise) {
                    foreach (Transform exerciseBox in exerciseBoxGroup.transform) {
                        exerciseBox.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
                    }
                    if (!hasRegisteredOutOfPath) {
                        State.exercise.incOutOfPath() ;
                        State.exercise.incTries();
                        hasRegisteredOutOfPath = true;
                    }
                    State.compensationInCurrentRep = true;
                }
            }

            if (State.compensationInCurrentRep && restartRepToggle.GetComponent<Toggle>().isOn) { // if is detected a compensation we restart the repetition
                State.compensationInCurrentRep = false;
                reversePath = false;
                if(State.currentTarget != 0) {
                    targets.transform.GetChild(State.currentTarget).gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                }
                State.currentTarget = 0;
            }
        }
    }

    // This method maked the correct target to blink 
    // AND updates the _arrows to point in the correct direction
    private void blinkTarget() {
        if (targets.transform.childCount == 0)
            return;

        Renderer renderer = null;
        int target = State.currentTarget;
        while (renderer == null) {
            Renderer rendererTemp = targets.transform.GetChild(target).gameObject.GetComponent<Renderer>();

            if (rendererTemp.enabled) {
                renderer = rendererTemp;
            }
            else if (reversePath) {
                target--;
            }
            else {
                target++;
            }
        }

        float delta;

        if (isGroing && renderer.material.color.r > 1) {
            isGroing = false;
        }
        else if (!isGroing && renderer.material.color.r < 0) {
            isGroing = true;
        }

        if (isGroing) {
            delta = 0.1f;
        }
        else {
            delta = -0.1f;
        }

        Color color = renderer.material.color;
        color.r += delta;
        color.b += delta;

        renderer.material.color = color;
        // Set _arrows with the correct direction
        for (int i = 0; i < _arrows.Length; i++)
        {
            if (target == 0 && _arrows[i].transform.parent.name == "UpArrows") _arrows[i].SetActive(false);
            if (target == 1 && _arrows[i].transform.parent.name == "UpArrows") _arrows[i].SetActive(true);
            if (target == 0 && _arrows[i].transform.parent.name == "DownArrows") _arrows[i].SetActive(true);
            if (target == 1 && _arrows[i].transform.parent.name == "DownArrows") _arrows[i].SetActive(false);
        }
    }

    private IEnumerator showExerciseFinishedMessage()
    {
        // Hide Exercise
        leftTargets.SetActive(false);
        leftExerciseBox.SetActive(false);
        rightTargets.SetActive(false);
        rightExerciseBox.SetActive(false);
        // Show the message for 5 secs
        exercisedFinishedMsg.SetActive(true);
        yield return new WaitForSeconds(5);
        exercisedFinishedMsg.SetActive(false);

        // Save the Exercise Preferences
        preferencesScript.SavePreferencesToFile();
        // Play next Exercise
        SequenceManager.nextExercise();
    }

    public void changePathSize(Slider slider)
    {
        foreach (Transform child in exerciseBoxGroup.transform)
        {
            child.localScale = new Vector3(slider.GetComponent<Slider>().value, child.localScale.y, child.localScale.z);
        }
    }
}
