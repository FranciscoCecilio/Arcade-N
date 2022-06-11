using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public Text username;
    public Text age;
    public Text gender;

    private string _arm;
    private int _exerciseId;

    public Dropdown exerDropdown;
    public Dropdown sessionDropdown;

    string selectionSessionTimestamp;

    string timestamp;
    private List<string> sessionTimestamps = new List<string>();
    private List<string> exercisesTimestamps = new List<string>();
    private List<string> exercisesName = new List<string>();

    public GameObject _firstView;
    public GameObject _secondView;

    //Infos
    public Text sucess;
    private double _sucess;
    public Text tries;
    private string _tries = "0";
    public Text correctReps;
    private string _correctReps = "0";
    public Text avgTime;
    private string _outOfPath = "0";
    public Text outOfPath;
    private string _avgTime = "0";
    public Text totalTime;
    private string _totalTime = "0";
    public Text lShoulderComp;
    public Image lShoulderImage;
    private string _lShoulderComp = "0";
    public Text rShoulderComp;
    public Image rShoulderImage;
    private string _rShoulderComp = "0";
    public Text spineComp;
    public Image spineImage;
    private string _spineComp = "0";
    public Image person;
    public Text arm;

    private bool _viewObservations = false;
    public Text viewObservationsButtonText;
    private string _observations = "";
    public InputField observations;

    bool hasSessions = false;
    bool hasExercises = false;
    public GameObject results;
    public GameObject noResults;

    bool regShoulderComp = false;
    bool regSpineComp = false;

    [Header("Main Info")]
    public TMP_Text nr_sessions;
    public TMP_Text performance;
    public TMP_Text totalTherapyTime;

    [Header("List Content")]
    public Transform listContent;
    

    public void SessionDropdown_IndexChanged(int index)
    {
        selectionSessionTimestamp = sessionTimestamps[index];
        PopulateExercisesDropdown();
        if (hasExercises) PopulateInfo(0);
    }

    public void Dropdown_IndexChanged(int index)
    {
        PopulateInfo(index);
    }

    // we want to populate the 
    void Start()
    {
        PopulateMainInfo();
        PopulateSessionsList();

        PopulateUserInfo();
        PopulateSessionsDropdown();
        if (hasSessions) PopulateExercisesDropdown();
        if (hasExercises) PopulateInfo(0);
    }

    // Nr.Sessions ; Performance; total Time of therapy
    void PopulateMainInfo(){
        nr_sessions.text = "1";
        performance.text  = "100%";
        totalTherapyTime.text = "00:00:10";
    }

    // Fetch all sessions files and instantiate SessionResultsElement buttons on the listContent
    void PopulateSessionsList(){

    }

    void PopulateUserInfo()
    {
        username.text = SessionInfo.getName();
        age.text = SessionInfo.getAge();
        gender.text = SessionInfo.getGender();
    }

    void PopulateInfo(int index)
    {
        _observations = "";

        System.DateTime sessionTime = System.DateTime.ParseExact(selectionSessionTimestamp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        string sessionTimestamp = sessionTime.ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        timestamp = exercisesTimestamps[index];
        System.DateTime date = System.DateTime.ParseExact(timestamp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        timestamp = date.ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sessions/" + sessionTimestamp + "/" + timestamp + ".txt";
        if (File.Exists(filepath)) {
            string line = "";
            StreamReader reader = new StreamReader(filepath);
            {
                line = reader.ReadLine();
                while (line != null && line != "")
                {
                    string[] data = line.Split('=');
                    if (data[0] == "tries") _tries = data[1];
                    else if (data[0] == "arm") _arm = data[1];
                    else if (data[0] == "correctReps") _correctReps = data[1];
                    else if (data[0] == "outOfPath") _outOfPath = data[1];
                    else if (data[0] == "avgTime") _avgTime = data[1];
                    else if (data[0] == "totalTime") _totalTime = data[1];
                    else if (data[0] == "leftShoulderComp") _lShoulderComp = data[1];
                    else if (data[0] == "rightShoulderComp") _rShoulderComp = data[1];
                    else if (data[0] == "spineComp") _spineComp = data[1];
                    else if (data[0] == "obs") _observations = data[1];
                    else if (data[0] == "regSpineComp") regSpineComp = bool.Parse(data[1]);
                    else if (data[0] == "regShouldComp") regShoulderComp = bool.Parse(data[1]);
                    line = reader.ReadLine();
                }
            }
            reader.Close();
        }
        if (double.Parse(_tries) != 0) _sucess = System.Math.Round(double.Parse(_correctReps) / double.Parse(_tries) * 100);
        else _sucess = 0;
        sucess.text = (_sucess).ToString() + "%";
        if (_sucess < 50) sucess.color = new Color(0.72f, 0.06f, 0.04f);
        else if (_sucess < 90) sucess.color = new Color(1.0f, 0.76f, 0.04f);
        else sucess.color = new Color(0f, 1f, 0f);

        arm.text = "Arm: " + _arm;

        tries.text = "Total Reps: " + _tries;
        correctReps.text = "Correct Reps: " + _correctReps;
        outOfPath.text = "Out of Path: " + _outOfPath;
        avgTime.text = _avgTime;
        totalTime.text = _totalTime;

        lShoulderComp.text = _lShoulderComp;
        rShoulderComp.text = _rShoulderComp;
        spineComp.text = _spineComp;

        observations.text = _observations;

        if (regShoulderComp)
        {
            if (int.Parse(_lShoulderComp) > 10)
            {
                lShoulderImage.GetComponent<Image>().color = new Color32(226, 49, 46, 255);
            }
            else if (int.Parse(_lShoulderComp) <= 10 && int.Parse(_lShoulderComp) > 5)
            {
                lShoulderImage.GetComponent<Image>().color = new Color32(236, 169, 0, 255);
            }
            else
            {
                lShoulderImage.GetComponent<Image>().color = new Color32(78, 171, 0, 255);
            }

            if (int.Parse(_rShoulderComp) > 10)
            {
                rShoulderImage.GetComponent<Image>().color = new Color32(226, 49, 46, 255);
            }
            else if (int.Parse(_rShoulderComp) <= 10 && int.Parse(_rShoulderComp) > 5)
            {
                rShoulderImage.GetComponent<Image>().color = new Color32(236, 169, 0, 255);
            }
            else
            {
                rShoulderImage.GetComponent<Image>().color = new Color32(78, 171, 0, 255);
            }
        }
        else
        {
            rShoulderImage.GetComponent<Image>().color = new Color32(128, 128, 128, 128);
            lShoulderImage.GetComponent<Image>().color = new Color32(128, 128, 128, 128);
        }
        
        if (regSpineComp)
        {
            if (int.Parse(_spineComp) > 10)
            {
                spineImage.GetComponent<Image>().color = new Color32(226, 49, 46, 255);
            }
            else if (int.Parse(_spineComp) <= 10 && int.Parse(_spineComp) > 5)
            {
                spineImage.GetComponent<Image>().color = new Color32(236, 169, 0, 255);
            }
            else
            {
                spineImage.GetComponent<Image>().color = new Color32(78, 171, 0, 255);
            }
        } else
        {
            spineImage.GetComponent<Image>().color = new Color32(128, 128, 128, 128);
        }

        if (!regSpineComp && !regShoulderComp)
        {
            var tempColor = person.GetComponent<Image>().color;
            tempColor.a = 0.6f;
            person.GetComponent<Image>().color = tempColor;
        } else
        {
            var tempColor = person.GetComponent<Image>().color;
            tempColor.a = 1f;
            person.GetComponent<Image>().color = tempColor;
        }

    }

    void PopulateSessionsDropdown()
    {
        string folderpath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sessions/";
        if (Directory.Exists(folderpath))
        {
            string[] folders = Directory.GetDirectories(folderpath);
            for (int i = folders.Length - 1; i > -1; i--) 
            {
                string[] files = Directory.GetFiles(folders[i]);
                if (files.Length > 0)
                {
                    hasSessions = true;
                    string[] filename = folders[i].Split('/');
                    string timestamp = filename[filename.Length - 1];
                    System.DateTime date = System.DateTime.ParseExact(timestamp, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    timestamp = date.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    sessionTimestamps.Add(timestamp);
                }
            }
            if (sessionTimestamps.Count != 0) selectionSessionTimestamp = sessionTimestamps[0];
            sessionDropdown.AddOptions(sessionTimestamps);
        }
    }

    void PopulateExercisesDropdown()
    {
        hasExercises = false;
        exercisesName.Clear();
        exercisesTimestamps.Clear();
        exerDropdown.options.Clear();

        System.DateTime pathTime = System.DateTime.ParseExact(selectionSessionTimestamp, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        string pathTimestamp = pathTime.ToString("yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);

        string folderpath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sessions/" + pathTimestamp + "/";

        if (Directory.Exists(folderpath))
        {
            foreach (string file in Directory.GetFiles(folderpath))
            {
                string[] filename = file.Split('.');
                if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
                {
                    hasExercises = true;
                    filename = filename[0].Split('/');
                    string timestamp = filename[filename.Length - 1];
                    string _name = "";
                    string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sessions/" + pathTimestamp + "/" + timestamp + ".txt";
                    if (File.Exists(filepath))
                    {
                        string line = "";
                        StreamReader reader = new StreamReader(filepath);
                        {
                            line = reader.ReadLine();
                            while (line != null && line != "")
                            {
                                string[] data = line.Split('=');
                                if (data[0] == "name")
                                {
                                    _name = data[1];
                                    break;
                                }
                                line = reader.ReadLine();
                            }
                        }
                        reader.Close();
                    }

                    System.DateTime date = System.DateTime.ParseExact(timestamp, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    timestamp = date.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    string timestamp_hour = date.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    exercisesName.Add(_name + " - " + timestamp_hour);
                    exercisesTimestamps.Add(timestamp);
                }
            }
            exerDropdown.AddOptions(exercisesName);
        }
        results.SetActive(hasExercises);
        noResults.SetActive(!hasExercises);
    }

    public void changeViews()
    {
        _viewObservations = !_viewObservations;
        _firstView.SetActive(!_viewObservations);
        _secondView.SetActive(_viewObservations);
        if (!_viewObservations) viewObservationsButtonText.text = "View Observations";
        else viewObservationsButtonText.text = "Hide Observations";
    }

    public void saveObservations()
    {
        //TODO - This doesnt overwrite

        StreamWriter writer = File.AppendText(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Exercises/" + _exerciseId + "/" + _arm + "/" + timestamp + ".txt");

        writer.WriteLine("obs=" + observations.text);

        writer.Close();
    }

    public void previousScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
