using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SequenceMenuScript : MonoBehaviour
{
    public GameObject nameSequenceBox; // dialogue

    public Text newSequenceName; // text field 

    [SerializeField] SequenceListControl seqListCtrl; 

    public void showNameSequenceBox()
    {
        nameSequenceBox.SetActive(true);
    }

    // Called on the new sequence dialogue
    public void confirmNameSequence()
    {
        // Creates new sequence
        SequenceManager.newSequence(newSequenceName.text);
        // Generates the button (and starts editing)
        seqListCtrl.GenerateSequenceButton(SequenceManager.sequence);
        // closes dialogue
        nameSequenceBox.SetActive(false);
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
