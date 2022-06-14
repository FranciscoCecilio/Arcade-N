using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used in Main Menu, Narrative Menu, all Exercises,
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    AudioSource[] allSources;

    private bool sm_musicIsOn;
    private bool sm_voiceIsOn;

    public Sound GetSound(string name){
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        return s;
    }

    void Awake(){
        // We want the music to not stop when we enter a new exercise 
        GameObject[] soundManagersInScene = GameObject.FindGameObjectsWithTag("SoundManager");
        string scene = SceneManager.GetActiveScene().name;

        if(soundManagersInScene.Length > 1){
            // If we are entering Main Menu from a scene that has a SoundManager (i.e ExerciseScene) we want to delete the last
            if(scene == "MainMenu"){
                for(int i = 0; i < soundManagersInScene.Length; i++){
                    if(soundManagersInScene[i] != this.gameObject){
                        Destroy(soundManagersInScene[i]);
                    }
                }
            }
            // If we are loading another scene, we want to destroy that scene's soundManager
            else{
                Destroy(this.gameObject);
            }
        }
        // if we got here, we want this SoundManager to DontDestroyOnLoad
        if ( scene == "MainMenu" || scene == "Exercise1Scene" || scene == "Exercise2Scene" || scene == "Exercise0Scene" ){
            DontDestroyOnLoad(gameObject);
        }

        // Initializae all sounds
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            //allSources.Add
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

        }
    }
    public string musicToPlayOnStart; 
    public void Start(){
        sm_musicIsOn = SessionInfo.isMusicOn();
        sm_voiceIsOn = SessionInfo.isVoiceOn();
        // We only want to play music if the user wants it
        if(sm_musicIsOn && !musicToPlayOnStart.Equals(string.Empty)){
            //Play("Sunny Sunday");
            Play(musicToPlayOnStart);
        }
    }
    
    public void Play(string name){
        if(!sm_musicIsOn) return;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name){
        if(!sm_musicIsOn) return;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    // called by the Music button
    public void MuteMusic(){
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach(AudioSource a in audios){
            //if(a.isPlaying)
                //audioData.Pause();
        }
    }

    // called by the Music button
    public void PlayMusicAgain(){
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach(AudioSource a in audios){
                //audioData.UnPause();
        }
    }

    // When a Music button is pressed we run this method
    public void SettingsChanged(){
        if(SessionInfo.isMusicOn()){
            //if we are playing, 
            if(sm_musicIsOn){
                // keep playing: Do nothing
            }
            else{
                // start some music
                sm_musicIsOn = true;
                Play(musicToPlayOnStart);
            }
        }
        else{
            //if we are playing
            if(sm_musicIsOn){
                // stop playing
                sm_musicIsOn = false;
                MuteMusic();
            }
            else{
                // keep not playing: Do nothing
            }
        }
        if(SessionInfo.isVoiceOn()){
            if(SessionInfo.isMusicOn()){
                //if we are playing, 
                if(sm_voiceIsOn){
                    // keep playing: Do nothing
                }
                else{
                    // start some music
                    sm_voiceIsOn = true;
                    Play(musicToPlayOnStart);
                }
            }
        }
        else{
            //if we are playing
            if(sm_voiceIsOn){
                // stop playing
                sm_voiceIsOn = false;
                MuteMusic();
            }
            else{
                // keep not playing: Do nothing
            }
        }
    }
}
