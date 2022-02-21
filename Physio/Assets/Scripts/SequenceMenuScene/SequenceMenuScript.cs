using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SequenceMenuScript : MonoBehaviour
{

    public GameObject nameSequenceBox;
    public Text newSequenceName;

    public void showNameSequenceBox()
    {
        nameSequenceBox.SetActive(true);
    }

    public void confirmNameSequence()
    {
        SequenceManager.newSequence(newSequenceName.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void addExercise()
    {
        SessionInfo.setView("SequenceCreat");
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void run()
    {
        SequenceManager.run();
    }

    public void previousScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
