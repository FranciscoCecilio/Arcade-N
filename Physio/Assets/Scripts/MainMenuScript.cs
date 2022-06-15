using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour {

    public TMP_Text helloText;
    public TMP_Text age_and_gender;

    public GameObject maleAvatar;
    public GameObject femaleAvatar;

    public GameObject deleteUI;
    public TMP_Text deleteMessageNameText;
    public GameObject afterDeleteUI;
    public TMP_Text afterDeleteText;

    SoundManager soundManager;

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
    // called by the button
    public void Quit(){
        SessionInfo.logout();
        Application.Quit();
    }
    // in case someone ALT+F4
    public void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        SessionInfo.logout();
    }

    public void previousScreen()
    {
        SessionInfo.logout();
        SceneManager.LoadScene("LoginMenu2");
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

    
    public void PlaySoundManager(string sound){
        // Update SoundManager
        if(soundManager == null){
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
            if(soundManager == null){
                Debug.Log("ERROR: could not find a SoundManager in this scene!");
            }
        }
        soundManager.PlayOneShot(sound);
    }
}
