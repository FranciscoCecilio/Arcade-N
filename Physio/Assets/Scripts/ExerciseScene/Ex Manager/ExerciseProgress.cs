using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Left panel about Leaned and Comps text
public class ExerciseProgress : MonoBehaviour
{
    [Header("Exercise")]
    public TMP_Text exerciseName;
    public TMP_Text seriesText;
    
    [Header("Therapist Info")]
    public GameObject therapistInfo;
    public TMP_Text leaned;
    public TMP_Text shoulderLift;
    public TMP_Text outOfPath;

    // Start is called before the first frame update
    void Start()
    {
        // assigns the name of the exercise
        exerciseName.text = "--"+ State.exercise.getName()+"--";
        // assigns the serie number ("Série 1/3" means )
        seriesText.text = "Série " + SequenceManager.GetExerciseIndex().ToString() + "/" + SequenceManager.sequence.getSeries().ToString();
        // shows the therapist Info or not
        therapistInfo.SetActive(SequenceManager.isTherapistInfoOn);
    }

    // assigns the therapist Info every second
    public void Update() {
        leaned.text = "" + State.exercise.getSpineComp();
        shoulderLift.text = "" + (State.exercise.getLeftShoulderComp() + State.exercise.getRightShoulderComp());
        outOfPath.text = "" + State.exercise.getOutOfPath();
    }
}
