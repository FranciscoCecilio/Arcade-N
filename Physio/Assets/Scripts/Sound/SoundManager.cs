using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Used in all menus. These managers pass on from scene to scene. They are spawned in Main Menu and 1st Exercise scene (2 different songs for each menu)
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public bool isMainMenuSM;

    List<AudioSource> allSources;
    public string musicToPlayOnStart; 
    
    [Header("For Debugging")]
    public bool sm_musicIsOn;

    void Awake(){
        // We want the music to not stop when we enter a new exercise 
        GameObject[] soundManagersInScene = GameObject.FindGameObjectsWithTag("SoundManager");
        string scene = SceneManager.GetActiveScene().name;

        if(soundManagersInScene.Length > 1){
            // If we are entering Main Menu from a scene that has a SoundManager (i.e ExerciseScene) we want to delete the last

            if(isMainMenuSM){
                if(scene == "MainMenu"){
                    for(int i = 0; i < soundManagersInScene.Length; i++){
                        if(soundManagersInScene[i] != this.gameObject){
                            Destroy(soundManagersInScene[i]); // Delete other
                        }
                    }
                }
                else if(scene == "Exercise1Scene" || scene == "Exercise2Scene" || scene == "Exercise0Scene"){
                    Destroy(this.gameObject); // delete MainMenuSM and leave Exercise scene SM
                }
            }
            // If we are loading another scene, we want to destroy that scene's soundManager
            else{
                Destroy(this.gameObject);
            }
        }

        // We want this SoundManager to DontDestroyOnLoad
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

    public void Start(){
        sm_musicIsOn = SessionInfo.isMusicOn();
        Debug.Log("seinfo: " + SessionInfo.isMusicOn());
        // We only want to play music if the user wants it
        if(sm_musicIsOn && !musicToPlayOnStart.Equals(string.Empty)){
            //Play("Sunny Sunday");
            Play(musicToPlayOnStart);
        }
    }
    
    public Sound GetSound(string name){
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        return s;
    }

    public void Play(string name){
        Debug.Log("sm_musicOn: " + sm_musicIsOn);
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
                if(music.time > 0){
                    music.UnPause();
                }
                else{
                    Play(music.clip.name);
                }
            }
            else{
                // Do Nothing: don't play anithghing
                sm_musicIsOn = false;
            }
        }
    }
}
