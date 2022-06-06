using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Class responsible for invoking the Countdowns
public class TimeVis : MonoBehaviour {
    [Header("Countdown on Start")]
    public TMP_Text counterOnStart;

    [Header("Session Time")]
    public TMP_Text sessionTimeCounter;

    [Header("Timer until next Rest")]
    public TMP_Text timeUntilNextRestCounter;

    [Header("Is Resting")]
    public GameObject isRestingMessage;
    public TMP_Text isRestingCounter;

    private int sessionTimeInt;
    private int timeUntilNextRest;
    private int restTimeInt;
    private int startCounterInt;
    private bool wasInitialized = false;
    
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
        if (State.exercise.isCompleted()) {
            counterOnStart.transform.gameObject.SetActive(false);
            CancelInvoke("countDown");
            CancelInvoke("restTimeDec");
            CancelInvoke("setTimeDec");
            CancelInvoke("sessionTimeInc");
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
        // turn off the rest message
        isRestingMessage.SetActive(false);
        // start the countdown on start
        startCounterInt = 4;
        counterOnStart.transform.gameObject.SetActive(true);
        InvokeRepeating("initialCountDown", 0, 1);
    }

    private void initialCountDown() {
        if (startCounterInt <= 0) {
            // start the session timer
            initSessionTime(); // goes up
            initUntilNextRestTimer(); // goes down
            counterOnStart.transform.gameObject.SetActive(false);
            State.isTherapyOnGoing = true;
            CancelInvoke("initialCountDown");
        }
        startCounterInt--;
        counterOnStart.text = "" + startCounterInt;
    }

    // Starts the session timer (goes Up)
    private void initSessionTime() {
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
        isRestingMessage.SetActive(true);
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
        isRestingMessage.SetActive(false);
        State.isTherapyOnGoing = true;
        initUntilNextRestTimer();
    }

    
}
