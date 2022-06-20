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

    public string[] byeSoundsNames;

    [Header("For Debug")]
    [SerializeField] bool sm_voiceIsOn;

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
        sm_voiceIsOn = SessionInfo.isVoiceOn();
    }

    // it returns the lenght of the clip being played
    public float PlayVoiceLine(string name){
        //if(!sm_musicIsOn) return;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return 0f;
        }
        s.source.PlayOneShot(s.clip);
        Debug.Log("Audio clip length : " + s.source.clip.length);
        return s.source.clip.length;
    }

    public float PlayRandomBye(){
        // Play one of the "bye" sounds randomly
        int index = UnityEngine.Random.Range (0, byeSoundsNames.Length);
        string chosenByeSound  = byeSoundsNames[index];
        return PlayVoiceLine(chosenByeSound);
    }

    // When a Voice button is pressed we run this method
    public void VoiceSettingsChanged(){
        // Music -----------------------------------
        /*AudioSource music = null;
        foreach(AudioSource a in allSources){
            if(a.name.Equals(musicToPlayOnStart)){
                music = a; // get original music
            }
        }
        // Music is playing
        if(music != null && music.isPlaying){
            if(SessionInfo.isMusicOn()){
                // Do Nothing: keep playing the music
                sm_musicIsOn = true;
            }
            else{
                // Pause the music
                music.Pause();
                sm_musicIsOn = false;
            }
        }
        // Music is not playing
        else{
            if(SessionInfo.isMusicOn()){
                // Then we need to Unpause the music or Play it from start
                if(music.time > 0){
                    music.UnPause();
                }
                else{
                    Play(music.name);
                }
                sm_musicIsOn = true;
            }
            else{
                // Do Nothing: don't play anithghing
                sm_musicIsOn = false;
            }
        }*/
    }
}
