using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// when a bar checkpoint is unlocked in the ex_scene we want it to shake until its unlocked in the narrative scene.
// STARS are supossed to be setActive(false) on start
public class BarCheckpointAnimation : MonoBehaviour
{

    bool isAnimating = true;

    public GameObject Circle;
    public GameObject littleStar1;
    public GameObject littleStar2;
    public GameObject littleStar3;

    public void StartAnimationLoop(){
        isAnimating = true;
        littleStar1.SetActive(true);
        littleStar2.SetActive(true);
        littleStar3.SetActive(true);

         // shake it like you miss it :p
        //LeanTween.moveY(Circle, 0.4f, 0.2f).setEase(LeanTweenType.easeShake).setLoopClamp(-1);
        // release particles (little stars)
        /*LeanTween.rotateAround(littleStar1, Vector3.forward, 0, 2.5f).setLoopClamp(-1);
        LeanTween.rotateAround(littleStar2, Vector3.forward, 0, 2.5f).setLoopClamp(-1);
        LeanTween.rotateAround(littleStar3, Vector3.forward, 0, 2.5f).setLoopClamp(-1);*/
    }
    
    void Start()
    {
        StartAnimationLoop();
    }
}
