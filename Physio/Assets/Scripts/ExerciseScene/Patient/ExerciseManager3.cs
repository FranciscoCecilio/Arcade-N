using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseManager3 : MonoBehaviour {

    public GameObject cursor;
    public bool hasSecondaryCursor;

    public Text avgTime;
    public Text lastrepTime;

    private int lastrep = 0;

    public string exerciseName;

    public GameObject leftExerciseBox;
    public GameObject rightExerciseBox;

    public GameObject leftTargets;
    public GameObject rightTargets;

    private int targetHitCounter = 0;
    private bool[] targetHits;
    private GameObject[] targetsArray;

    private AudioClip beep;
    private AudioSource audioSource;

    private bool isGroing;

    private bool isBlinking;

    public Toggle restartRepToggle;

    public GameObject wellDoneMessage;

    public GameObject pathSize;

    // Use this for initialization
    void Start () {
    }

    void init() {
        State.hasSecondaryCursor = hasSecondaryCursor;
        State.hasStartedExercise = false;
        activate(State.exercise.isLeftArm());

        beep = (AudioClip)Resources.Load("Sounds/beep");
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        init();
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void activate(bool left) {
        leftTargets.SetActive(left);
        rightTargets.SetActive(!left);

        targetsArray = GameObject.FindGameObjectsWithTag("TargetCollider");
        targetHits = new bool[targetsArray.Length];
        for (int i = 0; i < targetHits.Length; i++) targetHits[i] = false;
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);

        if(State.isTherapyOnGoing) {

            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }

            if (Physics.Raycast(landingRay, out hit)) {

                if (hit.collider.tag == "TargetCollider") { // has hit a target

                    bool hasHitTheCurrentTarget = false;
                    int targetIndex = 0;
                    for (int i = 0; i < targetsArray.Length; i++)
                    {
                        hasHitTheCurrentTarget = hit.transform.position == targetsArray[i].transform.position;
                        if (hasHitTheCurrentTarget)
                        {
                            targetIndex = i;
                            break;
                        }
                    }

                    if (!targetHits[targetIndex])
                    {
                        targetHits[targetIndex] = true;

                        if (!State.hasStartedExercise)
                        {
                            State.hasStartedExercise = true;
                        }

                        targetsArray[targetIndex].GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 1);

                        targetHitCounter++;

                        if (targetHitCounter == targetsArray.Length)
                        {
                            State.exercise.incTries();
                            State.exercise.incCorrectReps();

                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;
                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;
                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);
                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";
                            lastrep = State.sessionTimeInt;

                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps())
                            { // done all the needed reps
                                State.exercise.setTotalTime(State.sessionTimeInt);
                                State.exercise.setCompleted(true);
                                State.isTherapyOnGoing = false;
                                State.resetState();
                                StartCoroutine(showWellDoneMessage());
                            } else
                            {
                                targetHitCounter = 0;
                                for (int i=0; i < targetHits.Length; i++)
                                {
                                    targetHits[i] = false;
                                    targetsArray[i].GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                                }
                            }
                        }

                        audioSource.PlayOneShot(beep);
                    }
                }
            }
        }
    }

    private void blinkTarget() {

        float delta;

        for (int i = 0; i < targetsArray.Length; i++)
        {
            if (!targetHits[i])
            {
                Renderer renderer = targetsArray[i].GetComponent<Renderer>();
                if (isGroing && renderer.material.color.r > 1)
                {
                    isGroing = false;
                }
                else if (!isGroing && renderer.material.color.r < 0)
                {
                    isGroing = true;
                }

                if (isGroing)
                {
                    delta = 0.1f;
                }
                else
                {
                    delta = -0.1f;
                }

                Color color = renderer.material.color;
                color.r += delta;
                color.b += delta;

                renderer.material.color = color;
            }
        }
    }

    private IEnumerator showWellDoneMessage()
    {
        leftTargets.SetActive(false);
        rightTargets.SetActive(false);
        wellDoneMessage.SetActive(true);
        yield return new WaitForSeconds(5);
        wellDoneMessage.SetActive(false);
    }
}
