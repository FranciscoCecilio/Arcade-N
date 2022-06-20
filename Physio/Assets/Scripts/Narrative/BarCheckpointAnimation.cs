using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// when a bar checkpoint is unlocked in the ex_scene we want it to shake until its unlocked in the narrative scene.
// STARS are supossed to be setActive(false) on start
public class BarCheckpointAnimation : MonoBehaviour
{
    [Header("Animate")]
    public GameObject circle;
    public GameObject littleStar1;
    public GameObject littleStar2;
    public GameObject littleStar3;
    public Color green;

    [Header("Narrative")]
    public Image chapterImage; 

    public void StartAnimationLoop(){
        
        // Play joyfull unlocking sound
        SoundManager soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        if(soundManager == null){
            Debug.Log("ERROR: could not find a SoundManager in this scene!");
        }
        soundManager.PlayOneShot("bar_checkpoint");
        
        // set the starts visible
        littleStar1.SetActive(true);
        littleStar2.SetActive(true);
        littleStar3.SetActive(true);

        // change the circle color from white to green
        circle.GetComponent<Image>().color = green; // green

        // shake it like you mean it
        LeanTween.moveLocalY(circle, 0.0002f, 0.5f).setEase(LeanTweenType.easeShake).setRepeat(-1);

        // rotate (little stars)
        LeanTween.rotateAround(littleStar1, -Vector3.forward, 360, 2.5f).setRepeat(-1);
        LeanTween.rotateAround(littleStar2, -Vector3.forward, 360, 2.5f).setRepeat(-1);
        LeanTween.rotateAround(littleStar3, -Vector3.forward, 360, 2.5f).setRepeat(-1);
    }
    
    void Start()
    {
        //StartAnimationLoop();
    }
}