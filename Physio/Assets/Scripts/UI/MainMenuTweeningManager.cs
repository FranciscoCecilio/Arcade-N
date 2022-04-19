using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTweeningManager : MonoBehaviour
{
    [SerializeField] GameObject mainImage, buttonsPanel, text1, button1, text2, button2, text3, button3;
    
    void Start()
    {
        if(LastScene._lastSceneIndex != 0) return;
        // hardcode the postion and scale of the Profile
        mainImage.transform.localPosition = new Vector3 (0,0,0);
        mainImage.transform.localScale = new Vector3 (0,0,0);
        // hardcode the postion of the buttons panel
        buttonsPanel.transform.localPosition = new Vector3 (0,-380,0);
        button1.transform.localScale = new Vector3 (0,0,0);
        button2.transform.localScale = new Vector3 (0,0,0);
        button3.transform.localScale = new Vector3 (0,0,0);

        // Profile appears on the center
        LeanTween.scale(mainImage, new Vector3(1f,1f,1f), 1.2f).setDelay(.5f).setEase(LeanTweenType.easeOutBack);
        // Goes to the top and reduces scale          
        LeanTween.scale(mainImage, new Vector3(.7f,.7f,.7f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);          
        LeanTween.moveLocal(mainImage, new Vector3(0,153F,0), .7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(TweenMainButtons);
    }

    void TweenMainButtons(){
        // the buttons panel comes in from bellow
        LeanTween.moveLocal(buttonsPanel, new Vector3(0, -80, 0), 1f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc);
        // each one of the buttons shake
        LeanTween.scale(button1, new Vector3(1f,1f,1f) ,2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(button2, new Vector3(1f,1f,1f) ,2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(button3, new Vector3(1f,1f,1f) ,2f).setDelay(1f).setEase(LeanTweenType.easeOutElastic);

    }
    
}
