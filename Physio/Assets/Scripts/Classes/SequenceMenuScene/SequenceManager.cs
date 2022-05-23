﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class should go through the list and play all sequences.
public static class SequenceManager
{
    // current sequence
    public static Sequence sequence;

    // when we press RUN we fetch all the sequences in the list
    public static List<Sequence> sequencesToRun;

    // gives the number of the sequence we are running
    private static int sequenceIndex = 0;

    // index gives the number of the exercise of the given sequence (if the sequence has 3 exercises and we are running the 1st exercise, index = 0)
    public static int index = 0;

    // called by SequenceMenuScript.confirmNameSequence()
    public static void newSequence(string name)
    {
        // creates a sequence 
        State.exercise = null;
        sequence = new global::Sequence(name);
        sequence.setTimestamp(DateTime.Now.ToString("yyyyMMddTHHmmss"));
        // creates a file 
        sequence.toFile();
    }

    // run is called in the start of a session by the button
    public static void run(Transform listContent)
    {
        // Set View
        SessionInfo.setView("RunSequence");

        // Get all Sequences in the List
        sequencesToRun = new List<Sequence>();

        foreach (Transform eachChild in listContent) {
            Debug.Log(eachChild);
            Sequence seqToAdd = eachChild.GetComponent<SequenceListElement>().GetSequence();
            if(seqToAdd == null){
                Debug.Log("ERROR: tried to Add a Sequence from List Content but it is null.");
            }
            else{
                sequencesToRun.Add(seqToAdd);
            }
        }
        // TODO : Calculate the XP per Sequence

        // Start running the 1st Sequence
        if(sequencesToRun.Count > 0){
            sequenceIndex = 0;
            nextSequence();
        }
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
            // END of Session!
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
            // this sequence is finished - go to next!
            // TODO? here we should go back to the session screen for the rest duration!
            sequenceIndex ++;
            nextSequence();
            //SceneManager.LoadScene("MainMenu");
        }
    }
}