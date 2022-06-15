﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Used in Main Menu, Narrative Menu, all Exercises,
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    List<AudioSource> allSources;

    [SerializeField] bool sm_musicIsOn;
    [SerializeField] bool sm_voiceIsOn;

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
    public string musicToPlayOnStart; 
    public void Start(){
        sm_musicIsOn = SessionInfo.isMusicOn();
        Debug.Log("seinfo: " + SessionInfo.isMusicOn());
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
        //if(!sm_musicIsOn) return;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    public void PlayVoiceLine(string name){
        //if(!sm_musicIsOn) return;
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    // called by the Music button
    public void MuteMusic(){
        foreach(AudioSource a in allSources){
            //if(a.isPlaying)
                //audioData.Pause();
        }
    }

    // called by the Music button
    public void PlayMusicAgain(){
        foreach(AudioSource a in allSources){
                //audioData.UnPause();
        }
    }

    // When a Music button is pressed we run this method
    public void MusicSettingsChanged(){
        // Music -----------------------------------
        AudioSource music = null;
        foreach(AudioSource a in allSources){
            Debug.Log(a.clip.name + " vs " + musicToPlayOnStart);
            if(a.clip.name.Equals(musicToPlayOnStart)){
                music = a; // get original music
                break;
            }
        }
        if(music == null){
            Debug.Log("ERROR: Starting Music was not found");
            return;
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
                sm_musicIsOn = true;
                Debug.Log("music.time: " + music.time);
                if(music.time > 0){
                    music.UnPause();
                }
                else{
                    Play(music.name);
                }
            }
            else{
                // Do Nothing: don't play anithghing
                sm_musicIsOn = false;
            }
        }
    }

    // When a Voice button is pressed we run this method
    public void VoiceSettingsChanged(){
        // Music -----------------------------------
        AudioSource music = null;
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
        }
    }
}
