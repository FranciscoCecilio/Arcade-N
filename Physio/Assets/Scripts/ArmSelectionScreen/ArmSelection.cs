using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArmSelection : MonoBehaviour {

    public Text title;

    void Start()
    {
        if (SessionInfo.toView() == "SequenceCreat")
        {
            title.text = "Select the arm for the exercise";
        }
    }

	public void selectArm(bool isLeft)
    {
        if (State.exercise != null)  State.exercise.setArm(isLeft);
        if (SessionInfo.toView() == "Exercise") SceneManager.LoadScene("ExerciseParametersSelection");
        else if (SessionInfo.toView() == "SequenceCreat") SceneManager.LoadScene("ExerciseParametersSelection");
        //else if (SessionInfo.toView() == "Results") SceneManager.LoadScene("ReportScreen");
    }

    public void previousScreen()
    {
        SceneManager.LoadScene("ExerciseSelection");
    }
}
