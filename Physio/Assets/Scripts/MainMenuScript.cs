﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainMenuScript : MonoBehaviour {

    public TMP_Text helloText;
    public TMP_Text age_and_gender;

    public GameObject deleteUI;
    public TMP_Text deleteMessageNameText;
    public GameObject afterDeleteUI;
    public TMP_Text afterDeleteText;

    [Header("Narrative on the right")]
    public GameObject objToAnimate;
    public Image chapterImg;
    public TMP_Text chapterText;
    public TMP_Text proverbioText;

    SoundManager soundManager;
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

    void Awake()
    {
        // this method loads the User in the SessionInfo
        SessionInfo.loadUser();
    }

	// Use this for initialization
	void Start () {
        
        // verificar a hora do dia! para dizer Bom dia, Boa tarde
        int sysHour = System.DateTime.Now.Hour; //gives you the current hour as an integer.
        string greetingsText;
        
        if(sysHour >= 12){
            greetingsText = "BOA TARDE, ";
            
        }
        else{
            greetingsText = "BOM DIA, ";
        }
        if(LastScene._lastSceneIndex < 3) voiceAssistant.PlayRandomGreet(sysHour); // play "Bom dia" if we came from login or register screens
        // place image and text
        SetMainPage();

        // set " Bom dia, <username> " text
        string firstName = SessionInfo.getName().Split(' ')[0];
        helloText.text = greetingsText + firstName.ToUpper() + "!";
        
	}

    void SetMainPage(){
        // we want to show the last chapter unlocked (and we win XP in the end of the session, i.e. if we unlocked chapter 2 in the last session, our XP is 300)
        int currentChapter =  SessionInfo.getXP() / 100 - 1;
        if(currentChapter < 1) currentChapter = 1;
        
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
            Debug.LogWarning("Warning: Sprite not found in Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/"+ lastImg.ToString()+ " not found.");
            //  SET default
            sprite = Resources.Load<Sprite>("Narrative Materials/Chapter5/5");
        }
        chapterImg.sprite = sprite;

        //  SET TEXT
        string titlePath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/Text/Title.txt";
        chapterText.text = "CAPÍTULO " + currentChapter;
        
        if (System.IO.File.Exists( titlePath)){ 
            //Read the title directly from the Title.txt
            StreamReader reader = new StreamReader(titlePath);
            proverbioText.text = reader.ReadToEnd();
            reader.Close();
        }
        else{
            Debug.Log("ERROR: " + titlePath + " not found.");
            proverbioText.text = "A alegria é um tesouro que vale muito mais que ouro";
        } 
        LeanTween.scale(objToAnimate, new Vector3(0.95f,0.95f,0.95f) , 3f).setLoopType(LeanTweenType.pingPong).setRepeat(-1);
        
    }

    // called by the button
    public void Quit(){
        Application.Quit();
    }
    // in case someone ALT+F4
    public void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        SessionInfo.saveUserProgress();
    }

    public void previousScreen()
    {
        
        if(soundManager){
            soundManager.PlayOneShot("button_click1");
            Destroy(soundManager.gameObject, 0.5f);
        }

        if(SessionInfo.isVoiceOn()){
            StartCoroutine(previousScreenAndBye());     
        }
        else{
            SessionInfo.saveUserProgress();
            SceneManager.LoadScene("LoginMenu2");
        }
    }
    IEnumerator previousScreenAndBye(){
        SessionInfo.saveUserProgress();
        float clipLenght = voiceAssistant.PlayRandomBye();
        yield return new WaitForSeconds(clipLenght);
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
        // Find SoundManager if its null
        if(soundManager == null){
            GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
            if(soundManagerObj == null){
                Debug.LogError("ERROR: could not find a SoundManager in this scene!");
            }
            else{
                soundManager = soundManagerObj.GetComponent<SoundManager>();
            }
        }
        soundManager.PlayOneShot(sound);
    }
}
