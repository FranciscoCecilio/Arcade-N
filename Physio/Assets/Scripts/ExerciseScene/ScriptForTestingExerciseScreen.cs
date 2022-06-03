using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script helps running the exercise screen without coming from Session Creation by setting the view and States
public class ScriptForTestingExerciseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        // Set View
        SessionInfo.setView("RunSequence");
        // Creates new sequence
        SequenceManager.newSequence("testSequence");
        Sequence testSequence = SequenceManager.sequence;
        // Add Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
        testSequence.addExercise(new Exercise(2, "Horizontal", "Exercise2Scene", 1, 5, 20, 10));
        // Add sequence to seq to run
        SequenceManager.sequencesToRun = new List<Sequence>();
        SequenceManager.sequencesToRun.Add(testSequence);
        // Run sequences
        SequenceManager.SetSeqIndex(0);
        SequenceManager.nextSequence();
        // We run the 1st exercise
        /*Exercise tempExercise = testSequence.getExercise(0);
        State.exercise = tempExercise;*/
    }
}
