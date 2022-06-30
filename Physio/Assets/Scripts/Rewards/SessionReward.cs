using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SessionReward : MonoBehaviour
{
    public TMP_Text Duration;
    public TMP_Text Performance;

    public Image Medal;
    public GameObject Pedro;
    [Header("0 gold ; 1 silver ; 3 bronze")]
    public Sprite[] medalSprites; 
    
    SoundManager soundManager;
    public VoiceAssistant voiceAssistant;

    // Start is called before the first frame update
    void Start()
    {
        // play festive sounds
        // Find SoundManager
        if(soundManager == null){
            GameObject soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
            if(soundManagerObj == null){
                Debug.LogWarning("ERROR: could not find a SoundManager in this scene!");
            }
            else{
                soundManager = soundManagerObj.GetComponent<SoundManager>();
                soundManager.PlayOneShot("end_of_session");
            }
        }

        // play voice assistant 
        voiceAssistant.PlayRandomEndOfSession();

        // Performance and Duration
        CalculateParameters();

        // Animate medal
        Vector3 initScale = Medal.gameObject.transform.localScale ;
        Medal.gameObject.transform.localScale = new Vector3(0,0,0);
        LeanTween.scale(Medal.gameObject, initScale, 3f).setEase(LeanTweenType.easeOutElastic);   
        LeanTween.scale(Medal.gameObject, initScale + new Vector3(0.01f, 0.01f, 0.01f), 1f).setDelay(3f).setLoopType(LeanTweenType.pingPong);
        // Animate Pedro
        float initialY = Pedro.transform.localPosition.y;
        float initialX = Pedro.transform.localPosition.x;
        Vector3 initScaleP = Pedro.transform.localScale ;
        LeanTween.moveLocalY(Pedro, -500 ,0f);
        LeanTween.moveLocalY(Pedro, initialY , 1f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocalX(Pedro, initialX + 5f, 3f).setDelay(1f).setLoopType(LeanTweenType.pingPong);
        LeanTween.moveLocalY(Pedro, initialY + 0.3f, 3f).setDelay(1f).setLoopType(LeanTweenType.pingPong);
        LeanTween.scale(Pedro, initScaleP + new Vector3(0.004f, 0.004f, 0.004f), 3f).setDelay(1f).setLoopType(LeanTweenType.pingPong);

    }

    void CalculateParameters(){
        List<double> seqPerformances = new List<double>();
        List<string> seqDurations = new List<string>();
        for(int i = 0; i < SequenceManager.sequencesToRun.Count ; i++){
            Sequence seq = SequenceManager.sequencesToRun[i];
            Debug.Log("Adding performance: " +seq.getPerformance() * 100+ " duration: " + seq.getTotalDuration()+" on seq number" + i);
            seqPerformances.Add(seq.getPerformance() * 100);
            seqDurations.Add(seq.getTotalDuration());
        }

        // Calculate total time ----------------
        string expression;
        TimeSpan totalTime = TimeSpan.Zero;
        // Format: HH:MM:SS horas
        for(int i = 0; i < seqDurations.Count; i++){
            expression = seqDurations[i].Split(' ')[0]; //HH:MM:SS 
            TimeSpan ts = TimeSpan.ParseExact(expression, "hh\\:mm\\:ss", System.Globalization.CultureInfo.InvariantCulture);
            totalTime += ts;
        }
        Debug.Log("totalTime_: " + totalTime);
        // assign the variable (finally)        
        Duration.text = string.Format("{0:D2}:{1:D2}:{2:D2}", totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
     
        // Calculate total performance ----------------
        double average = seqPerformances.Count > 0 ? seqPerformances.Average() : 0.0;      
        double rounded_avg = Math.Round(average, 2); //rounds 1.5362 to 1.54
        Performance.text = "Performance: " + (rounded_avg).ToString() + " %";
        // change medal image

        if(rounded_avg < 50f) Medal.sprite = medalSprites[2];
        else if(rounded_avg < 80f) Medal.sprite = medalSprites[1];
        else Medal.sprite = medalSprites[0];

        SequenceManager.ResetAfterEndOfSession();
    }


    public void LoadMainMenu(){
        if(soundManager) soundManager.PlayOneShot("button_click1");
        SceneManager.LoadScene("MainMenu");
    }
    
}
