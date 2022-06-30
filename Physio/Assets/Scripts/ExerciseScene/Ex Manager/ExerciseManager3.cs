using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Grid exercise manager
public class ExerciseManager3 : MonoBehaviour {

    [Header("Cursors")]
    public GameObject cursor;
    public bool hasSecondaryCursor;

    [Header("Time")]
    public Text avgTime; // these are not used
    public Text lastrepTime; // these are not used
    int lastrep = 0;

    [Header("Exercise")]
    public ExercisePreferencesSetup preferencesScript;
    GameObject[] targetsArray;
    public GameObject leftTargets;
    public GameObject rightTargets;

    int targetHitCounter = 0;
    bool[] targetHits;

    [Header("Other Objects")]
    public NarrativeExerciseScreen narrativeScript;
    public GameObject exercisedFinishedMsg;
    
    // AUDIO
    private AudioClip beep;
    private AudioSource audioSource;

    // Booleans
    private bool isGroing;
    private bool isBlinking;


    // Sound Manager
    SoundManager soundManager;
    public VoiceAssistant voiceAssistant;

    // Use this for initialization
    void Start () {
        init();
    }

    private void OnDisable() {
        CancelInvoke();
    }

    void init() {
        State.hasSecondaryCursor = hasSecondaryCursor;
        State.hasStartedExercise = false;

        activateCorrectTargets();

        beep = (AudioClip)Resources.Load("Sounds/beep");
        audioSource = GetComponent<AudioSource>();
    }
   
    // Disable the incorrect grid targets
    private void activateCorrectTargets() {
        // hide the incorrect targets
        bool left = State.exercise.isLeftArm();
        leftTargets.SetActive(left);
        rightTargets.SetActive(!left);
        // fetch the array of active targets
        targetsArray = GameObject.FindGameObjectsWithTag("TargetCollider");
        // initialize the bool array
        targetHits = new bool[targetsArray.Length];
        for (int i = 0; i < targetHits.Length; i++) targetHits[i] = false;
    }

    // Update is called once per frame
    /*void Update () {
        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);

        if(State.isTherapyOnGoing) {

            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }

            if (Physics.Raycast(landingRay, out hit)) {

                if (hit.collider.tag == "TargetCollider") { // has hit a target

                    bool hasHitTheCurrentTarget = false;
                    int targetIndex = 0;
                    for (int i = 0; i < targetsArray.Length; i++)
                    {
                        hasHitTheCurrentTarget = hit.transform.position == targetsArray[i].transform.position;
                        if (hasHitTheCurrentTarget)
                        {
                            targetIndex = i;
                            break;
                        }
                    }
                    // FC - Isto mete o targetHits[0] sempre a true??
                    if (targetHits[targetIndex] == false)
                    {
                        targetHits[targetIndex] = true;

                        if (!State.hasStartedExercise)
                        {
                            State.hasStartedExercise = true;
                        }
                        // Change the color of the target hit
                        targetsArray[targetIndex].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 1);
                        // Increment the number of targets hit
                        targetHitCounter++;
                        // If all targets were hit then...
                        if (targetHitCounter == targetsArray.Length)
                        {
                            // Increment the number of repetitions
                            State.exercise.incTries();
                            State.exercise.incCorrectReps();
                            
                            // Calculate the average time per Repetition
                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;
                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);

                            // Set the time it took to do this repetition
                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;
                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";
                            lastrep = State.sessionTimeInt;

                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps())
                            { // done all the needed reps: finish Exercise
                                State.exercise.setTotalTime(State.sessionTimeInt);
                                State.exercise.setCompleted(true);
                                State.isTherapyOnGoing = false;
                                State.resetState();
                                StartCoroutine(showWellDoneMessage());
                            } 
                            else
                            {   // some repetitions are left!
                                targetHitCounter = 0;
                                for (int i=0; i < targetHits.Length; i++)
                                {
                                    targetHits[i] = false;
                                    targetsArray[i].GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                                }
                            }
                        }

                        audioSource.PlayOneShot(beep);
                    }
                }
            }
        }
    }*/

    void Update () {
        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);

        if(State.isTherapyOnGoing) {
            // Makes the remaining targets blink
            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }

            if (Physics.Raycast(landingRay, out hit)) {

                if (hit.collider.tag == "TargetCollider") { // has hit a target

                    bool hasHitTheCurrentTarget = false;
                    int targetIndex = 0;
                    for (int i = 0; i < targetsArray.Length; i++)
                    {
                        hasHitTheCurrentTarget = hit.transform.position == targetsArray[i].transform.position;
                        if (hasHitTheCurrentTarget)
                        {
                            targetIndex = i;
                            break;
                        }
                    }
                    // We hit a target -> targetsArray[tagetIndex]
                    if (targetHits[targetIndex] == false)
                    {
                        targetHits[targetIndex] = true;
                        // declare the exercise has started
                        if (!State.hasStartedExercise)
                        {
                            State.hasStartedExercise = true;
                        }
                        // play target hit animation
                        targetsArray[targetIndex].GetComponent<Targets_Tween>().SquashAndStretch();
                        // Change the color of the target hit to "blueish"
                        targetsArray[targetIndex].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 1);
                        // Increment the number of targets hit
                        targetHitCounter++;
                        // If all targets were hit then...
                        if (targetHitCounter == targetsArray.Length)
                        {
                            // Increment the number of repetitions ( a repetition means all targets hit)
                            State.exercise.incTries();
                            State.exercise.incCorrectReps(); // we cannot have incorrect reps
                            
                            // this is not used anymore-----------------
                            // Calculate the average time per Repetition
                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;
                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);

                            // Set the time it took to do this repetition
                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;
                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";
                            lastrep = State.sessionTimeInt;
                            // this is not used anymore-----------------

                            // Finished the exercise > Play Animation > Play next exercise
                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps())
                            { // done all the needed reps: finish Exercise
                                // TODO Save the Exercise Preferences (grid: save the mode)
                                //preferencesScript.SaveEverything();
                                // Reset State stuff
                                State.exercise.setTotalTime(State.sessionTimeInt);
                                State.exercise.setCompleted(true);
                                State.isTherapyOnGoing = false; //???
                                State.resetState();
                                // Show congratulations
                                StartCoroutine(showExerciseFinishedMessage());
                            } 
                            else
                            {   // some repetitions are left! - we want to clear the grid
                                targetHitCounter = 0;
                                for (int i=0; i < targetHits.Length; i++)
                                {
                                    targetHits[i] = false;
                                    targetsArray[i].GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                                }
                                // TODO show some animation! Doing a grid repetition is hard, even if there are more to come
                            }
                        }
                        // FC - if all targets are NOT hit, we have to highlight the next one!
                        else{
                            // retrieve the next targetIndex
                            // grab it from the array
                            // change its color
                            // mark it has the next 
                        }
                        audioSource.PlayOneShot(beep);
                    }
                }
            }
        }
    }
   
    // This method blinks the remaining targets (the ones that aren't yet hit)
    /*private void blinkTarget() {

        float delta;

        for (int i = 0; i < targetsArray.Length; i++)
        {
            if (!targetHits[i])
            {
                Renderer renderer = targetsArray[i].GetComponent<Renderer>();
                if (isGroing && renderer.material.color.r > 1)
                {
                    isGroing = false;
                }
                else if (!isGroing && renderer.material.color.r < 0)
                {
                    isGroing = true;
                }

                if (isGroing)
                {
                    delta = 0.1f;
                }
                else
                {
                    delta = -0.1f;
                }

                Color color = renderer.material.color;
                color.r += delta;
                color.b += delta;

                renderer.material.color = color;
            }
        }
    }*/
    
    // This method blinks the remaining targets (the ones that aren't yet hit)
    private void blinkTarget() {
        
        if (targetsArray.Length == 0)
            return;
        if(State.currentTarget < 0 || State.currentTarget > targetsArray.Length)
            return;

        // State.currentTarget should be decided in Update randomly from the remaining targets
        Renderer renderer = targetsArray[State.currentTarget].GetComponent<Renderer>();

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
    }

    private IEnumerator showExerciseFinishedMessage()
    {
        // Hide Exercise
        leftTargets.SetActive(false);
        rightTargets.SetActive(false);
        // Show the message for 5 secs
        exercisedFinishedMsg.SetActive(true);
        yield return new WaitForSeconds(5);
        exercisedFinishedMsg.SetActive(false);

        // Save the Exercise Preferences
        preferencesScript.SavePreferencesToFile();

        // Start calculating resting time
        SequenceManager.StartRestCountDown(TimeSpan.FromSeconds(SequenceManager.sequence.getRestDuration()));

        // Play next Exercise
        SequenceManager.nextExercise();
    }
}
