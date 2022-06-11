using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExerciseResult : MonoBehaviour
{
    [Header("Main parameters")]
    public TMP_Text _TotalDurationText;
    public ExerciseImage _ExImg;

    int _TotalDuration;
    int _ExType;

    [Header("Planned paremeters")]
    public TMP_Text _NSeriesText;
    public TMP_Text _NRepsText;
    public TMP_Text _ArmText;
    public TMP_Text _DurationText;
    public TMP_Text _RestimeText;

    int _NSeries;
    int _NReps;
    string _Arm;
    int _Duration;
    int _Restime;
    
    

    [Header("Results")]
    public TMP_Text _correctRepsText;
    public TMP_Text _outOfPathsText;
    public TMP_Text _totalTimeText;
    public TMP_Text _averageTimeText;

    int[] _correctReps;
    int[] _outOfPaths;
    int[] _totalTime;
    int[] _averageTime;

    

    //[Header("Mistakes")]
    
    int[] _spineCompensations;

    // Indexes
    int _selectedSerieIndex;
    int _listIndex;

    // sequence folder path
    public string folderPath;

    // Start is called before the first frame update
    void Start()
    {
        //PopulateExerciseListElement();
    }

    void PopulateExerciseListElement(){

    }
    
}
