using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour {

    public TMP_Text helloText;
    public TMP_Text age_and_gender;
    public TMP_Text time;

    public GameObject maleAvatar;
    public GameObject femaleAvatar;

    public GameObject deleteUI;
    public TMP_Text deleteMessageNameText;
    public GameObject afterDeleteUI;
    public TMP_Text afterDeleteText;

    public void loadSequenceScene()
    {
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("SequenceMenu");
    }

    public void loadExerciseSelectionScene()
    {
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SessionInfo.setView("Exercise");
        SceneManager.LoadScene("ExerciseSelection");
    }

    public void loadExerciseResultsScene() 
    {
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SessionInfo.setView("Results");
        SceneManager.LoadScene("ReportScreen");
    }

     public void loadNarrativeScene() 
    {
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SessionInfo.setView("Narrative");
        SceneManager.LoadScene("NarrativeMenu");
    }

	// Use this for initialization
	void Start () {
        
        // verificar a hora do dia! para dizer Bom dia, Boa tarde
        int sysHour = System.DateTime.Now.Hour; //gives you the current hour as an integer.
        string greetingsText = "BOM DIA, ";
        if(sysHour > 12){
            greetingsText = "BOA TARDE, ";
        }
        // this method loads the User in the SessionInfo
        SessionInfo.loadUser();
        
        helloText.text = greetingsText + SessionInfo.getName().ToUpper() + "!";
        //age_and_gender.text = SessionInfo.getGender() + ", " + SessionInfo.getAge();
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

    private void DeleteUser()
    {
        afterDeleteUI.SetActive(true);
        SessionInfo.DeleteUser(afterDeleteText);
        StartCoroutine(Delay());
    }

    public void CancelDelete(){
        deleteUI.SetActive(false);
    }
    public void OpenDeleteMessage(){
        deleteUI.SetActive(true);
        deleteMessageNameText.text = "Deseja eliminar o perfil do user: " + SessionInfo.getName() + "?";
    }

    IEnumerator Delay()
    {
        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("LoginMenu2");   
    }
}
