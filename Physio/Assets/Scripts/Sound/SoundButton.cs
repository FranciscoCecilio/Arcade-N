using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// All sound buttons should be active on start
public class SoundButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("What button is this? Tell the script!")]
    public bool isMusicButton;
    public bool isVoiceButton;
    public bool isMuteButton;

    [Header("Are we on the exerciseScene?")]
    public bool isExerciseScene;

    public GameObject otherButton;

    public GameObject white;
    float tweenTime = 0.2f;
    SoundManager soundManager; // if the user clicks on the music buttons, we tell SoundManager
    public VoiceAssistant voiceAssistant; // one per scene

    // Start is called before the first frame update
    void Start()
    {
        if(!isExerciseScene) white.SetActive(false);
        // Make it On or Off depending on sessionInfo and tell it to SoundManager after finding it
        if(isMusicButton){
            if(SessionInfo.isMusicOn()){
                if(!isMuteButton){
                    otherButton.SetActive(false);
                }
                else{
                    this.gameObject.SetActive(false);
                }
            }
            else{
                if(!isMuteButton){
                    otherButton.SetActive(true);
                }
                else{
                    this.gameObject.SetActive(true);
                }
            }
        }
        else if(isVoiceButton){
            if(SessionInfo.isVoiceOn()){
                if(!isMuteButton){
                    otherButton.SetActive(false);
                }
                else{
                    this.gameObject.SetActive(false);
                }
            }
            else{
                if(!isMuteButton){
                    otherButton.SetActive(true);
                }
                else{
                    this.gameObject.SetActive(true);
                }
            }
        }
        else{
            Debug.Log("ERROR: we have to assign the button identity in the editor");
        }
        //SessionInfo.
        // fetch correct size

    }

    public void ScaleSideway(){
        white.SetActive(true);
        LeanTween.cancel(white);
        // scale width
        LeanTween.scale(white, new Vector3(1, 0.5f, 1), tweenTime).setEase(LeanTweenType.easeInBack);
        white.transform.localScale = new Vector3(1,0,0);
        //scale height
        LeanTween.scale(white, new Vector3(1, 1, 1), tweenTime).setDelay(tweenTime).setEase(LeanTweenType.easeInBack);
    }

    public void ScaleBack(){
        white.SetActive(false);
        white.transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(isExerciseScene) return;
        ScaleSideway();
    }

    public void OnPointerExit(PointerEventData eventData){
        if(isExerciseScene) return;
        ScaleBack();
    }

    // called by clicking the button 
    public void OnClick(){
        // update SessionInfo and SoundManager
        if(isMusicButton){
            // Set sessionInfo variable
            if(!isMuteButton){ // OFF
                SessionInfo.setMusic(false);
            }
            else{ // ON
                SessionInfo.setMusic(true);
            }
        }
        else if(isVoiceButton){
            // Set sessionInfo variable
            if(!isMuteButton){ // OFF
                SessionInfo.setVoice(false);
            }
            else{ // ON
                SessionInfo.setVoice(true);
            }
        }
        else{
            Debug.Log("ERROR: we have to assign the button identity in the editor");
        }
        // Update SoundManager
        if(soundManager == null){
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
            if(soundManager == null){
                Debug.Log("ERROR: could not find a SoundManager in this scene!");
            }
        }
        // Change settings
        if(isMusicButton){
            soundManager.MusicSettingsChanged();
        }
        else if(isVoiceButton && voiceAssistant != null){
            voiceAssistant.VoiceSettingsChanged();
        }
        
        // Play Sound
        soundManager.PlayOneShot("button_click2");
        
        // Show and Hide buttons
        otherButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
