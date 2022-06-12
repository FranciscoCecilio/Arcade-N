using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

// Keeps the results from all the Exercises from a specific Sequence (each ExerciseResults button has one of these attached)
public class ExerciseResult : MonoBehaviour
{
    [Header("Main parameters")]
    public TMP_Text _TotalDurationText;
    public ExerciseImage _ExImg;

    string _TotalDuration;
    string _ExType;

    [Header("Planned paremeters")]
    public TMP_Text _NSeriesText;
    public TMP_Text _NRepsText;
    public TMP_Text _ArmText;
    public TMP_Text _DurationText;
    public TMP_Text _RestimeText;

    string _NSeries;
    string _NReps;
    string _Arm;
    string _Duration;
    string _Restime;
    
    [Header("Results")]
    public TMP_Text _correctRepsText;
    public TMP_Text _outOfPathsText;
    public TMP_Text _totalTimeText;
    public TMP_Text _averageTimeText;

    List<string> _correctReps = new List<string>();
    List<string> _outOfPaths = new List<string>();
    List<string> _totalTime = new List<string>();
    List<string> _averageTime = new List<string>();

    [Header("Mistakes")]
    public TMP_Text _regShouldCompText;
    public TMP_Text _regSpineCompText;
    public TMP_Text _leftShoulderCompText;
    public TMP_Text _rightShoulderCompText;
    public TMP_Text _spineCompText;

    List<string> _regShouldComps = new List<string>();
    List<string> _regSpineComps = new List<string>();
    List<string> _leftShoulderComps = new List<string>();
    List<string> _rightShoulderComps = new List<string>();
    List<string> _spineComps = new List<string>();

    // Indexes
    int _selectedSerieIndex = 0;
    int _listIndex;
    
    //size of exercises saved in this button - useless atm
    int exercisesCount = 0;

    // sequence folder path
    public string folderPath;

    // Start is called before the first frame update
    void Start()
    {
        //PopulateExerciseListElement();
    }

    // populate the button with the correct values
    // called after we change _selectedSerieIndex or when we come from all_sessions_screen
    // TODO personalizar as strings
    public void  PopulateExerciseListElement(){
        // Main parameters
        Debug.Log(_Arm);
        Debug.Log(_correctReps[0]);
        _NSeriesText.text = _NSeries;
        _NRepsText.text = _NReps;
        _ArmText.text = _Arm;
        _DurationText.text = _Duration;
        _RestimeText.text = _Restime;
        // Results
        _correctRepsText.text = _correctReps[_selectedSerieIndex];
        _outOfPathsText.text =  _outOfPaths[_selectedSerieIndex];
        _totalTimeText.text = _totalTime[_selectedSerieIndex];
        _averageTimeText.text = _averageTime[_selectedSerieIndex];
        // mistakes
        /*_regShouldCompText.text = _regShouldComps[_selectedSerieIndex];
        _regSpineCompText.text = _leftShoulderComps[_selectedSerieIndex];
        _leftShoulderCompText.text = _leftShoulderComps[_selectedSerieIndex];
        _rightShoulderCompText.text = _rightShoulderComps[_selectedSerieIndex];
        _spineCompText.text = _spineComps[_selectedSerieIndex];*/
    }

    // this receives the parameters and results of one of the exercises from the sequence 
    public void AddExerciseInfo(Dictionary<string, string> newExercise){
        // planning variables (always the same)
        _Arm = newExercise["arm"];
        _NReps = newExercise["nrReps"];
        _Duration = newExercise["duration"];
        _Restime = newExercise["restTime"];
        // results variables 
        _correctReps.Add(newExercise["correctReps"]);
        _outOfPaths.Add(newExercise["outOfPath"]);
        _correctReps.Add(newExercise["correctReps"]);
        _averageTime.Add(newExercise["avgTime"]);
        _totalTime.Add(newExercise["totalTime"]);
        // mistakes variables
        _regShouldComps.Add(newExercise["regShouldComp"]);
        _leftShoulderComps.Add(newExercise["leftShoulderComp"]);
        _leftShoulderComps.Add(newExercise["leftShoulderComp"]);
        _rightShoulderComps.Add(newExercise["rightShoulderComp"]);
        _spineComps.Add(newExercise["spineComp"]);
        
        // lastly increase the control variable
        exercisesCount ++;
    }
}
