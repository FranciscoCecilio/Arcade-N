using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Text helloPhrase;
    public Text age_and_gender;
    public Text time;

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
        
        // verificar a hora do dia! para dizer Bom dia, Boa tarde
        int sysHour = System.DateTime.Now.Hour; //gives you the current hour as an integer.
        string greetingsText = "Bom dia, ";
        if(sysHour > 12){
            greetingsText = "Boa tarde, ";
        }

        helloPhrase.text = greetingsText + SessionInfo.getName() + "!";
        age_and_gender.text = SessionInfo.getGender() + ", " + SessionInfo.getAge();
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

    void Update()
    {
        time.text = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
    }
}
