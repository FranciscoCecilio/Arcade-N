using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExerciseParametersPanel : MonoBehaviour
{
    // UI fields
    [Header("Debugging")]
    public SequenceListElement _selectedListSequence;
    public int exTypeIndex; // -1 NotDef , 0 Grid , 1 Left/RIght , 2 Up/Down
    public int armIndex; // -1 NotDef , 0 Left , 1 Right

    [Header("Overview Screen")]
    public Image ExImgPlaceholder;
    public TMP_Text ArmText;
    public TMP_Text NSeriesText;
    public TMP_Text NRepsText;
    public TMP_Text RestTimerText;

    [Header("Parameters Selection")]
    public TMP_InputField nSeriesField;
    public TMP_InputField nRepsField; 
    public TMP_InputField restTimerField;

    [Header("Flow")]
    public GameObject overview_screen;
    public GameObject arm_screen;
    public GameObject parameters_screen;


    // method called when the user clicks on a list element
    // TODO prevents if we are editing
    public void SetPanelActive(GameObject sequenceListElement){
        this.gameObject.SetActive(true);
        _selectedListSequence = sequenceListElement.GetComponent<SequenceListElement>();
        // Load Info regarding that sequence (if its a new one there isn't)
        LoadSequenceParameters(_selectedListSequence.GetSequence());
    }

    // Fetches the parameters from the selected sequence
    private void LoadSequenceParameters(Sequence seq){
        
        if(seq.getLength() == 0){ // This means the sequence was recently created and doesn't have any exercises yet.
            exTypeIndex = -1;
            armIndex = - 1;
            NSeriesText.text = "";
            NRepsText.text = "";
            RestTimerText.text = "";
        }
        else{
            exTypeIndex = seq.getExercisesIds()[0];
            // var beverage = (age >= 21) ? "Beer" : "Juice";
            armIndex = seq.getExercise(0).isLeftArm() ? 0 : 1 ; // armIndex is 0 if isLeftArm is true
            NSeriesText.text = seq.getSeries().ToString();
            NRepsText.text =  seq.getExercise(0).getNReps().ToString();
            RestTimerText.text = seq.getExercise(0).getRestTime().ToString();
        }
    }

    // checks if the fields were filled
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
        //emptyExercisesBox.SetActive(true);
    }

    public void Highlight_Arms(){
        //emptyArmsBox.SetActive(true);
    }

    // --------------------------------- Set Parameters Buttons ---------------------------------------------------
    
    // ExerciseCode: 0 Grid , 1 Left/Right , 2 Up/Down
    public void SetExerciseType(int exerciseCode){
        exTypeIndex = exerciseCode;
        //emptyExercisesBox.SetActive(false);
    }
    
    // Armcode: 0 Left , 1 Right
    public void SetArm(int armCode){
        exTypeIndex = armCode;
        //emptyArmsBox.SetActive(false);
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
