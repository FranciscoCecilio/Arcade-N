using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 
public class SequenceMenuScript : MonoBehaviour
{

    public GameObject nameSequenceBox;
    public Text newSequenceName;
    [SerializeField] SequenceListControl seqListCtrl;

    public void showNameSequenceBox()
    {
        nameSequenceBox.SetActive(true);
    }

    // Called on the new sequence dialogue
    public void confirmNameSequence()
    {
        SequenceManager.newSequence(newSequenceName.text);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        seqListCtrl.GenerateSequenceButton(SequenceManager.sequence);
        nameSequenceBox.SetActive(false);
        // TODO we want to open edit panel - implica mostrar exercse_selection
        
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
