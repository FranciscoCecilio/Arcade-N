using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// A list Element is instantiated in two places: CRIAR SEQUENCIA button and SequenceListControl.Start()  
public class SequenceListElement : MonoBehaviour
{
    public int typeIndex;
    public ExerciseImage exImg;
    public TMP_Text armText;
    public TMP_Text nRepsField;
    public TMP_Text nSeriesField;
    public TMP_Text restTimerField;
    
    private Sequence _sequence;

    public Sequence GetSequence(){
        if(_sequence == null){
            Debug.Log("Error: Tried to Get a null sequence.");
            return null;
        }
        else 
            return _sequence;
    }


    public void SetSequence(Sequence sequence)
    {
        _sequence = sequence;

        // Also sets the parameteres in the list element
        int exTypeIndex;
        int armIndex;
        int nSeries;
        int nReps;
        int restTime;

        if(_sequence.getLength() == 0){ // This means the sequence was recently created and doesn't have any exercises yet.
            exTypeIndex = -1;
            armIndex = - 1;
            nSeries = 0;
            nReps = 0;
            restTime = 0;        
        }
        else{
            exTypeIndex = _sequence.getExercisesId();
            // var beverage = (age >= 21) ? "Beer" : "Juice";
            armIndex = _sequence.getExercise(0).isLeftArm() ? 0 : 1 ; // armIndex is 0 if isLeftArm is true
            nSeries= _sequence.getSeries();
            nReps =  _sequence.getExercise(0).getNReps();
            restTime = _sequence.getExercise(0).getRestTime();
        }

        SetSequenceParameters(exTypeIndex, armIndex, nSeries, nReps, restTime);
    }
    
    // Called from Exercise Panel on CONFIRM button
    // Populates the List Element with the Sequence values; Stores in the sequence; [NOT:Saves to a file]
    public void SetSequenceParameters(int exTypeIndex, int armIndex, int nSeries, int nReps, int restTime){

        // place the info in the placeholders
        typeIndex = exTypeIndex;
        exImg.SetImage(typeIndex);

        if(armIndex == 0) armText.text = "Esquerdo";
        else if(armIndex == 1) armText.text = "Direito";
        else armText.text = "-"; // armIndex == -1

        nSeriesField.text = nSeries.ToString();
        nRepsField.text = nReps.ToString();
        restTimerField.text = restTime.ToString();

        // save info on the sequence itself
        _sequence.setSeries(nSeries);
        _sequence.clearExerciseList();
        for(int i = 0; i < nSeries; i++){
            // create X=n_series Exercises of the same type with Y=nReps reptitions
            // exe=2=Left/Right=Exercise2Scene=right=10=60=60
            // Exercise(int id, string name, string scenePath, string arm, int nreps, int duration, int restTime)
            _sequence.addExercise(new Exercise(exTypeIndex,  exImg.GetTypeText() , "Exercise"+exTypeIndex.ToString()+"Scene" , armIndex, nReps, 60, restTime));
        }
    }

    // Requests to Delete to SequenceListControl.cs
    public void DeleteSequenceButton(){
        //Search the script that controls the buttons of the list
        GameObject sequencesList = GameObject.Find("Sequences");
        
        if(sequencesList == null) Debug.Log("ERROR: tried to find Sequences object in scene but is null");

        SequenceListControl script = (SequenceListControl) sequencesList.GetComponent(typeof(SequenceListControl));
        
        // Selects this as active sequence and opens delete dialogue
        script.ActiveSequence(_sequence);
        script.OpenDeleteDialogue();
    }

}

