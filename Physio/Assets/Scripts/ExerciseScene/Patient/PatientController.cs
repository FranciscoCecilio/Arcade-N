using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientController : MonoBehaviour {
    public Text sessionTimePatient;
    public Text correctRepetitionsPatient;
    public Text setTimePatient;
    public Text restTimePatient;
    public Text startCounterPatient;

    public Text sessionTimeTherapist;
    public Text correctRepetitionsTherapist;
    public Text setTimeTherapist;
    public Text restTimeTherapist;
    public Text triesTherapist;
    public Text totalReps;

    public GameObject restPatient;
    public GameObject restTherapist;
    public GameObject completed;

    private int sessionTimeInt;
    private int setTimeInt;
    private int restTimeInt;
    private int startCounterInt;
     
    void init() {
        totalReps.text = "" + State.exercise.getNReps();
        restPatient.SetActive(false);
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
        sessionTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        sessionTimeTherapist.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        State.sessionTimeInt = sessionTimeInt;
    }

    private void activateRest() {
        restPatient.SetActive(true);
        restTherapist.SetActive(true);
        State.isTherapyOnGoing = false;
        State.restCount++;
        initRestTimer();
    }

    private void activateSet() {
        restPatient.SetActive(false);
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
        setTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
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
        restTimePatient.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        restTimeTherapist.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


    private void OnEnable() {
        init();
    }

    private void OnDisable() {
        CancelInvoke();
    }

    // Use this for initialization
    void Start() {
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
        completed.SetActive(State.exercise.isCompleted());

        correctRepetitionsPatient.text = "" + State.exercise.getCorrectReps();
        correctRepetitionsTherapist.text = "" + State.exercise.getCorrectReps();
        triesTherapist.text = "" + State.exercise.getTries();
    }
}
