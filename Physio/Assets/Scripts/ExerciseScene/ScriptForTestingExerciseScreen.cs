using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script helps running the exercise screen without coming from Session Creation by setting the view and States
public class ScriptForTestingExerciseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(SessionInfo.toView().Equals("")){
            // Set View
            SessionInfo.setView("RunSequence");
            SessionInfo.createSessionPath();

            // Add 1nd sequence
            // Creates new sequence
            SequenceManager.newSequence("testSequence1");
            Sequence testSequence1 = SequenceManager.sequence;
            // Add Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
            testSequence1.addExercise(new Exercise(2, "nome1", "Exercise2Scene", 1, 3, 5, 2));
            testSequence1.addExercise(new Exercise(2, "nome1", "Exercise2Scene", 1, 2, 40, 5));
            // Add sequence to seq to run
            SequenceManager.sequencesToRun = new List<Sequence>();
            SequenceManager.sequencesToRun.Add(testSequence1);

            // Add 2nd sequence
            // Creates new sequence
            SequenceManager.newSequence("testSequence2");
            Sequence testSequence2 = SequenceManager.sequence;
            // Add Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
            testSequence2.addExercise(new Exercise(2, "nome2", "Exercise2Scene", 1, 1, 40, 2));
            // Add sequence to seq to run
            SequenceManager.sequencesToRun.Add(testSequence2);

            // Run sequences
            SequenceManager.SetSeqIndex(0);
            SequenceManager.nextSequence();

            // We run the 1st exercise
            /*Exercise tempExercise = testSequence.getExercise(0);
            State.exercise = tempExercise;*/
        }
    }
}
