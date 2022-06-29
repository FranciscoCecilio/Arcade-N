using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Class responsible for invoking the Countdowns
public class TimeVis : MonoBehaviour {
    [Header("Countdown on Start")]
    public GameObject counterOnStart;
    public TMP_Text counterOnStartText;

    [Header("Session Time")]
    public TMP_Text sessionTimeCounter;

    [Header("Timer until next Rest")]
    public TMP_Text timeUntilNextRestCounter;

    [Header("Is Resting")]
    public GameObject isRestingVis;
    public Image isRestingRadialBar;
    public TMP_Text isRestingCounter;

    private int sessionTimeInt;
    private int timeUntilNextRest;
    private int restTimeInt;
    private int startCounterInt;
    private bool wasInitialized = false;
    private bool timerSkip = false;
    bool exerciseCompleted = false; // just a performance flag
    
    SoundManager soundManager;
    public VoiceAssistant voiceAssistant;
    public DragEditionManager dragManager;

    private void OnEnable() {
        //init();
    }

    private void OnDisable() {
        wasInitialized = false;
        CancelInvoke();
    }

    // Update is called once per frame
    void Update() {
        if(!wasInitialized) 
            return;
        if (exerciseCompleted && State.exercise.isCompleted()) { 
            counterOnStart.transform.gameObject.SetActive(false);
            CancelInvoke("initRestCountDown");
            CancelInvoke("restTimeDec");
            CancelInvoke("setTimeDec");
            CancelInvoke("sessionTimeInc");
            exerciseCompleted = true;
        }
        if(isRestingVis.activeSelf){
            isRestingRadialBar.fillAmount = (float) (SequenceManager.RestTimeLeft.TotalSeconds / SequenceManager.GetTotalRestTime()); // 5s/60s = 1/12
        }
        //VERIFICAR ISTO TODO
        //completed.SetActive(State.exercise.isCompleted());

        // Isto já é feito no Viz controller
        //correctRepetitionsPatient.text = "" + State.exercise.getCorrectReps();
        //correctRepetitionsTherapist.text = "" + State.exercise.getCorrectReps();
        //triesTherapist.text = "" + State.exercise.getTries();
    }

    // This method is called in BodyScreen.StartTherapy()
    public void init() {
        // allow update to run
        wasInitialized = true;
        dragManager.LockEdition();

        // start the restin timer countdown (it can be non-existing if we have no time left, and in that case we jump straight to intiial countdown)
        startCounterInt = 4;
        if (SequenceManager.RestTimeLeft.TotalSeconds > startCounterInt){
            isRestingVis.SetActive(true);
            // voice line
            voiceAssistant.PlayRandomRest();
            InvokeRepeating("initRestCountDown", 0, 1);
        }
        else{
            StartCountDown();
        }
        
    }
    
    // called by the skip button
    public void SkipRestingTime(){
        timerSkip = true;
    }

    private void initRestCountDown() {
        // when resting duration is on 4 seconds to end, invoke inistial countdown
        if (SequenceManager.RestTimeLeft.TotalSeconds <= startCounterInt || timerSkip) {
            isRestingVis.SetActive(false);
            StartCountDown();
            CancelInvoke("initRestCountDown");
        }
        // show resting time vis 
        isRestingCounter.text = string.Format("{0:D2} : {1:D2}",(int) SequenceManager.RestTimeLeft.TotalMinutes, (int)SequenceManager.RestTimeLeft.TotalSeconds);
    }

    // Starts the session timer (goes Up)
    private void StartCountDown() {
        counterOnStart.gameObject.SetActive(true);
        LeanTween.scale(counterOnStart, new Vector3(1.1f, 1.1f, 1.1f), 0.5f).setLoopType(LeanTweenType.pingPong);
        InvokeRepeating("initCountDown", 0, 1);
    }

    private void initCountDown() {

        if (startCounterInt <= 0) {
            startSessionTimer(); // goes up

            //initUntilNextRestTimer(); // TODO not anymore
            LeanTween.cancel(counterOnStart); // cancel animations
            counterOnStart.SetActive(false);

            State.isTherapyOnGoing = true;
            CancelInvoke("initCountDown");
        }
        startCounterInt--;
        if(startCounterInt == 0) 
            counterOnStartText.text = "AÇÃO!";
        else 
            counterOnStartText.text = "" + startCounterInt;
    }

    // Starts the session timer (goes Up)
    private void startSessionTimer() {
        sessionTimeInt = 0;
        InvokeRepeating("sessionTimeInc", 0, 1);
    }

    // TODO Francisco - do we want to have SessionTime ?
    // Increments the session_timer every second
    private void sessionTimeInc() {
        sessionTimeInt++;
        int minutes = sessionTimeInt / 60;
        int seconds = sessionTimeInt % 60;
        sessionTimeCounter.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        // Updates the State everytime
        State.sessionTimeInt = sessionTimeInt;
    }

    // Starts the timer until next rest (goes down)
    private void initUntilNextRestTimer() {
        timeUntilNextRest = State.exercise.getDuration(); // gives the remaining time
        InvokeRepeating("untilNextRestDec", 0, 1);
    }

    // checks every second if is already time to rest
    private void untilNextRestDec() {
        if (timeUntilNextRest == 0) {
            CancelInvoke("untilNextRestDec"); // freeze time_until_next_rest 
            activateRest(); // shoes rest message
            return;
        }

        timeUntilNextRest--;
        int minutes = timeUntilNextRest / 60;
        int seconds = timeUntilNextRest % 60;
        timeUntilNextRestCounter.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // Shows the Is Resting Message and starts the resting countdown
    private void activateRest() {
        isRestingVis.SetActive(true);
        State.isTherapyOnGoing = false;
        State.restCount++;
        initIsRestingTimer();
    }
    // Starts the timer until rest is over (goes down)
    private void initIsRestingTimer() {
        restTimeInt = State.exercise.getRestTime();
        InvokeRepeating("restTimeDec", 0, 1);
    }

    // checks every second if rest time is over
    private void restTimeDec() {
        if (restTimeInt == 0) {
            CancelInvoke("restTimeDec"); // stop checking if rest time is over (because it is)
            activateSet(); // close message and restart until_next_rest counter
            return;
        }

        restTimeInt--;
        int minutes = restTimeInt / 60;
        int seconds = restTimeInt % 60;
        isRestingCounter.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // close message and restart until_next_rest counter
    private void activateSet() {
        //restPatient.SetActive(false);
        isRestingVis.SetActive(false);
        State.isTherapyOnGoing = true;
        initUntilNextRestTimer();
    }

    
}
