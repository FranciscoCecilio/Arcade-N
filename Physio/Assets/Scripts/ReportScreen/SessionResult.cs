using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class SessionResult : MonoBehaviour
{
    public TMP_Text sessionNumber;
    public TMP_Text dateText;
    // performance
    public TMP_Text performanceText;
    List<double> performances = new List<double>();
    // duration
    public TMP_Text totalDurationText;
    List<string> timespans = new List<string>();
    public string totalTimeSpan; // HH:mm:ss 

    // session folder path
    public string sessionFolderPath;   // Application.dataPath + "/Users/" + SessionInfo.getUsername() + <SessionTS>
    public GameObject exerciseResultPrefab;

    // parent of ExerciseResult prefabs
    Transform specificSessionListContent;
    List<GameObject> exerciseResults = new List<GameObject>();
    
    // Indexes
    int _listIndex;

    // Tween Manager
    Results_TweenManager tweenManager;

    // Control variables
    [SerializeField] bool isInList = true; // is 
    [SerializeField] bool hasLoaded = false; // do we have all ExerciseResults already loaded



    // Start is called before the first frame update
    void Start()
    {
        //PopulateExerciseListElement();
    }

    public void PopulateSessionListElement(int sessionNum, string sessionDate, string fPath, Transform listContent){
        sessionNumber.text = sessionNum.ToString();
        sessionFolderPath = fPath;
        specificSessionListContent = listContent;
        dateText.text = sessionDate;
        // TODO performance
        performanceText.text = "85%";
        FetchSequenceInfo();
        CalculateSessionDuration();
        CalculateSessionPerformance();

    }

    // we are interested in this to calculate the total therapy time in Results Screen
    public string GetTotalTimeSpan(){
        return totalTimeSpan;
    }

    public void OpenSessionPanel(){
        // we only fetch the exercise results one time
        if(!hasLoaded){
            FetchExerciseResults();
            hasLoaded = true;
        }

        // play animation
        if(tweenManager == null){
            tweenManager = GameObject.FindGameObjectWithTag("TweenManager").GetComponent<Results_TweenManager>();
        }
        if(isInList){
            tweenManager.SetSpecificSessionParameters(this.GetComponent<SessionResult>());
            tweenManager.OpenSpecificSession(gameObject);
            // make all the corresponding exercise_results buttons appear
            for(int i = 0; i < exerciseResults.Count; i++){
                exerciseResults[i].SetActive(true);
            }
        }
        else{
            // next method ALREADY hides exercise_results buttons in list 
            tweenManager.ReturnToAllSessions(gameObject);
        }

        // set the correct session number title
        // set the correct session paremeters on the top of the panel
        // set all the sequences on Specific Session Panel ContentList

    }
    
    // Generates a exercise result button for each sequence inside this session
    void FetchExerciseResults(){
        if (Directory.Exists(sessionFolderPath))
        {
            // sequenceFolders[i]: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS> "/" <Sequencefolder i>
            string[] sequenceFolders = Directory.GetDirectories(sessionFolderPath);

            // For each Sequence folder we fetch its Exercises and instantiate one ExerciseResult Button
            for (int i = sequenceFolders.Length - 1; i > -1; i--) 
            {
                Debug.Log("Is inside the Sequencefolder");
                // Instantiate Exercise Result Button
                string exerciseFolderName = sequenceFolders[i].Substring(sessionFolderPath.Length+1);
                ExerciseResult newButton = GenerateExerciseResultButton(exerciseFolderName);

                // save the buttons for when we comeback to this screen
                exerciseResults.Add(newButton.gameObject);

                // Exercise Files: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS> "/" <Sequencefolder i> "/" exercises here
                string specificSequence = sequenceFolders[i];
                string[] exfiles = Directory.GetFiles(specificSequence);
               
                // For each Exercise file we fetch and store its parameters
                foreach (string file in exfiles)
                {
                    //Verifica que e um ficheiro txt e nao meta
                    string[] filename = file.Split('.');
                    if (filename.Length != 2 || filename[1] != "txt" ) 
                    {
                        continue;
                    }
                    // Verifies if its the Sequence file (we want only exercise)
                    string[] filename1 = file.Split('\\');
                    //Debug.Log("HEY: " + filename1[filename1.Length-1]);
                    //Debug.Log("HEY2: " + filename1[filename1.Length-1].Substring(0, "Sequence".Length));
                    if(filename1[filename1.Length-1].Substring(0, "Sequence".Length).Equals("Sequence"))
                    {
                        Debug.Log("It's a sequence, continue: " + filename);
                        continue;
                    }
                    //Debug.Log("is inside exercise:" + file);
                    // dictionary <key, value>
                    Dictionary<string, string> newExerciseResultdictionary = new Dictionary<string, string>();
                    int listIndex = 0;
                    string line = "";
                    StreamReader reader = new StreamReader(file);
                    {
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            string[] data = line.Split('=');
                            /*timestamp=20220612T124004
                            name=Vertical
                            arm=right
                            nrReps=1
                            correctReps=1
                            tries=1
                            outOfPath=0
                            duration=60
                            restTime=60
                            avgTime=00:06 m
                            totalTime=00:06 m
                            regShouldComp=False
                            regSpineComp=False
                            leftShoulderComp=0
                            rightShoulderComp=0
                            spineComp=0*/
                            if (data[0] == "arm") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "name") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "nrReps") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "duration") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "correctReps") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "outOfPath") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "restTime") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "avgTime") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "totalTime") newExerciseResultdictionary.Add(data[0], data[1]);
                            //else if (data[0] == "regShouldComp") newExerciseResultdictionary.Add(data[0], data[1]);\
                            //else if (data[0] == "regSpineComp") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "leftShoulderComp") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "rightShoulderComp") newExerciseResultdictionary.Add(data[0], data[1]);
                            else if (data[0] == "spineComp") newExerciseResultdictionary.Add(data[0], data[1]);

                            line = reader.ReadLine();
                        }
                    }
                    reader.Close();
                    // increement the variable that has the order in the exercise_result list
                    newExerciseResultdictionary.Add("lastIndex", listIndex.ToString());
                    listIndex ++;
                    // Add the exercise serie
                    newButton.AddExerciseInfo(newExerciseResultdictionary);

                }
                newButton.PopulateExerciseListElement();
            }
        }
        else{
            Debug.Log("Error: SessionResult is trying to fetch exercises from: " + sessionFolderPath);
        }
    }

    // Generates a button (we set the parameters after)
    public ExerciseResult GenerateExerciseResultButton(string exerciseTS){
        //Instantiate prefab
        GameObject button = Instantiate(exerciseResultPrefab) as GameObject;
        button.name = exerciseTS;
        button.SetActive(true);
        // Place it in the list content
        button.transform.SetParent(specificSessionListContent, false);
        // return the button so we can set its parameters
        return button.GetComponent<ExerciseResult>();
    }


    // goes inside the sequence files and fetches Total duration and performance
    void FetchSequenceInfo(){
        // sessionFolderPath: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS>
        if (Directory.Exists(sessionFolderPath))
        {
            // sequenceFolders[i]: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS> "/" <Sequencefolder i>
            string[] sequenceFolders = Directory.GetDirectories(sessionFolderPath);

            // For each Sequence folder we fetch its Exercises and instantiate one ExerciseResult Button
            for (int i = sequenceFolders.Length - 1; i > -1; i--) 
            {
                // Exercise Files: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS> "/" <Sequencefolder i> "/" exercises here
                string specificSequence = sequenceFolders[i];
                string[] exfiles = Directory.GetFiles(specificSequence);
               
                // For each Exercise file we fetch and store its parameters
                foreach (string file in exfiles)
                {
                    //Verifica que e um ficheiro txt e nao meta
                    string[] filename = file.Split('.');
                    if (filename.Length != 2 || filename[1] != "txt" ) 
                    {
                        continue;
                    }
                    // Verifies if its the Sequence file
                    string[] filename1 = file.Split('\\');
                    if(!filename1[filename1.Length-1].Substring(0, "Sequence".Length).Equals("Sequence"))
                    {
                        //"It's an exercise, continue"
                        continue;
                    }
                    string line = "";
                    StreamReader reader = new StreamReader(file);
                    {
                        line = reader.ReadLine();
                        while (line != null)
                        {
                            string[] data = line.Split('=');
                            if (data[0] == "totalDuration"){
                                timespans.Add(data[1]); // Get what we came for and store: Duration 00:15:12
                            }
                            else if(data[0] == "performance"){
                                performances.Add(double.Parse(data[1], System.Globalization.CultureInfo.InvariantCulture)); // Get what we came for and store: 00:15:12
                            }
                            line = reader.ReadLine();
                        }
                    }
                    reader.Close();
                }
            }
        }
        else{
            Debug.Log("ERROR: SessionResult is trying to fetch sequence files from: " + sessionFolderPath);
        }
    }
    
    void CalculateSessionDuration(){
        string expression;
        TimeSpan totalTime = TimeSpan.Zero;
        // Format: HH:MM:SS horas
        for(int i = 0; i < timespans.Count; i++){
            expression = timespans[i].Split(' ')[0]; //HH:MM:SS 
            TimeSpan ts = TimeSpan.ParseExact(expression, "hh\\:mm\\:ss", System.Globalization.CultureInfo.InvariantCulture);
            totalTime += ts;
        }
        
        totalTimeSpan = string.Format("{0:D2}:{1:D2}:{2:D2}", totalTime.Hours, totalTime.Minutes, totalTime.Seconds);
        totalDurationText.text = totalTimeSpan;
    }

    void CalculateSessionPerformance(){
        // calculate average performance
        double average = performances.Count > 0 ? performances.Average() : 0.0;        
        performanceText.text = (average * 100).ToString() + " %";
    }



    // called when a button is instantiated after clicking a session in list
    public void SetIsInList(bool intention){
        isInList = intention;
    }

    // after we pick one session we must pass them to the specific_session
    public void SetSelectesSessionValues(SessionResult selectedResult){
        if(isInList) Debug.Log("ERROR: this is supposed to be called on the selected session on the specific_panel");
        sessionNumber.text = selectedResult.GetSessionNumber();
        dateText.text = selectedResult.GetDate();
        totalDurationText.text = selectedResult.GetDuration();
        performanceText.text = selectedResult.GetPerformance();
    }

    public string GetSessionNumber(){
        return sessionNumber.text;
    }
    public string GetDate(){
        return dateText.text;
    }
    public string GetDuration(){
        return totalDurationText.text;
    }
    public string GetPerformance(){
        return performanceText.text;
    }

    



    
}
