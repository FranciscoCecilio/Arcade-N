using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    AudioSource[] allSources;

    public Sound GetSound(string name){
        Sound s =  Array.Find(sounds, sound => sound.name == name);
        return s;
    }

    void Awake(){
        // We want the music to not stop when we enter a new exercise 
        GameObject[] soundManagersInScene = GameObject.FindGameObjectsWithTag("SoundManager");
        if(soundManagersInScene.Length > 1){
            Destroy(this.gameObject);
        }
        else{
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

    public void Start(){
        // We only want to play music if the user wants it
        if(SequenceManager.isMusicOn){
            //Play("Sunny Sunday");
        }
    }
    
    public void Play(string name){
        Sound s = GetSound(name);
        if( s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name){
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

}
