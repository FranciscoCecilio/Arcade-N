using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Old file not used anymore. We select the exercises in ExerciseParametersPanel.cs (in the Sequence Creation Scene)
public class ExerciseParametersSelection : MonoBehaviour {

    public InputField nRepsField;
    public InputField durationField;
    public InputField restTimeField;

    public void next()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")) State.exercise.setNReps(int.Parse(nRepsField.text));
            else State.exercise.setNReps(10);
        }
        else State.exercise.setNReps(10);

        if (durationField != null)
        {
            if (!durationField.text.Equals("")) State.exercise.setDuration(int.Parse(durationField.text));
            else State.exercise.setDuration(60);
        }
        else State.exercise.setDuration(60);

        if (restTimeField != null)
        {
            if (!restTimeField.text.Equals("")) State.exercise.setRestTime(int.Parse(restTimeField.text));
            else State.exercise.setRestTime(60);
        }
        else State.exercise.setRestTime(60);

        if (SessionInfo.toView() == "SequenceCreat")
        {
            SequenceManager.sequence.addExercise(State.exercise);
            SequenceManager.sequence.toFile();
            SequenceManager.sequence = null;
            SceneManager.LoadScene("SequenceMenu");
        }
        else SceneManager.LoadScene(State.exercise.getScenePath());
    }

    public void nRepsUp()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")) nRepsField.text = (int.Parse(nRepsField.text) + 1).ToString();
            else nRepsField.text = "11";
        }
    }

    public void nRepsDown()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")) nRepsField.text = (int.Parse(nRepsField.text) - 1).ToString();
            else nRepsField.text = "9";
        }
    }

    public void durationUp()
    {
        if (durationField != null)
        {
            if (!durationField.text.Equals("")) durationField.text = (int.Parse(durationField.text) + 5).ToString();
            else durationField.text = "65";
        }
    }

    public void durationDown()
    {
        if (durationField != null)
        {
            if (!durationField.text.Equals("")) durationField.text = (int.Parse(durationField.text) - 5).ToString();
            else durationField.text = "55";
        }
    }

    public void restTimeUp()
    {
        if (restTimeField != null)
        {
            if (!restTimeField.text.Equals("")) restTimeField.text = (int.Parse(restTimeField.text) + 5).ToString();
            else restTimeField.text = "65";
        }
    }

    public void restTimeDown()
    {
        if (restTimeField != null)
        {
            if (!restTimeField.text.Equals("")) restTimeField.text = (int.Parse(restTimeField.text) - 5).ToString();
            else restTimeField.text = "55";
        }
    }

    public void previousScreen()
    {
        SceneManager.LoadScene("ArmSelection");
    }
}
