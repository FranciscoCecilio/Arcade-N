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
    public TMP_Text _ListIndexText;
    public TMP_Text _TotalDurationText;
    public ExerciseImage _ExImg;

    string _ListIndex;
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

    [Header("Compensatory")]
    public TMP_Text _leftShoulderCompText;
    public TMP_Text _rightShoulderCompText;
    public TMP_Text _spineCompText;

    public Image LsholderImage;
    public Image RsholderImage;
    public Image SpineImage;

    List<string> _leftShoulderComps = new List<string>();
    List<string> _rightShoulderComps = new List<string>();
    List<string> _spineComps = new List<string>();

    [Header("Series button")]
    public GameObject prevSerieButton; 
    public GameObject nextSerieButton; 
    public TMP_Text numberText;

    // Indexes
    int _selectedSerieIndex = 0;
    
    // Variables
    int exercisesCount = 0; 
    bool initialPopulate = false;

    // sequence folder path
    public string folderPath;

    // Start is called before the first frame update
    void Start()
    {
    }

    // called by the button
    public void NextSeries(){
        _selectedSerieIndex ++;
        PopulateExerciseListElement();
    }

    // called by the button
    public void PreviousSeries(){
        _selectedSerieIndex --;
        PopulateExerciseListElement();
    }

    // populate the button with the correct values
    // called after we change _selectedSerieIndex or when we come from all_sessions_screen
    // TODO personalizar as strings
    public void  PopulateExerciseListElement(){
        // Main parameters
        if(!initialPopulate){
            PickExImg();
            CalculateTotalTime();
            _ListIndexText.text = _ListIndex;
            _NSeries = exercisesCount.ToString();
            _NSeriesText.text = "<b>Séries: </b>" + _NSeries;
            _NRepsText.text = "<b>Repetições: </b>" + _NReps;
            _ArmText.text = "<b>Braço: </b>" + _Arm;
            _DurationText.text = "<b>Duração: </b>" + _Duration;
            _RestimeText.text = "<b>T. Descanso: </b>" + _Restime;
            initialPopulate = true;
        }
        // Results
        _correctRepsText.text = "<b>Reps. corretas: </b>" + _correctReps[_selectedSerieIndex] + "/" + _NReps;
        _outOfPathsText.text =  "<b>Foras de trajeto: </b>" + _outOfPaths[_selectedSerieIndex];
        _totalTimeText.text = "<b>Tempo total: </b>" + _totalTime[_selectedSerieIndex];
        _averageTimeText.text ="<b>Tempo médio: </b>" + _averageTime[_selectedSerieIndex];
        // Compensatory
        ShowShoulderComp();
        ShowSpineComps();
        // Text
        numberText.text = (_selectedSerieIndex + 1).ToString();
        // Hide the previous/next_series buttons
        if(_selectedSerieIndex <= 0){
            prevSerieButton.SetActive(false);
        }
        else{
            prevSerieButton.SetActive(true);
        }
        if(_selectedSerieIndex >= exercisesCount - 1){
            nextSerieButton.SetActive(false);
        }
        else{
            nextSerieButton.SetActive(true);
        }
    }

    // this receives the parameters and results of one of the exercises from the sequence 
    public void AddExerciseInfo(Dictionary<string, string> newExercise){
        // planning variables (always the same)
        _Arm = newExercise["arm"];
        _ExType = newExercise["name"];
        _NReps = newExercise["nrReps"];
        _Duration = newExercise["duration"];
        _Restime = newExercise["restTime"];
        // results variables 
        _correctReps.Add(newExercise["correctReps"]);
        _outOfPaths.Add(newExercise["outOfPath"]);
        _correctReps.Add(newExercise["correctReps"]);
        _averageTime.Add(newExercise["avgTime"]);
        _totalTime.Add(newExercise["totalTime"]);
        // Compensatory variables
        _leftShoulderComps.Add(newExercise["leftShoulderComp"]);
        _rightShoulderComps.Add(newExercise["rightShoulderComp"]);
        _spineComps.Add(newExercise["spineComp"]);
        // lastly increase the control variable
        _ListIndex = newExercise["lastIndex"];
        exercisesCount ++;
    }

    // Aux functions

    void PickExImg(){
        switch(_ExType){
            case "Grelha":
                _ExImg.SetImage(0);
                break;
            case "Horizontal":
                _ExImg.SetImage(1);
                break;
            case "Vertical":
                _ExImg.SetImage(2);
                break;
            default:
                _ExImg.SetImage(-1);
                break;
        }
    }

    void CalculateTotalTime(){
        int minutes = 0;
        int seconds = 0;
        string expression;
        string[] numbers;
        // Format: 00:02 min
        for(int i = 0; i < _totalTime.Count; i++){
            expression = _totalTime[i].Split(' ')[0]; // 00:02
            numbers = expression.Split(':'); // 00:02
            minutes += Int32.Parse(numbers[0]); // 00
            seconds += Int32.Parse(numbers[1]); // 02
        }
        _TotalDurationText.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }

    void ShowShoulderComp(){
        _leftShoulderCompText.text = _leftShoulderComps[_selectedSerieIndex];
        _rightShoulderCompText.text = _rightShoulderComps[_selectedSerieIndex];

        int LShoulderComps = Int32.Parse(_leftShoulderComps[_selectedSerieIndex]);
        int RShoulderComps = Int32.Parse(_rightShoulderComps[_selectedSerieIndex]);
        
        if (LShoulderComps > 0 || RShoulderComps > 0)
        {
            if (LShoulderComps > 10)
            {
                LsholderImage.color =  new Color32(0xF9, 0x53, 0x53, 0xFF);
            }
            else if (LShoulderComps <= 10 && LShoulderComps > 5)
            {
                LsholderImage.color = new Color32(0xF3, 0xFF, 0x24, 0xFF);
            }
            else
            {
                LsholderImage.color = new Color32(0x4F, 0xFB, 0x7B, 0xFF);
            }

            if (RShoulderComps > 10)
            {
                RsholderImage.color = new Color32(0xF9, 0x53, 0x53, 0xFF);
            }
            else if (RShoulderComps <= 10 && RShoulderComps > 5)
            {
                RsholderImage.color = new Color32(0xF3, 0xFF, 0x24, 0xFF);
            }
            else
            {
                RsholderImage.color = new Color32(0x4F, 0xFB, 0x7B, 0xFF);
            }
        } 
        else
        {
            RsholderImage.color = new Color32(0x80, 0x80, 0x80, 0x80);
            LsholderImage.color = new Color32(0x80, 0x80, 0x80, 0x80);
        }
    }

    void ShowSpineComps()
    {
        _spineCompText.text = _spineComps[_selectedSerieIndex];
        int spineComps = Int32.Parse(_spineComps[_selectedSerieIndex]);

        if (spineComps > 0)
        {
            if (spineComps > 10)
            {
                SpineImage.color = new Color32(0xF9, 0x53, 0x53, 0xFF);
            }
            else if (spineComps <= 10 && spineComps > 5)
            {
                SpineImage.color = new Color32(0xF3, 0xFF, 0x24, 0xFF);
            }
            else
            {
                SpineImage.color = new Color32(0x4F, 0xFB, 0x7B, 0xFF);
            }
        } else
        {
            SpineImage.color = new Color32(0x80, 0x80, 0x80, 0x80);
        }
    }
}
