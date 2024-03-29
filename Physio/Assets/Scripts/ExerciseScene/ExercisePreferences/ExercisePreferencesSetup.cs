using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.UI;


// At the start of each exercise 1 and 2, we want to set it according to the last run. (path and target positions; path width; grid order;)
// This script is also responsible to save those preferences in each User/ExercisePreferences folder
public class ExercisePreferencesSetup : MonoBehaviour
{
    public static bool preferencesChanged = false; // static variable changed by  DragDrop and DragDropX whenever the user drags the Path or Targets TODO pathSize

    public Camera worldCamera; // usually therapist Camera

    public Slider pathSizeSlider; // to update with the correct value on start

    GridPaternManager gridManager; // the script needs to be on the same obejct as this script

    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        if ( scene == "Exercise1Scene" || scene == "Exercise2Scene")
        {
            PositionObjects();
        }
        else if(scene == "Exercise0Scene"){
            gridManager = GetComponent<GridPaternManager>();
            SetGridPatern();
        }
    }

    // Fetches and applies the last used grid patern
    private void SetGridPatern()
    {
        string scene = SceneManager.GetActiveScene().name;
        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/ExercisePreferences/" + scene + ".txt";
        
        // If the file exists:
        // Fetches the grid patern from the last exercise and applies
        if (System.IO.File.Exists(filepath))
        {
            string line = "";
            StreamReader reader = new StreamReader(filepath);
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split('=');
                    if (data[0] == "gridPatern")
                    {
                       gridManager.SetChosenPatern(data[1]);
                    }
                    line = reader.ReadLine();
                }
            }
            reader.Close();
        }
        else{
            // by default if the user didnt chose any patern, we use spiralPatern
            gridManager.SetChosenPatern("spiralPatern");
        }
    }

    // Fetches and applies the positions of the path and targets and sets the pathSize from the last exercise
    private void PositionObjects()
    {
        string scene = SceneManager.GetActiveScene().name;
        // Defines the variable _arm
        string _arm = "";
        if (State.exercise.isLeftArm()){
            _arm = "left";
        } 
        else{
            _arm = "right";
        }

        // Defines the filepath to the text file in "Last" folder corresponding to the exercise
        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/ExercisePreferences/" + scene + ".txt";
        
        // If the file exists:
        // Fetches the correct [Path and Target position] from the last exercise and applies
        if (System.IO.File.Exists(filepath))
        {
            string line = "";
            StreamReader reader = new StreamReader(filepath);
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split('=');
                    if (data[0] == "arm")
                    {
                        if (!data[1].Equals(_arm)) break;
                    }
                    if (data[0] == "pathSize")
                    {
                        Vector3 pathSize = StringToVector3(data[1]);
                        GameObject path = GameObject.FindGameObjectWithTag("ExerciseCollider");
                        path.transform.localScale = pathSize;
                        pathSizeSlider.value = pathSize.x;
                    }
                    else if (data[0] == "pathPosition")
                    {
                        Vector3 pathPosition = StringToVector3(data[1]);
                        GameObject path = GameObject.FindGameObjectWithTag("ExerciseCollider");
                        path.transform.position = worldCamera.ScreenToWorldPoint(pathPosition);
                    }
                    else if (data[0] == "target0")
                    {
                        Vector3 target1Position = StringToVector3(data[1]);
                        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
                        //targets[0].transform.position = worldCamera.ScreenToWorldPoint(target1Position);
                        targets[0].transform.localPosition = target1Position;
                    }
                    else if (data[0] == "target1")
                    {
                        Vector3 target2Position = StringToVector3(data[1]);
                        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
                        //targets[1].transform.position = worldCamera.ScreenToWorldPoint(target2Position);
                        targets[1].transform.localPosition = target2Position;
                    }
                    line = reader.ReadLine();
                }
            }
        }
    }

    

    // aux function
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }
        sVector = sVector.Replace(" ", string.Empty);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0].Replace(".", ",")),
            float.Parse(sArray[1].Replace(".", ",")),
            float.Parse(sArray[2].Replace(".", ",")));

        return result;
    }

    // Exercise finished - Save everything for vertical and horizontal exercises
    public void SaveEverything(){
        savePathPosition();
        saveTargetPositions();
        SavePreferencesToFile();
    }
    
    // When we EDIT the exercise preferences, we want to write it in the User/ExercisePreferences/<exercise>.txt 
    // HORZIONTAL and VERTICAL exercises: This is called on ExerciseManager and needs the State.exercise to be set
    // GRID exercise: This is called on ExerciseManager3 OR GridPaternManager when we click a grid button
    public void SavePreferencesToFile(){
        Debug.Log("Preferences! grid manager:" + gridManager);
        // Create the ExercisePreferences File
        string scene = SceneManager.GetActiveScene().name;

        // It means that, despite the exercise started and was runed, the user never edited the path or target
        if((scene == "Exercise1Scene" || scene == "Exercise2Scene") && preferencesChanged == false){
            return;
        }
        // Otherwise we made changes and will now save them
        preferencesChanged = false;
        
        // Create ExercisePreferences folder
        String folderPath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/ExercisePreferences/";
        if(!Directory.Exists(folderPath)){
            Directory.CreateDirectory(folderPath);
            Debug.Log("Created folder ExercisePreferences.");
        }

        // Create the ExercisePreferences File
        String filename = scene;
        String filepath = folderPath + filename + ".txt";
        Debug.Log("We will save preferences here:"  + filepath);

        // If exists delete it
        if (System.IO.File.Exists(filepath)) 
            System.IO.File.Delete(filepath);

        // Defines the variable _arm
        string _arm = "";
        if (State.exercise.isLeftArm()){
            _arm = "left";
        } 
        else{
            _arm = "right";
        }
        // Write stuff to the file
        // For Horizontal and Vertical Exercise - target and path position
        if ( scene == "Exercise1Scene" || scene == "Exercise2Scene")
        {
            Vector3[] _targetPositions = State.exercise.GetTargetPositions();
            Vector3 _pathPosition =  State.exercise.GetPathPosition();
            Vector3 _pathSize =  State.exercise.GetPathSize();

            using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("arm=" + _arm );
                if (_pathPosition != null)
                {
                    writer.WriteLine("pathPosition=" + _pathPosition.ToString());
                }
                if (_pathPosition != null)
                {
                    writer.WriteLine("pathSize=" + _pathSize.ToString());
                }
                if ( _targetPositions != null)
                {
                    for (int i = 0; i < _targetPositions.Length; i++)
                    {
                        writer.WriteLine("target" + i + "=" + _targetPositions[i].ToString());
                    }
                }

                writer.Close();
                stream.Close();
            }
        }
        // GRID scene - save the gridPatern
        else if(scene == "Exercise0Scene"){
            using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("arm=" + _arm );
                if (!gridManager.chosenPaternName.Equals(String.Empty))
                {
                    writer.WriteLine("gridPatern=" + gridManager.chosenPaternName);
                }
                else{
                    Debug.LogWarning("Saving an empty gridPatern to exercise preferences File.");
                }

                writer.Close();
                stream.Close();
            }   
            Debug.Log("Saved preferences to file.");
        }

    }

    // Save stuff
    public void saveTargetPositions()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetCollider");
        Vector3[] targetPositions = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targetPositions[i] = targets[i].transform.localPosition;
            //targetPositions[i] = worldCamera.WorldToScreenPoint(targets[i].transform.position);
        }
        State.exercise.saveTargetPositions(targetPositions);
    }

    public void savePathPosition()
    {
        GameObject path = GameObject.FindGameObjectWithTag("ExerciseCollider"); //Right Exercise Box
        State.exercise.savePathPosition(worldCamera.WorldToScreenPoint(path.transform.position));
        State.exercise.savePathSize(path.transform.localScale);
    }
}
