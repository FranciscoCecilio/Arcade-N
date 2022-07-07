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

    bool[] targetHits; // not used anymore

    [Header("Other Objects")]
    public NarrativeExerciseScreen narrativeScript;
    public GameObject exercisedFinishedMsg;
    public GridPaternManager gridManager;
    
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

    }
   
    // Disable the incorrect grid targets
    private void activateCorrectTargets() {
        // hide the incorrect targets
        bool left = State.exercise.isLeftArm();
        leftTargets.SetActive(left);
        rightTargets.SetActive(!left);
        // fetch the array of active targets
        targetsArray = GameObject.FindGameObjectsWithTag("TargetCollider");
        Array.Sort( targetsArray, CompareObNames );
        // initialize the bool array --- we dont need anymore bcs we have order now
        targetHits = new bool[targetsArray.Length];
        for (int i = 0; i < targetHits.Length; i++) targetHits[i] = false;
    }

    int CompareObNames( GameObject x, GameObject y )
    {
        return Int32.Parse(x.name).CompareTo( Int32.Parse(y.name) );
        //return x.name.CompareTo( y.name );
    }

    public void FindSoundManager(){
        GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
        if(soundManagerObj == null){
            Debug.LogError("ERROR: could not find a SoundManager in this scene!");
        }
        else{
            soundManager = soundManagerObj.GetComponent<SoundManager>();
        }
    }


    void Update () {
        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);
        
        // TimeVis.cs sets true State-IsTherapyOnGoing
        if(State.isTherapyOnGoing) {
            // Makes the correct target blink every second
            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }

            if (Physics.Raycast(landingRay, out hit)) {

                if (hit.collider.tag == "TargetCollider") { // has hit a target

                    Vector3 currentTargetPos = targetsArray[gridManager.GetCurrentTargetID()].transform.position;
                   
                    // We hit a target -> targetsArray[tagetIndex]
                    //if (targetHits[targetIndex] == false)
                    if(hit.transform.position == currentTargetPos)
                    {
                        int targetIndex = gridManager.GetCurrentTargetID(); // Target with ID = 0 is the target with index 0 in the targetsArray
                        targetHits[targetIndex] = true; // we dont need this anymore but still ...

                        // declare the exercise has started
                        if (!State.hasStartedExercise)
                        {
                            State.hasStartedExercise = true;
                        }
                        // play target hit animation
                        targetsArray[targetIndex].GetComponent<Targets_Tween>().SquashAndStretch();
                        // Change the color of the target hit to "green"
                        targetsArray[targetIndex].GetComponent<Renderer>().material.color = new Color(0.2f, 0.7f, 0.2f, 0.5f); // greenish
                        // Increment the number of targets hit
                        gridManager.IncrementTargetHitCounter(); // targetHitCounter ++;
                        // If all targets were hit then...
                        if (gridManager.GetTargetHitCounter() >= targetsArray.Length)
                        {
                            // Increment the number of repetitions ( a repetition means all targets hit)
                            State.exercise.incTries();
                            State.exercise.incCorrectReps(); // we cannot have incorrect reps
                            // updates the session progression 
                            narrativeScript.IncNarrativePerc();
                            // ALWAYS play good sound + text
                            voiceAssistant.PlayRandomGood();
                            // this is not used anymore: Calculate the average time per Repetition
                            /* 
                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;
                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);

                            // Set the time it took to do this repetition
                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;
                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";
                            lastrep = State.sessionTimeInt;
                            */ 

                            // Finished the exercise > Play Animation > Play next exercise
                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps())
                            { // done all the needed reps: finish Exercise
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
                                // reset TargetHitCounter 
                                gridManager.SetTargetHitCounter(0);
                                // reset the unnused targetsHit array
                                for (int i=0; i < targetHits.Length; i++)
                                {
                                    targetHits[i] = false;
                                    targetsArray[i].GetComponent<Renderer>().material.color = new Color(1, 1, 1);
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
                        if(soundManager == null) FindSoundManager();
                        soundManager.PlayOneShot("beep");
                    }
                }
            }
        }
    }
   
    
    // This method blinks the correct target to hit on the grid
    // TODO when is this called? once per second? or once.
    private void blinkTarget() {
        
        if (targetsArray.Length == 0)
            return;
        /*if(State.currentTarget < 0 || State.currentTarget > targetsArray.Length)
            return;*/

        // define whats the correct target
        int currentTargetID = gridManager.GetCurrentTargetID();
        if(gridManager.GetTargetHitCounter() >= targetsArray.Length) return;

        // color the next-next target
        if(gridManager.GetTargetHitCounter() + 1 < targetsArray.Length) 
        {
            int nextTargetID = gridManager.GetNextTargetID();

            Renderer renderer2 = targetsArray[nextTargetID].GetComponent<Renderer>();

            Color32 color2 = new Color(0.53725f, 0.81176f, 0.94118f) ;

            renderer2.material.color = color2;


        }

        // blink the currentTarget
        Renderer renderer = targetsArray[currentTargetID].GetComponent<Renderer>();

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
        if(color == new Color(0.53725f, 0.81176f, 0.94118f)) color = new Color (0,1,0, 1);
        //Color color = new Color(renderer.material.color.r,1,renderer.material.color.b,renderer.material.color.a);
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
        // Save the Exercise Preferences
        preferencesScript.SavePreferencesToFile();
        yield return new WaitForSeconds(5);
        exercisedFinishedMsg.SetActive(false);

        // Start calculating resting time
        SequenceManager.StartRestCountDown(TimeSpan.FromSeconds(SequenceManager.sequence.getRestDuration()));

        // Play next Exercise
        SequenceManager.nextExercise();
    }

}
