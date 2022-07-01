using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class is responsible for playing the correct exercises
// If we have to play 3 sequences, we will play all the Exercises of Sequence 1, then 2, and then 3.
public static class SequenceManager
{
    // current sequence
    public static Sequence sequence;

    // when we press RUN we fetch all the sequences in the list
    public static List<Sequence> sequencesToRun;

    // gives the number of the sequence we are running
    private static int sequenceIndex = 0;

    // index gives the number of the exercise of the given sequence (if the sequence has 3 exercises and we are running the 1st exercise, index = 0)
    private static int index = 0;

    // grid, horizontal, vertical (it becomes 1 if in this session we played that exercise)
    private static int[] runnedExerciseTypes = new int[3] {0, 0, 0}; 
    
    // these are not actually percentages bc they go from 0 to 1.
    private static float sessionProgressionPerc = 0; // current progress (0 to 1)
    private static float percPerRepetition = 0; // each repetition has a percentage associated to it (0 to 1)
    
    // Resting time between series 
    static DateTime RestTimeStarted;
    static TimeSpan TotalRestTime;

    // Narrative variables
    public static List<int> unlockedChaptersEncoding ;  // 0: not yet unlocked; 1: unlocked and waiting to have it image placed; -1: image placed
    private static int currentChapter; // current Level
    public static bool hasImagesToUnlock = false;
    public static bool hasPreviewToUnlock = false; // only becomes true on the start of the session.
    public static bool showOverview = false;

    // Setters and Getters
    public static void SetSeqIndex(int i){
        sequenceIndex = i;
    }

    public static int GetSeqIndex(){
        return sequenceIndex;
    }

    public static void SetExerciseIndex(int i){
        index = i;
    }

    public static bool ShouldOpenExerciseEdition(){
        // we want to open exercise_setup automatically when we start a new exercise type
        return index == 1 && runnedExerciseTypes[sequence.getExercisesId()] == 0;
    }
    public static int GetExerciseIndex(){
        return index;
    }

    public static void SetSessionProgressionPerc(float i){
        sessionProgressionPerc = i;
    }

    public static float GetSessionProgressionPerc(){
        return sessionProgressionPerc;
    }

    public static void SetPercPerRepetition(float i){
        percPerRepetition = i;
    }

    public static float GetPercPerRepetition(){
        return percPerRepetition;
    }
    
    public static void SetCurrentChapter(int i){
        currentChapter = i;
    }

    public static int GetCurrentChapter(){
        currentChapter =  SessionInfo.getXP() / 100;
        if(currentChapter == 0 ||currentChapter == 1 ) 
            currentChapter = 2;
        return currentChapter;
    }

    public static void IncrementSessionProgression(){
        // increment the percentage
        sessionProgressionPerc += percPerRepetition;

        Debug.Log(sessionProgressionPerc);
        if(sessionProgressionPerc>0.98f) sessionProgressionPerc = 1.0f;

        // update the encoding
        if(currentChapter%2 == 0){
            // chatpter is even and has 4 Images to unlock
            // checkpoints are on 25% , 50% , 75% and 100%
            switch(sessionProgressionPerc){
                case <=0.24f:
                    // do nothing: we have not reached any image 
                    return;
                case <= 0.49f:
                    // check if we reach image 1 and assign it
                    UpdateEncoding(1);
                    break;
                case <= 0.74f:
                    // check if we reach image 1, 2 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    break;
                case < 0.99f:
                    // check if we reach image 1, 2, 3 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    UpdateEncoding(3);
                    break;
                case >= 0.99f:
                    Debug.Log("updates encoding for last image even");
                    // check if we reach image 1, 2, 3, 4 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    UpdateEncoding(3);
                    UpdateEncoding(4);
                    break;
            }
        }
        else{
            // chatpter is odd and has 5 Images to unlock
            // checkpoints are on 20% , 40% , 60% , 80% and 100%
            switch(sessionProgressionPerc){
                case <=0.19f:
                    // do nothing: we have not reached any image 
                    return;
                case <= 0.39f:
                    // check if we reach image 1 and assign it
                    UpdateEncoding(1);
                    break;
                case <= 0.59f:
                    // check if we reach image 1, 2 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    break;
                case < 0.79f:
                    // check if we reach image 1, 2, 3 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    UpdateEncoding(3);
                    break;
                case < 0.99f:
                    // check if we reach image 1, 2, 3, 4 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    UpdateEncoding(3);
                    UpdateEncoding(4);
                    break;
                case >= 0.99f:
                    Debug.Log("updates encoding for last image odd");
                    // check if we reach image 1, 2, 3, 4, 5 and assign it
                    UpdateEncoding(1);
                    UpdateEncoding(2);
                    UpdateEncoding(3);
                    UpdateEncoding(4);
                    UpdateEncoding(5);
                    break;
            }
        }
    }

    // this is called when the percentage is directly above a certain image (is enough to unlock that image)
    static void UpdateEncoding(int imageNumber){ // image number goes from 1 to 5
        int i = imageNumber - 1;
        int code = unlockedChaptersEncoding[i];
        switch(code){
            case 0:
                // we want to unlock this image for the first time
                unlockedChaptersEncoding[i] = 1;
                hasImagesToUnlock = true;
                break;
            case 1:
                // we want to unlock this image but it was flaged before
                // DO nothing
                break;
            case -1:
                // we have unlocked this image so declare it as such
                // DO nothing
                break;
        }
    }

    // called by SequenceMenuScript.CreateNewSequence()
    public static void newSequence(string name)
    {
        // creates a sequence 
        State.exercise = null;
        sequence = new global::Sequence(name);
        sequence.setTimestamp("Sequence" + DateTime.Now.ToString("yyyyMMddTHHmmss"));
    }

    // run is called in the start of a session by the button
    public static void run(Transform listContent)
    {
        // Set View
        SessionInfo.setView("RunSequence");

        // Get all Sequences in the List
        sequencesToRun = new List<Sequence>();

        foreach (Transform eachChild in listContent) {
            
            Sequence seqToAdd = eachChild.GetComponent<SequenceListElement>().GetSequence();
            
            if(seqToAdd == null){
                Debug.LogError("ERROR: tried to Add a null Sequence from List Content.");
            }
            else{
                sequencesToRun.Add(seqToAdd);
            }
        }
        // assign the current level
        currentChapter =  SessionInfo.getXP() / 100;

        if(currentChapter%2 == 0){
            // chatpter is even and has 4 Images to unlock
            unlockedChaptersEncoding = new List<int>() {0,0,0,0};
        }
        else{
            // chatpter is odd and has 5 Images to unlock
            unlockedChaptersEncoding = new List<int>() {0,0,0,0,0};
        }
        
        // Reset the sessionProgressionPercentage
        sessionProgressionPerc = 0;

        // when we start a new Session we want to watch the last chapter's preview
        hasPreviewToUnlock = true;

        //Calculate the PercPerRepetition
        CalculatePercPerRepetition();

        // Start running the 1st Sequence
        if(sequencesToRun.Count > 0){
            sequenceIndex = 0;
            nextSequence();
        }
    }

    
    // calculates the impact one repetition has on the percentageBar during the exercise
    public static void CalculatePercPerRepetition(){
        int totalRepetitions = 0;
        // we iterate over every sequence and check the total number of repetitions
        for(int i = 0; i < sequencesToRun.Count; i++){
            totalRepetitions += sequencesToRun[i].getTotalRepetitions();
        }
        // set the variable percePerRepetition
        percPerRepetition = (100f / (float)totalRepetitions) * 0.01f;
        // here we want to set in which percentages we need to unlock images from the chapter

    }

    public static void nextSequence()
    {
        if (sequenceIndex < sequencesToRun.Count)
        {
            // Assign the current sequence
            sequence = sequencesToRun[sequenceIndex];
            // Restart exercises index
            index = 0;
            nextExercise();
        }
        else
        {   
            // END of Session!
            EndOfSession(); 
        }
    }

    // this method is called after each exercise is complete
    public static void nextExercise()
    {
        // here we want to check for narrative pictures to show!
        if(1 == 2){
        //if(hasPreviewToUnlock || hasImagesToUnlock){
            SceneManager.LoadScene("NarrativeMenu");
        }
        else{
            if (index < sequence.getLength())
            {
                Exercise tempExercise = sequence.getExercise(index);
                State.exercise = tempExercise;
                SceneManager.LoadScene(tempExercise.getScenePath());
                index++;
            }
            else
            {
                // THIS SEQUENCE IS FINISHED
                // Save Sequence
                sequence.toFile();
                // Update runned exercise types
                runnedExerciseTypes[sequence.getExercisesId()] = 1; // i.e. if we run a vertical sequence: runnedExerciseTypes= {0,0,1}
                // go to next!
                sequenceIndex ++;
                nextSequence();
            }
        }
    }

 

    // GlobalCountDown.StartCountDown(TimeSpan.FromMinutes(5));
    // Called before nextExercise() on ExerciseManager
    public static void StartRestCountDown(TimeSpan totalTime)
    {
        RestTimeStarted = DateTime.UtcNow;
        TotalRestTime = totalTime;
    }

    // returns the timespan with the time left to rest
    public static TimeSpan RestTimeLeft
    {
        get
        {
            if(TotalRestTime == null) return  TimeSpan.Zero; // if we didnt assign (for testing for example)

            var result = TotalRestTime - (DateTime.UtcNow - RestTimeStarted);
            if (result.TotalSeconds <= 0)
                return TimeSpan.Zero;
            return result;
        }
    }

    public static double GetTotalRestTime(){
        return TotalRestTime.TotalSeconds;
    }
    
    public static void EndOfSession(){
        // Give the user the xp - Level up (NOTE: CAP of 500 XP)
        int updatedXP = (currentChapter + 1) * 100;
        if(updatedXP <= 500){
            SessionInfo.setXP(updatedXP);
        } 
        else{
            showOverview = true;
        }
        // Save session
        SessionInfo.saveSession();
        // Show congratulations and show results
        SceneManager.LoadScene("SessionRewards");
    }

    // called by Session Rewards
    public static void ResetAfterEndOfSession(){
        // after loading SessionRewards
        // clear variables
        sequencesToRun.Clear();
        sequence = null;
        TotalRestTime =  TimeSpan.Zero;
        // Reset the sessionProgressionPercentage
        sessionProgressionPerc = 0;
        // reset narrative stuff
        hasPreviewToUnlock = false;
        hasImagesToUnlock = false;
        // reset indexes
        sequenceIndex = 0;
        index = 0;
    }

    // this method returns the **1st Exercise from the next Sequence** and is usefull to show in the panel of exercise scene
    public static Exercise GetNextExercise(){
        int nextSIndex = sequenceIndex + 1;

        // if exists a next sequence
        if (nextSIndex < sequencesToRun.Count)
        {
            // Assign the current sequence
            Sequence tempSequence = sequencesToRun[nextSIndex];
            // We assume that the next sequence has at least 1 exercise (is not empty)
            return tempSequence.getExercise(0); 
        }
        return null;
    }

    
    //-----------------------------------------------------------------------------------------------------------------------------------
    // Settings during the session (maybe passar para sessionInfo e dar upload no text do user)
    
    public static bool isNextPanelOn = true;
    public static bool isTherapistInfoOn = true;
    public static float chapterBarPercentage = 0;


    // This resets the State because we changed Project Settings> Editor > Enter Play Mode > (disable) Domain Reload
    // If Domain Reload was checked all static values would reset BUT sometimes we would have an infinite "Application.Reload" on entering play mode
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        sequence = null;
        sequencesToRun = null;
        sequenceIndex = 0;
        index = 0;
        isNextPanelOn = true;
        isTherapistInfoOn = true;
        chapterBarPercentage = 0;
        Debug.Log("SequenceManager reset.");
    }
}