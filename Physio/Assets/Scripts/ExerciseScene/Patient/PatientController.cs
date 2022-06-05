using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TODO Francisco - do we want to have patient stuff? ?
public class PatientController : MonoBehaviour {
    public GameObject completed;
    [Header("Repetitions")]
    public Text correctRepetitionsPatient;
    public Text correctRepetitionsTherapist;
    public Text triesTherapist;
    public Text totalReps;
    
    [Header("Rest Time")]
    public TMP_Text restTimeTherapist;
    public Text setTimeTherapist;
    public Text setTimePatient;
    public Text restTimePatient;
    public GameObject restPatient;
    public GameObject restTherapist;
    public Text sessionTimePatient;
    public TMP_Text sessionTimeTherapist;
    public TMP_Text startCounterPatient;

    private int sessionTimeInt;
    private int setTimeInt;
    private int restTimeInt;
    private int startCounterInt;
     
    private void OnEnable() {
        init();
    }

    private void OnDisable() {
        Debug.Log("Disabled PatientController");
        CancelInvoke();
    }

    void init() {
        //totalReps.text = "" + State.exercise.getNReps();
        //restPatient.SetActive(false);
        restTherapist.SetActive(false);

        startCounterInt = 4;
        InvokeRepeating("countDown", 0, 1);
    }

    private void countDown() {
        startCounterPatient.transform.gameObject.SetActive(true);
        if (startCounterInt <= 0) {
            initSessionTime();
            initSetTimer();
            startCounterPatient.transform.gameObject.SetActive(false);
            State.isTherapyOnGoing = true;
            CancelInvoke("countDown");
        }
        startCounterInt--;
        startCounterPatient.text = "" + startCounterInt;
    }

    private void initSessionTime() {
        sessionTimeInt = 0;
        InvokeRepeating("sessionTimeInc", 0, 1);
    }

    private void initSetTimer() {
        setTimeInt = State.exercise.getDuration();
        InvokeRepeating("setTimeDec", 0, 1);
    }

    private void initRestTimer() {
        restTimeInt = State.exercise.getRestTime();
        InvokeRepeating("restTimeDec", 0, 1);
    }

    private void sessionTimeInc() {
        sessionTimeInt++;
        int minutes = sessionTimeInt / 60;
        int seconds = sessionTimeInt % 60;
        // TODO Francisco - do we want to have SessionTime ?
        /*
        sessionTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        */
        sessionTimeTherapist.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        
        State.sessionTimeInt = sessionTimeInt;
    }

    private void activateRest() {
        //restPatient.SetActive(true);
        restTherapist.SetActive(true);
        State.isTherapyOnGoing = false;
        State.restCount++;
        initRestTimer();
    }

    private void activateSet() {
        //restPatient.SetActive(false);
        restTherapist.SetActive(false);
        State.isTherapyOnGoing = true;
        initSetTimer();
    }

    private void setTimeDec() {
        if (setTimeInt == 0) {
            CancelInvoke("setTimeDec");
            activateRest();
            return;
        }

        setTimeInt--;
        int minutes = setTimeInt / 60;
        int seconds = setTimeInt % 60;
        // TODO Francisco - do we want to have patient stuff? ?
        //setTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        setTimeTherapist.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void restTimeDec() {
        if (restTimeInt == 0) {
            CancelInvoke("restTimeDec");
            activateSet();
            return;
        }

        restTimeInt--;
        int minutes = restTimeInt / 60;
        int seconds = restTimeInt % 60;
        //restTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        restTimeTherapist.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    

    // Update is called once per frame
    void Update() {
        if (State.exercise.isCompleted()) {
            startCounterPatient.transform.gameObject.SetActive(false);
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
}
