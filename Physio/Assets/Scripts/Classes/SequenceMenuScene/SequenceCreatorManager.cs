using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SequenceManager
{
    public static Sequence sequence;

    public static int index = 0;

    public static void newSequence(string name)
    {
        State.exercise = null;
        sequence = new global::Sequence(name);
        sequence.setTimestamp(DateTime.Now.ToString("yyyyMMddTHHmmss"));
        sequence.toFile();
    }

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