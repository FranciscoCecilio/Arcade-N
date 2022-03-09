using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseManager : MonoBehaviour {

    public GameObject cursor;
    public bool hasSecondaryCursor;

    public Text avgTime;
    public Text lastrepTime;

    private int lastrep = 0;

    public string exerciseName;

    public GameObject leftExerciseBox;
    public GameObject rightExerciseBox;
    private GameObject exerciseBoxGroup;

    public GameObject leftTargets;
    public GameObject rightTargets;
    private GameObject targets;

    private AudioClip beep;
    private AudioSource audioSource;

    private bool reversePath;

    private bool isGroing;

    private bool hasRegisteredOutOfPath;
    private bool isBlinking;

    private bool showArrows = false;
    private GameObject[] _arrows;

    private int repCounter;

    public Toggle restartRepToggle;

    public GameObject wellDoneMessage;

    public GameObject pathSize;

    // Use this for initialization
    void Start () {
    }

    void init() {
        State.hasSecondaryCursor = hasSecondaryCursor;
        State.currentTarget = 0;
        State.hasStartedExercise = false;
        reversePath = false;
        setArea();

        beep = (AudioClip)Resources.Load("Sounds/beep");
        audioSource = GetComponent<AudioSource>();

        Renderer renderer = targets.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.r = color.b = 0;
        color.g += 1;

        renderer.material.color = color;
    }

    private void OnEnable() {
        init();
    }

    private void OnDisable() {
        CancelInvoke();
    }

    //Choose the arm area
    private void setArea() {
        if (State.exercise.isLeftArm()) {
            activate(true);
            exerciseBoxGroup = leftExerciseBox;
            targets = leftTargets;
        }
        else if (!State.exercise.isLeftArm()) {
            activate(false);
            exerciseBoxGroup = rightExerciseBox;
            targets = rightTargets;
        }
    
        //Detect arrows
        _arrows = GameObject.FindGameObjectsWithTag("Arrow");
        for (int i = 0; i < _arrows.Length; i++)
        {
            _arrows[i].SetActive(false);
        }
        showArrows = false;
    }

    private void activate(bool left) {
        leftTargets.SetActive(left);
        leftExerciseBox.SetActive(left);
        rightTargets.SetActive(!left);
        rightExerciseBox.SetActive(!left);
    }

    // Update is called once per frame
    void Update () {
        /*GameObject[] target = GameObject.FindGameObjectsWithTag("TargetCollider");
        Debug.Log(target[0].transform.position.ToString());*/

        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.back);

        if(State.isTherapyOnGoing) {

            //Show arrows
            if (!showArrows)
            {
                for (int i = 0; i < _arrows.Length; i++)
                {
                    _arrows[i].SetActive(true);
                }
                showArrows = true;
            }

            if(!isBlinking) {
                InvokeRepeating("blinkTarget", 0, 0.05f);
                isBlinking = true;
            }

            if (Physics.Raycast(landingRay, out hit)) {
                if (hit.collider.tag == "ExerciseCollider") { // is inside of the exercise area
                    if (State.hasStartedExercise) {
                        foreach (Transform exerciseBox in exerciseBoxGroup.transform) {
                            exerciseBox.gameObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
                        }
                    }
                }
                if (hit.collider.tag == "TargetCollider") { // has hit a target
                    bool hasHitTheCurrentTarget = hit.transform.position == targets.transform.GetChild(State.currentTarget).transform.position;

                    if (hasHitTheCurrentTarget) {

                        hasRegisteredOutOfPath = false;

                        if (!State.hasStartedExercise) {
                            State.hasStartedExercise = true;
                        }

                        targets.transform.GetChild(State.currentTarget).gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                        if (State.currentTarget == (targets.transform.childCount - 1)) {
                            reversePath = true;
                            State.currentTarget--;
                        }

                        else if (reversePath && State.currentTarget == 0) {
                            State.currentTarget = 0;
                            State.exercise.incTries();
                            State.exercise.incCorrectReps();
                            reversePath = false;

                            int minutes = (State.sessionTimeInt / State.exercise.getCorrectReps()) / 60;
                            int seconds = (State.sessionTimeInt / State.exercise.getCorrectReps()) % 60;

                            int lastm = (State.sessionTimeInt - lastrep) / 60;
                            int lasts = (State.sessionTimeInt - lastrep) % 60;

                            avgTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
                            State.exercise.setAvgTime(avgTime.text);

                            lastrepTime.text = lastm.ToString("00") + ":" + lasts.ToString("00") + " m";

                            lastrep = State.sessionTimeInt;

                            if (State.exercise.getCorrectReps() >= State.exercise.getNReps()) { // done all the needed reps
                                State.exercise.setTotalTime(State.sessionTimeInt);
                                State.exercise.setCompleted(true);
                                State.isTherapyOnGoing = false; //???
                                State.resetState();
                                StartCoroutine(showWellDoneMessage());
                            }
                        }

                        else if (!reversePath) {
                            State.currentTarget++;
                        }

                        else if (reversePath) {
                            State.currentTarget--;
                        }

                        audioSource.PlayOneShot(beep);
                    }
                }
            }
            else { // the cursor isn't inside of the area
                if (State.hasStartedExercise) {
                    foreach (Transform exerciseBox in exerciseBoxGroup.transform) {
                        exerciseBox.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
                    }
                    if (!hasRegisteredOutOfPath) {
                        State.exercise.incOutOfPath() ;
                        State.exercise.incTries();
                        hasRegisteredOutOfPath = true;
                    }
                    State.compensationInCurrentRep = true;
                }
            }

            if (State.compensationInCurrentRep && restartRepToggle.GetComponent<Toggle>().isOn) { // if is detected a compensation we restart the repetition
                State.compensationInCurrentRep = false;
                reversePath = false;
                if(State.currentTarget != 0) {
                    targets.transform.GetChild(State.currentTarget).gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                }
                State.currentTarget = 0;
            }
        }
    }

    private void blinkTarget() {
        if (targets.transform.childCount == 0)
            return;

        Renderer renderer = null;
        int target = State.currentTarget;
        while (renderer == null) {
            Renderer rendererTemp = targets.transform.GetChild(target).gameObject.GetComponent<Renderer>();

            if (rendererTemp.enabled) {
                renderer = rendererTemp;
            }
            else if (reversePath) {
                target--;
            }
            else {
                target++;
            }
        }

        float delta;

        if (isGroing && renderer.material.color.r > 1) {
            isGroing = false;
        }
        else if (!isGroing && renderer.material.color.r < 0) {
            isGroing = true;
        }

        if (isGroing) {
            delta = 0.1f;
        }
        else {
            delta = -0.1f;
        }

        Color color = renderer.material.color;
        color.r += delta;
        color.b += delta;

        renderer.material.color = color;
        
        for (int i = 0; i < _arrows.Length; i++)
        {
            if (target == 0 && _arrows[i].transform.parent.name == "UpArrows") _arrows[i].SetActive(false);
            if (target == 1 && _arrows[i].transform.parent.name == "UpArrows") _arrows[i].SetActive(true);
            if (target == 0 && _arrows[i].transform.parent.name == "DownArrows") _arrows[i].SetActive(true);
            if (target == 1 && _arrows[i].transform.parent.name == "DownArrows") _arrows[i].SetActive(false);
        }
    }

    private IEnumerator showWellDoneMessage()
    {
        leftTargets.SetActive(false);
        leftExerciseBox.SetActive(false);
        rightTargets.SetActive(false);
        rightExerciseBox.SetActive(false);
        wellDoneMessage.SetActive(true);
        yield return new WaitForSeconds(5);
        wellDoneMessage.SetActive(false);
    }

    public void changePathSize()
    {
        foreach (Transform child in exerciseBoxGroup.transform)
        {
            child.localScale = new Vector3(pathSize.GetComponent<Slider>().value, child.localScale.y, child.localScale.z);
        }
    }
}
