using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class SessionResult : MonoBehaviour
{
    public TMP_Text listIndexText;
    public TMP_Text dateText;
    public TMP_Text durationText;
    public TMP_Text performanceText;

    // session folder path
    public string sessionFolderPath;   // Application.dataPath + "/Users/" + SessionInfo.getUsername() + <SessionTS>
    public GameObject exerciseResultPrefab;
    // this is where we instantiate ExerciseResult prefabs
    Transform specificSessionListContent;
    
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

    // TODO receive parameters
    public void PopulateSessionListElement(string fPath, Transform listContent){
        sessionFolderPath = fPath;
        specificSessionListContent = listContent;
        listIndexText.text = "5";
        dateText.text = "20 Maio / 12h45";
        durationText.text = "01:25:13";
        performanceText.text = "85%";
    }

    public void OpenSessionPanel(){
        // play animation
        if(tweenManager == null){
            tweenManager = GameObject.FindGameObjectWithTag("TweenManager").GetComponent<Results_TweenManager>();
        }
        if(isInList){
            tweenManager.OpenSpecificSession();
        }
        else{
            tweenManager.ReturnToAllSessions();
        }

        // set the correct session number title
        // we only fetch the exercise results one time
        if(!hasLoaded){
            FetchExerciseResults();
            hasLoaded = true;
        }
        // set the correct session paremeters on the top of the panel
        // set all the sequences on Specific Session Panel ContentList

    }

    // goes inside the Session folder
    void FetchExerciseResults(){
        // sessionFolderPath: Application.dataPath + "/Users/" + SessionInfo.getUsername() "/" <SessionTS>
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
                            if (data[0] == "name") newExerciseResultdictionary.Add(data[0], data[1]);
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
                    Debug.Log(newExerciseResultdictionary.Count);
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




    // called when a button is instantiated after clicking a session in list
    public void SetIsInList(bool intention){
        isInList = intention;
    }


    
}
