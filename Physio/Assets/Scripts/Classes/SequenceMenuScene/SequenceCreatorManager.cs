using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SequenceManager
{
    public static Sequence sequence;

    // index gives the number of the exercise (if the sequence has 3 exercises and we are running the 1st exercise, index = 0)
    public static int index = 0;

    // chamado apenas em SequenceMenuScript.confirmNameSequence()
    public static void newSequence(string name)
    {
        // criar uma sequencia 
        State.exercise = null;
        sequence = new global::Sequence(name);
        sequence.setTimestamp(DateTime.Now.ToString("yyyyMMddTHHmmss"));
        // criar um ficheiro com o TimeStamp da sequencia
        sequence.toFile();
    }

    // run is called in the start of a session
    public static void run()
    {
        SessionInfo.setView("RunSequence");
        index = 0;
        nextExercise();
    }

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
            SceneManager.LoadScene("MainMenu");
        }
    }

}