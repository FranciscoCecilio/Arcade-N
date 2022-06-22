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

    [Header("Narrative on the right")]
    public Image chapterImg;
    public TMP_Text chapterText;

    public SoundManager soundManager;
    public VoiceAssistant voiceAssistant;

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
        string greetingsText;
        
        if(sysHour > 12){
            greetingsText = "BOA TARDE, ";
            voiceAssistant.PlayVoiceLine("boa_tarde");
        }
        else{
            greetingsText = "BOM DIA, ";
            voiceAssistant.PlayVoiceLine("bom_dia");
        }
        // this method loads the User in the SessionInfo
        SessionInfo.loadUser();
        
        // place image and text
        SetMainPage();

        // set " Bom dia, <username> " text
        string firstName = SessionInfo.getName().Split(' ')[0];
        helloText.text = greetingsText + firstName.ToUpper() + "!";
        
        //age_and_gender.text = SessionInfo.getGender() + ", " + SessionInfo.getAge();
        if (SessionInfo.getGender() == "Female")
        {
            maleAvatar.SetActive(false);
            femaleAvatar.SetActive(true);
        }
	}

    void SetMainPage(){
        int currentChapter =  SessionInfo.getXP() / 100 + 1;
        int lastImg;
        if(currentChapter % 2 == 0){
            lastImg = 4;
        }
        else{
            lastImg = 5;
        }
        //  SET IMAGE
        Sprite sprite = Resources.Load<Sprite>("Narrative Materials/Chapter" + currentChapter.ToString() +"/"+ lastImg.ToString());

        if(sprite == null){
            Debug.LogError("ERROR: Sprite not found in Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/"+ lastImg.ToString()+ " not found.");
        }
        else{
            chapterImg.sprite = sprite;
            chapterText.text = "CAPÍTULO " + currentChapter;
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
        if(SessionInfo.isVoiceOn()){
            StartCoroutine(previousScreenAndBye());     
        }
        else{
            SessionInfo.logout();
            SceneManager.LoadScene("LoginMenu2");
        }
    }
    IEnumerator previousScreenAndBye(){
        float clipLenght = voiceAssistant.PlayRandomBye();
        yield return new WaitForSeconds(clipLenght);
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
        // Find SoundManager if its null
        if(soundManager == null){
            GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
            if(soundManager == null){
                Debug.LogError("ERROR: could not find a SoundManager in this scene!");
            }
            else{
                soundManager = soundManagerObj.GetComponent<SoundManager>();
            }
        }
        soundManager.PlayOneShot(sound);
    }
}
