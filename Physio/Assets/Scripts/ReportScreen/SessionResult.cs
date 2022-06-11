using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SessionResult : MonoBehaviour
{
    public TMP_Text listIndexText;
    public TMP_Text dateText;
    public TMP_Text durationText;
    public TMP_Text performanceText;

    // session folder path
    public string folderPath;
    
    // Indexes
    int _listIndex;

    // Tween Manager
    Results_TweenManager tweenManager;
    [SerializeField] bool isInList = true;

    // Start is called before the first frame update
    void Start()
    {
        //PopulateExerciseListElement();
    }

    void PopulateSessionListElement(){
        listIndexText.text = "5";
        dateText.text = "20 Maio / 12h45";
        durationText.text = "01:25:13";
        performanceText.text = "85%";
    }

    public void OpenSessionPanel(){
        // play animation
        if(tweenManager == null){
            tweenManager = GameObject.FindGameObjectWithTag("TweenManager").GetComponent<Results_TweenManager>();
        }
        if(isInList){
            tweenManager.OpenSpecificSession();
        }
        else{
            tweenManager.ReturnToAllSessions();
        }
    }

    // called when a button is instantiated after clicking a session in list
    public void SetIsInList(bool intention){
        isInList = intention;
    }
    
}
