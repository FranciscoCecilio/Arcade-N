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
            SessionInfo.loadUser();

            // assign the current level
            SequenceManager.SetCurrentChapter(SessionInfo.getXP() / 100);
            if( SequenceManager.GetCurrentChapter() %2 == 0){
            // chatpter is even and has 4 Images to unlock
                 SequenceManager.unlockedChaptersEncoding = new List<int>() {0,0,0,0};
            }
            else{
                // chatpter is odd and has 5 Images to unlock
                 SequenceManager.unlockedChaptersEncoding = new List<int>() {0,0,0,0,0};
            }
            // when we start a new Session we want to watch the last chapter's preview
            SequenceManager.hasPreviewToUnlock = true;

            // Add 1nd sequence
            // Creates new sequence
            SequenceManager.newSequence("testSequence1");
            Sequence testSequence1 = SequenceManager.sequence;
            // Add Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
            testSequence1.addExercise(new Exercise(0, "grid1", "Exercise2Scene", 2, 3, 40, 20));
            //testSequence1.addExercise(new Exercise(2, "vertical1", "Exercise2Scene", 1, 2, 40, 5));
            // Add sequence to seq to run
            SequenceManager.sequencesToRun = new List<Sequence>();
            SequenceManager.sequencesToRun.Add(testSequence1);

            // Add 2nd sequence
            // Creates new sequence
            SequenceManager.newSequence("testSequence2");
            Sequence testSequence2 = SequenceManager.sequence;
            // Add                      Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
            testSequence2.addExercise(new Exercise(0, "grid1", "Exercise1Scene", 2, 3, 40, 20));
            // Add sequence to seq to run
            SequenceManager.sequencesToRun.Add(testSequence2);
            //SequenceManager.sequencesToRun.Add(testSequence2);

            // Run sequences
            SequenceManager.SetSessionProgressionPerc(0);
            SequenceManager.CalculatePercPerRepetition();
            SequenceManager.SetSeqIndex(0);
            SequenceManager.nextSequence();

            // We run the 1st exercise
            /*Exercise tempExercise = testSequence.getExercise(0);
            State.exercise = tempExercise;*/
        }
    }
}
