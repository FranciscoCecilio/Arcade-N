using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTweeningManager : MonoBehaviour
{
    [SerializeField] GameObject mainImage, buttonsPanel, text1, button1, text2, button2, text3, button3, text4, button4;
    private Vector3 mainImgInit;
    private Vector3 buttonsPanelInit;

    void Start()
    {
        // we only want to play animation when we come from Login
        if(LastScene._lastSceneIndex != 0) return;
        // get their initial positions
        mainImgInit = mainImage.transform.localPosition;
        buttonsPanelInit = buttonsPanel.transform.localPosition;
        // hardcode the postion and scale of the Profile
        mainImage.transform.localScale = new Vector3 (0,0,0);
        mainImage.transform.localPosition = mainImgInit + new Vector3 (0,-100,0);
        // hardcode the postion of the buttons panel
        buttonsPanel.transform.localPosition = buttonsPanelInit + new Vector3 (0,-380,0);
        button1.transform.localScale = new Vector3 (0,0,0);
        button2.transform.localScale = new Vector3 (0,0,0);
        button3.transform.localScale = new Vector3 (0,0,0);
        button4.transform.localScale = new Vector3 (0,0,0);
        text1.transform.localScale = new Vector3 (0,0,0);
        text2.transform.localScale = new Vector3 (0,0,0);
        text3.transform.localScale = new Vector3 (0,0,0);
        text4.transform.localScale = new Vector3 (0,0,0);

        // Profile appears on the center
        LeanTween.scale(mainImage, new Vector3(.8f,.8f,.8f), 1f).setEase(LeanTweenType.easeOutBack);
        // Goes to the top and reduces scale          
        LeanTween.scale(mainImage, new Vector3(.7f,.7f,.7f), 1f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);          
        LeanTween.moveLocal(mainImage, mainImgInit, 1f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(TweenMainButtons);
    }

    void TweenMainButtons(){
        // the buttons panel comes in from bellow
        LeanTween.moveLocal(buttonsPanel, buttonsPanelInit, 0f).setEase(LeanTweenType.easeOutCirc);
        // each one of the buttons shake
        LeanTween.scale(button1, Vector3.one ,2f).setDelay(.3f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(button2, Vector3.one ,2f).setDelay(.4f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(button3, Vector3.one ,2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(button4, new Vector3(.7f,.7f,.7f) ,2f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic); // quit button stays smaller
        // texts
        LeanTween.scale(text1, Vector3.one ,1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(text2, Vector3.one ,1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(text3, Vector3.one ,1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(text4, Vector3.one ,1f).setDelay(.6f).setEase(LeanTweenType.easeOutElastic);

    }
    
}
