using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
 using System.Collections;
using TMPro;
using System.IO;

// Used in Main Menu, Narrative Menu, all Exercises,
public class VoiceAssistant : MonoBehaviour
{
    public Sound[] sounds;
    List<AudioSource> allSources;

    public string[] goodSoundsNames; // played when the user successfully hit a target during Exercise
    public string[] badSoundsNames; // played when the user goes out of path during Exercise
    public string[] encSoundsNames; // played when the user goes out of path during Exercise
    public string[] restSoundsNames; // played when the user is waiting to start the exercise
    public string[] greetingSoundsNames; // played when we login
    public string[] byeSoundsNames; // played when we logout
    public string[] endOfSessionSoundsNames; // played on the session rewards scene

    [Header("For Debug")]
    [SerializeField] bool _voiceIsOn;

    [Header("Voice Text")]
    public GameObject VoiceLine;
    public TMP_Text voiceLineText;

    bool hasStarted = false;

    public Sound GetSound(string name){
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        return s;
    }

    void Awake(){
        // Initializae all sounds
        allSources = new List<AudioSource>();
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            // Add audiosource to list
            allSources.Add(s.source);
        }
    }

    public void Start(){
        _voiceIsOn = SessionInfo.isVoiceOn();
        hasStarted = true;
    }

    // returns the lenght of the clip being played or 5 seconds by default
    public float PlayVoiceLine(string name){

        if(!hasStarted)
             _voiceIsOn = SessionInfo.isVoiceOn(); // in the mainmenu we play greeting before this script run Start

        if(!_voiceIsOn)
            return 5f;
        
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            if(VoiceLine != null) 
            {
                StopCoroutine("ShowVoiceLine");
                StartCoroutine(ShowVoiceLine(5, name));
            }
            return 5f;
        }

        s.source.PlayOneShot(s.clip);
        Debug.Log("Audio clip length : " + s.source.clip.length);

        float clipLength = s.source.clip.length;

        if(clipLength == 0){
            if(VoiceLine != null) {
                StopCoroutine("ShowVoiceLine");
                StartCoroutine(ShowVoiceLine(5, name));
            }
            return 5f;
        } 
        else{
            if(VoiceLine != null) {
                StopCoroutine("ShowVoiceLine");
                StartCoroutine(ShowVoiceLine(clipLength, name));
            }
            return clipLength;
        }
    }

    IEnumerator ShowVoiceLine(float clipDuration, string clipName){
        // load TEXT
        string textPath = Application.dataPath + "/Resources/Sounds/VoiceActing/ExpressionsScripts/" + clipName + ".txt";
        if (System.IO.File.Exists(textPath)){ 
            //Read the text from the .txt file
            StreamReader reader = new StreamReader(textPath);
            string textToWrite = reader.ReadToEnd();
            reader.Close();

            // scale to zero
            VoiceLine.transform.localScale = Vector3.zero;

            // fade in 
            VoiceLine.SetActive(true);
            LeanTween.scale(VoiceLine, Vector3.one ,0.2f).setEase(LeanTweenType.easeInBack); 

            // place text
            voiceLineText.text = textToWrite;
            
            // wait for its duration to hide
            yield return new WaitForSeconds(clipDuration);
            LeanTween.scale(VoiceLine, Vector3.zero, 0.2f).setEase(LeanTweenType.easeOutBack); 

            //VoiceLine.SetActive(false);
        }
        else{
            
            Debug.LogError("ERROR: Voice Line text " + textPath + " not found.");
            yield return 0;
        }
    }

    public float PlayRandomGood(){
        // Play one of the "good job" sounds randomly
        int index = UnityEngine.Random.Range (0, goodSoundsNames.Length);
        string chosenSound  = goodSoundsNames[index];
        return PlayVoiceLine(chosenSound);
    }

    public float PlayRandomBad(){
        // Play one of the "out_of_path" sounds randomly
        int index = UnityEngine.Random.Range (0, badSoundsNames.Length);
        string chosenSound  = badSoundsNames[index];
        // bad3 = "Desviado" ; bad4 = "Desviada"
        if(chosenSound.Equals("bad3") && SessionInfo.getGender().Equals("feminino")) chosenSound = "bad4";
        else if(chosenSound.Equals("bad4") && SessionInfo.getGender().Equals("masculino")) chosenSound = "bad3";
        return PlayVoiceLine(chosenSound);
    }

    public float PlayRandomRest(){
        // Play one of the "take a rest" sounds randomly
        int index = UnityEngine.Random.Range (0, restSoundsNames.Length);
        string chosenSound  = restSoundsNames[index];
        return PlayVoiceLine(chosenSound);
    }

    public float PlayRandomGreet(int sysHour){
        // Play one of the "Bom dia" sounds randomly
        int index = UnityEngine.Random.Range (0, greetingSoundsNames.Length);
        string chosenSound  = greetingSoundsNames[index];
        Debug.Log(chosenSound);
        // greet1 = "Bom dia" ; greet2 = "Boa tarde" ; greet3 = "Ola" ; greet 4 = "Bem vindo" ; greet5 = "Bem vinda"
        if(chosenSound.Equals("greet1") && sysHour >= 12) chosenSound = "greet2";
        else if(chosenSound.Equals("greet2") && sysHour < 12) chosenSound = "greet1";
        else if(chosenSound.Equals("greet4") && SessionInfo.getGender().Equals("feminino")) chosenSound = "greet5";
        else if(chosenSound.Equals("greet5") && SessionInfo.getGender().Equals("masculino")) chosenSound = "greet4";

        return PlayVoiceLine(chosenSound);
    }

    public float PlayRandomBye(){
        // Play one of the "bye" sounds randomly
        int index = UnityEngine.Random.Range (0, byeSoundsNames.Length);
        string chosenSound  = byeSoundsNames[index];
        //return PlayVoiceLine(chosenSound);
        return PlayVoiceLine("bye1");
    }

    public float PlayRandomEndOfSession(){
        // Play one of the "Boa Sessão!" sounds randomly
        int index = UnityEngine.Random.Range (0, endOfSessionSoundsNames.Length);
        string chosenSound  = endOfSessionSoundsNames[index];
        return PlayVoiceLine(chosenSound);
    }

    // When a Voice button is pressed we run this method
    public void VoiceSettingsChanged(){
        if(SessionInfo.isVoiceOn()){
            _voiceIsOn = true;
        }
        else{
            _voiceIsOn = false;
        }
    }
}
