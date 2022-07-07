using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

// used to instantiate all session buttons
public class ResultsScreen : MonoBehaviour
{
    /*public Text username;
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
    bool regSpineComp = false;*/

    [Header("Main Info")]
    public TMP_Text nr_sessions;
    public TMP_Text performance;
    public TMP_Text totalTherapyTime;
    List<string> sessionTimespans = new List<string>();

    

    [Header("List Contents")]
    public Transform allSessionListContent;
    public GameObject sessionResultPrefab;
    public Transform specificSessionListContent;

    public TMP_Text errorLog;


    // we want to populate the 
    void Start()
    {
        PopulateMainInfo();
        PopulateSessionsList();
        // Calculate the Total Therapy TIme

        /*PopulateUserInfo();
        PopulateSessionsDropdown();
        if (hasSessions) PopulateExercisesDropdown();
        if (hasExercises) PopulateInfo(0);*/
    }

    // Nr.Sessions ; Performance; total Time of therapy
    void PopulateMainInfo(){
        performance.text  = "100%";
        // Total therapy is calculated after populateSessionsList
    }

    // Fetch all sequence files and instantiate SessionResultsElement buttons on the listContent
    void PopulateSessionsList(){
        string folderpath = Application.dataPath + "/Users/" + SessionInfo.getUsername();
        int sessionNumber = 0;
        if (Directory.Exists(folderpath))
        {
            errorLog.gameObject.SetActive(false);
            string[] folders = Directory.GetDirectories(folderpath);
            // For each Session folder we instantiate a SessionResult Button
            int foldersLength = folders.Length; // we will delete stuff so its important to keep the initial size
            errorLog.text += "\n foldersLenght:" + foldersLength;
            for (int i = foldersLength - 1; i > -1; i--) 
            {
                errorLog.text += "\n i:" + i + " + folder[i] " + folders[i] ;
                
                // check if its an empty session folder -> delete && continue
                /* EDITOR ONLY - this doesn't work on a BUILD
                if(Directory.GetFiles(folders[i]).Length == 0){
                    Directory.Delete(folders[i]);
                    if(File.Exists(folders[i]+".meta")) 
                        File.Delete(folders[i]+".meta");
                    continue;
                }
                 EDITOR ONLY
                 */

                // We found the exercise folder -> continue
                string folderName = folders[i].Substring(folderpath.Length+1);
                errorLog.text += "\n folderName" + folderName;
                if(folderName.Equals("ExercisePreferences")) continue;

                // inc the session_number
                sessionNumber ++;

                // get the timestamp to build the date
                string ts = folderName.Replace("Session", string.Empty);
                string sessionDate = CalculateSessionDate(ts);

                // generate button
                SessionResult result = GenerateSessionResultButton(sessionNumber,sessionDate,folderName,folders[i]);
                sessionTimespans.Add(result.GetTotalTimeSpan());
            }
            // clean any deleted files
            //RefreshEditorProjectWindow();
            // at the end, after creating one button for each session, we calculate total therapy time
            CalculateTherapyTime();
        }

        // SessionsNumber Text
        nr_sessions.text = sessionNumber.ToString();
    }

    // returns a string with the date of the session
    string CalculateSessionDate(string ts){
        DateTime timestamp;
        DateTime.TryParseExact(ts, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out timestamp);
        //18 Maio 2022 / 13h45
        string sessionDate = timestamp.Day + " " +
            System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(timestamp.Month) + " " +
            timestamp.Year + " / " + 
            timestamp.Hour + "h" + timestamp.Minute;
        
        return sessionDate;
    }

    // calculates the main info parameter: total therapy time
    void CalculateTherapyTime(){
        string expression;
        TimeSpan totalTime = TimeSpan.Zero;
        // Format: HH:MM:SS horas
        for(int i = 0; i < sessionTimespans.Count; i++){
            expression = sessionTimespans[i].Split(' ')[0]; //HH:MM:SS 
            TimeSpan ts = TimeSpan.ParseExact(expression, "hh\\:mm\\:ss", System.Globalization.CultureInfo.InvariantCulture);
            totalTime += ts;
        }
        // assign the variable (finally)        
        totalTherapyTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
    }

    // Generates a session button
    public SessionResult GenerateSessionResultButton(/*will receive session result info*/ int sessionNumber, string sessionDate, string sessionTS, string folderPath){
        errorLog.text += "\n generates btn " + sessionTS;
        
        //Instantiate prefab
        GameObject button = Instantiate(sessionResultPrefab) as GameObject;
        button.name = sessionTS;
        button.SetActive(true);
        // Sets the parameters  
        button.GetComponent<SessionResult>().PopulateSessionListElement(sessionNumber, sessionDate, folderPath, specificSessionListContent);
        button.transform.SetParent(allSessionListContent, false);

        return button.GetComponent<SessionResult>();
    }

    void PopulateUserInfo()
    {
        /*username.text = SessionInfo.getName();
        age.text = SessionInfo.getAge();
        gender.text = SessionInfo.getGender();*/
    }
    
    public void previousScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private static void  RefreshEditorProjectWindow() 
    {
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }

}
