using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
     public Sound[] sounds;

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
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

        }
    }

    public void Start(){
        Play("Sunny Sunday");
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
}
