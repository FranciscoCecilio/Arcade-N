using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results_TweenManager : MonoBehaviour
{
    public GameObject AllSessions; // initial position.y = 37
    private Vector3 allSessions_init;
    public GameObject AllSessions_Title;
    private Vector3 allSessions_Title_init;
    public GameObject MainInfo;
    private Vector3 mainInfo_init;

    public GameObject SpecificSession; // initial position.y = 56
    private Vector3 specificSession_init;
    public GameObject SpecificSession_Title;
    private Vector3 specificSession_Title_init;

    void Start()
    {
        // store initial positions
        allSessions_init = AllSessions.transform.localPosition;
        allSessions_Title_init = AllSessions_Title.transform.localPosition;
        mainInfo_init = MainInfo.transform.localPosition;
        specificSession_init = SpecificSession.transform.localPosition;
        specificSession_Title_init = SpecificSession_Title.transform.localPosition;

        OpenSpecificSession();
    }

    public void OpenSpecificSession(){
        // Set initial positions
        LeanTween.moveLocal(SpecificSession_Title , new Vector3(1000,specificSession_Title_init.y,0), 0);
        LeanTween.moveLocal(SpecificSession , new Vector3(specificSession_init.x,-600,0), 0);
        AllSessions.SetActive(true);
        SpecificSession.SetActive(true);
        // Move title and Main Info right
        LeanTween.moveLocal(AllSessions_Title, new Vector3(1000,allSessions_Title_init.y,0), .7f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveLocal(MainInfo         , new Vector3(1000,mainInfo_init.y,0), .7f).setEase(LeanTweenType.easeInOutCubic);
        // Move all sessions UP
        LeanTween.moveLocal(AllSessions      , new Vector3(0,600,0), .7f).setDelay(.7f).setEase(LeanTweenType.easeInOutCubic);
        // Move specific sessions UP
        LeanTween.moveLocal(SpecificSession , specificSession_init, .7f).setDelay(.7f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveLocal(SpecificSession_Title, specificSession_Title_init, .7f).setDelay(.7f).setEase(LeanTweenType.easeInOutCubic);
    }

    public void ReturnToAllSessions(){
        // Set initial positions
        LeanTween.moveLocal(AllSessions , new Vector3(0,600,0), 0);
        LeanTween.moveLocal(AllSessions_Title , new Vector3(1000,allSessions_Title_init.y,0), 0);
        LeanTween.moveLocal(MainInfo , new Vector3(1000,mainInfo_init.y,0), 0);
        AllSessions.SetActive(true);
        SpecificSession.SetActive(true);
        
        // Move Specific session Title to the side
        LeanTween.moveLocal(SpecificSession_Title, new Vector3(1000,specificSession_Title_init.y,0), .7f).setEase(LeanTweenType.easeInOutCubic);
        // Move specific sessions Down
        LeanTween.moveLocal(SpecificSession , new Vector3(0,-600,0), .7f).setDelay(.7f).setEase(LeanTweenType.easeInOutCubic);
        // Move all sessions Down
        LeanTween.moveLocal(AllSessions      , new Vector3(0,allSessions_init.y,0), .7f).setDelay(.7f).setEase(LeanTweenType.easeInOutCubic);
        
        // Move title and Main Info back
        LeanTween.moveLocal(MainInfo         , mainInfo_init, .7f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveLocal(AllSessions_Title, allSessions_Title_init, .7f).setDelay(1f).setEase(LeanTweenType.easeInOutCubic);
    }
}
