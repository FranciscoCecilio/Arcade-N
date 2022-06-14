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

    // these are not actually percentages bc they go from 0 to 1.
    private static float sessionProgressionPerc = 0; // current progress (0 to 1)
    private static float percPerRepetition = 0; // each repetition has a percentage associated to it (0 to 1)


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
    
    public static void IncrementSessionProgression(){
        sessionProgressionPerc += percPerRepetition;
    }

    // called by SequenceMenuScript.CreateNewSequence()
    public static void newSequence(string name)
    {
        // creates a sequence 
        State.exercise = null;
        sequence = new global::Sequence(name);
        sequence.setTimestamp("Sequence" + DateTime.Now.ToString("yyyyMMddTHHmmss"));
        // NOT ANYMORE: creates a file 
        //sequence.toFile();
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
                Debug.Log("ERROR: tried to Add a Sequence from List Content but it is null.");
            }
            else{
                sequencesToRun.Add(seqToAdd);
            }
        }
        // Reset the sessionProgressionPercentage
        sessionProgressionPerc = 0;

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
        percPerRepetition = (100 / totalRepetitions) * 0.01f;
        Debug.Log("totalReps: " + totalRepetitions);
        Debug.Log("percPerRep: " + percPerRepetition);
    }

    public static void nextSequence()
    {
        if (sequenceIndex < sequencesToRun.Count)
        {
            // Assign the current sequence
            sequence = sequencesToRun[sequenceIndex];
            index = 0;
            nextExercise();
        }
        else
        {
            // END of Session! - 
            // TODO: Show congratulations, give the user the xp, show results in a panel, show narrative screen with the prize
            // TODO : save session 
            SceneManager.LoadScene("MainMenu");
        }
    }

    // this method is called after each exercise is complete
    public static void nextExercise()
    {
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
            // go to next!
            sequenceIndex ++;
            nextSequence();

            // NOT ANYMORE: here we should go back to the session screen for the rest duration!
            //SceneManager.LoadScene("MainMenu");
        }
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
    
    public static bool isNextPanelOn = false;
    public static bool isTherapistInfoOn = false;
    public static float chapterBarPercentage = 0;
    public static int nextRest = 96;


    // This resets the State because we changed Project Settings> Editor > Enter Play Mode > (disable) Domain Reload
    // If Domain Reload was checked all static values would reset BUT sometimes we would have an infinite "Application.Reload" on entering play mode
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        sequence = null;
        sequencesToRun = null;
        sequenceIndex = 0;
        index = 0;
        isNextPanelOn = false;
        isTherapistInfoOn = false;
        chapterBarPercentage = 0;
        nextRest = 96;
        Debug.Log("SequenceManager reset.");
    }
}