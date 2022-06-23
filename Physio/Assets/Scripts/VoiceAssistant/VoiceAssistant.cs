using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
 using System.Collections;
 
// Used in Main Menu, Narrative Menu, all Exercises,
public class VoiceAssistant : MonoBehaviour
{
    public Sound[] sounds;
    List<AudioSource> allSources;

    public string[] byeSoundsNames; // played when we logout
    public string[] endOfSessionSoundsNames; // played on the session rewards scene

    [Header("For Debug")]
    [SerializeField] bool _voiceIsOn;

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
    }

    // returns the lenght of the clip being played
    public float PlayVoiceLine(string name){
        if(!_voiceIsOn) return 5;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return 5f;
        }
        s.source.PlayOneShot(s.clip);
        Debug.Log("Audio clip length : " + s.source.clip.length);
        
        if(s.source.clip.length == 0) return 5;
        return s.source.clip.length;
    }

    public float PlayRandomBye(){
        // Play one of the "bye" sounds randomly
        int index = UnityEngine.Random.Range (0, byeSoundsNames.Length);
        string chosenSound  = byeSoundsNames[index];
        return PlayVoiceLine(chosenSound);
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
