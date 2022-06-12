using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SequenceMenuScript : MonoBehaviour
{

    [SerializeField] SequenceListControl seqListCtrl; 

    int just_a_number = 0; // we need a name for the sequence... lets assign the name to a number (it's arbitrary...)

    // Called on button Create Sequence
    public void CreateNewSequence()
    {
        // Creates new sequence
        SequenceManager.newSequence(just_a_number.ToString());
        // Generates the button (and starts editing)
        seqListCtrl.GenerateSequenceButton(SequenceManager.sequence);
        // theorically it should be equal to nr of sequences created (not if we delete one)
        just_a_number ++;
    }

    // personalizar as sequencias vai ser feito a partir do SequenceListElement
    public void addExercise()
    {
        SessionInfo.setView("SequenceCreat");
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void run()
    {
        // Send the List where the sequences elements are
        SequenceManager.run(seqListCtrl.listContent);
    }

    public void previousScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
