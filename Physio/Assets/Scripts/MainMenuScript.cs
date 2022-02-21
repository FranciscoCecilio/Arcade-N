using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Text helloPhrase;
    public Text age;
    public Text gender;

    public GameObject maleAvatar;
    public GameObject femaleAvatar;

    public void loadSequenceScene()
    {
        SceneManager.LoadScene("SequenceMenu");
    }

    public void loadExerciseSelectionScene()
    {
        SessionInfo.setView("Exercise");
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void loadExerciseResultsScene()
    {
        SessionInfo.setView("Results");
        SceneManager.LoadScene("ReportScreen");
    }

	// Use this for initialization
	void Start () {
        helloPhrase.text = "Hello, " + SessionInfo.getName() + "!";
        age.text = SessionInfo.getAge() + " years old";
        gender.text = SessionInfo.getGender();
        if (SessionInfo.getGender() == "Female")
        {
            maleAvatar.SetActive(false);
            femaleAvatar.SetActive(true);
        }
	}

    public void previousScreen()
    {
        SceneManager.LoadScene("LoginMenu2");
    }
}
