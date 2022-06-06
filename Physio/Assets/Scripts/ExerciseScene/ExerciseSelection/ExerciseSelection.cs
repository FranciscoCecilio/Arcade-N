using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NOTE: This script is not used anymore and probably won't work due to directory and file logic was changed
public class ExerciseSelection : MonoBehaviour {

    private GameObject[] _buttons;
    public GameObject finishButton;

    public Text succEx1;
    public Text timestampEx1;

    public Text succEx2;
    public Text timestampEx2;

    public Text succEx3;
    public Text timestampEx3;

    void Start()
    {
        /*if (SessionInfo.toView() == "SequenceCreat")
        {
            _buttons = GameObject.FindGameObjectsWithTag("ExerciseSelectionButton");
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].GetComponentInChildren<Text>().text = "Select";
            }
        }*/
        loadLastExercisesInfo(succEx1, timestampEx1, Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/Exercise1Scene.txt");
        loadLastExercisesInfo(succEx2, timestampEx2, Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/Exercise2Scene.txt");
        loadLastExercisesInfo(succEx3, timestampEx3, Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/Exercise3Scene.txt");
    }

    public void selectExercise(int id)
    {
        switch (id)
        {
            case 1:
                State.exercise = new global::Exercise(1, "Up/Down", "Exercise1Scene");
                break;

            case 2:
                State.exercise = new global::Exercise(2, "Left/Right", "Exercise2Scene");
                break;
            case 3:
                State.exercise = new global::Exercise(3, "Grid", "Exercise3Scene");
                break;
        }
        SceneManager.LoadScene("ArmSelection");
    }

    public void finishSequence()
    {
        /*SequenceManager.sequence.toFile();
        SequenceManager.sequence = null;*/

        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("SequenceMenu");
    }

    public void previousScreen()
    {
        // store this as the previous scene
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        /* we could use the _lastSceneIndex here */
        if (SessionInfo.toView() == "SequenceCreat") SceneManager.LoadScene("SequenceMenu");
        else SceneManager.LoadScene("MainMenu");
    }

    private void loadLastExercisesInfo(Text succ, Text timestamp, string filepath)
    {
        string timestampStr = "";
        int correctReps = 0;
        int tries = 0;

        if (System.IO.File.Exists(filepath))
        {
            string line = "";
            StreamReader reader = new StreamReader(filepath);
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split('=');
                    if (data[0] == "correctReps") correctReps = Int32.Parse(data[1]);
                    else if (data[0] == "tries") tries = Int32.Parse(data[1]);
                    else if (data[0] == "timestamp") timestampStr = data[1];
                    line = reader.ReadLine();
                }
            }
            succ.text = "" + ((correctReps * 100) / tries) + "%";
            System.DateTime date = System.DateTime.ParseExact(timestampStr, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            timestamp.text = date.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        } else
        {
            succ.text = "";
            timestamp.text = "";
        }
    }
}
