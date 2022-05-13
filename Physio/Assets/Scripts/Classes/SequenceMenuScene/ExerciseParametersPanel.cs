using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExerciseParametersPanel : MonoBehaviour
{
    public SequenceListElement _selectedListSequence;
    // UI fields
    public int exTypeIndex; // 0 Grid , 1 Left/RIght , 2 Up/Down
    public int armIndex; // 0 Left , 1 Right

    public GameObject emptyExercisesBox;
    public GameObject emptyArmsBox;

    public TMP_InputField nRepsField;
    public TMP_InputField nSeriesField;
    public TMP_InputField restTimerField;

    public void SetPanelActive(GameObject sequenceListElement){
        _selectedListSequence = sequenceListElement.GetComponent<SequenceListElement>();
        this.gameObject.SetActive(true);
        // Load Info regarding that sequence (if its a new one there isn't)

    }

    // checks if the fields are filled
    public void ConfirmButton(){
        bool exercise_was_selected = false;
        bool arm_was_selected = false;
       
        // Exercise type
        if(exTypeIndex == 0 || exTypeIndex == 1 || exTypeIndex == 2){
            exercise_was_selected = true;
        }

        // Arm
        if(armIndex == 0 || armIndex == 1){
            arm_was_selected = true;
        }

        // Confirm OR Highlight the buttons
        if(exercise_was_selected && arm_was_selected){
            // Confirm and Save on the Sequence List and txt file
            _selectedListSequence.SetSequenceParameters(exTypeIndex, armIndex, int.Parse(nSeriesField.text), int.Parse(nRepsField.text), int.Parse(restTimerField.text));
        }
        else{
            // Highlight exercises
            if(!exercise_was_selected){
                Highlight_Exercises();
            }
            // Highlight arm
            if(!arm_was_selected){
                Highlight_Arms();
            }
        }
    }

    // Highlights the UI to let users know they have to fill the fields
    public void Highlight_Exercises(){
        emptyExercisesBox.SetActive(true);
    }

    public void Highlight_Arms(){
        emptyArmsBox.SetActive(true);
    }

    // --------------------------------- Parameters Buttons ---------------------------------------------------
   
    // exerciseCode: 0 Grid , 1 Left/Right , 2 Up/Down
    public void SelectExerciseType(int exerciseCode){
        exTypeIndex = exerciseCode;
        emptyExercisesBox.SetActive(false);
    }

    // armcode: 0 Left , 1 Right
    public void SelectArm(int armCode){
        exTypeIndex = armCode;
        emptyArmsBox.SetActive(false);
    }

    // Series
    public void nSeriesUp()
    {
        if (nSeriesField != null)
        {
            if (!nSeriesField.text.Equals("")) nSeriesField.text = (int.Parse(nSeriesField.text) + 1).ToString();
            else nSeriesField.text = "2";
        }
    }

    public void nSeriesDown()
    {
        if (nSeriesField != null)
        {
            if (!nSeriesField.text.Equals("")) {
                if((int.Parse(nSeriesField.text) <= 1)) nSeriesField.text = "1";
                else nSeriesField.text = (int.Parse(nSeriesField.text) - 1).ToString();
            }
            else nSeriesField.text = "1";
        }
    }
    
    // Reps
    public void nRepsUp()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")) nRepsField.text = (int.Parse(nRepsField.text) + 1).ToString();
            else nRepsField.text = "2";
        }
    }

    public void nRepsDown()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")){
                if((int.Parse(nRepsField.text) <= 0)) nRepsField.text = "1";
                else nRepsField.text = (int.Parse(nRepsField.text) - 1).ToString();
            }
            else nRepsField.text = "1";
        }
    }

    // Timer
    public void restTimerUp()
    {
        if (restTimerField != null)
        {
            if (!restTimerField.text.Equals("")) restTimerField.text = (int.Parse(restTimerField.text) + 10).ToString();
            else restTimerField.text = "10";
        }
    }

    public void restTimerDown()
    {
        if (restTimerField != null)
        {
            if (!restTimerField.text.Equals("")){
                if((int.Parse(restTimerField.text) <= 0)) restTimerField.text = "0";
                else restTimerField.text = (int.Parse(restTimerField.text) - 10).ToString();
            }
            else restTimerField.text = "0";
        }
    }

}
