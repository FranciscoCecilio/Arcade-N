using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExerciseParametersPanel : MonoBehaviour
{
    [Header("Debugging")]
    public SequenceListElement _selectedListSequence;
    public int exTypeIndex; // -1 NotDef , 0 Grid , 1 Left/RIght , 2 Up/Down
    public int armIndex; // -1 NotDef , 0 Left , 1 Right
    public ExerciseParametersFlow flowManager;

    [Header("Overview Screen")]
    public ExerciseImage ExImg;
    public TMP_Text ArmText;
    public TMP_Text NSeriesText;
    public TMP_Text NRepsText;
    public TMP_Text RestTimerText;

    [Header("Parameters Selection")]
    public TMP_InputField nSeriesField;
    public TMP_InputField nRepsField; 
    public TMP_InputField restTimerField;
    

    // method called when the user clicks on a list element
    // TODO prevents if we are editing
    public void SetPanelActive(SequenceListElement seqlistElement){
        _selectedListSequence = seqlistElement;
        LoadSequenceParameters(_selectedListSequence.GetSequence());
        flowManager.StartEditing();
    }

    // Fetches the parameters from the sequence list element that was clicked and UPDATES the overview_screen fields
    private void LoadSequenceParameters(Sequence seq){

        if(seq.getLength() == 0){ // This means the sequence was recently created and doesn't have any exercises yet.
            exTypeIndex = -1;
            armIndex = - 1;
            // Fields on Paremeter Screen
            NSeriesText.text = "-";
            NRepsText.text = "-";
            RestTimerText.text = "-";
        }
        
        else{
            exTypeIndex = seq.getExercisesIds()[0];
            armIndex = seq.getExercise(0).isLeftArm() ? 0 : 1 ; // armIndex is 0 if isLeftArm is true
            // Fields on Paremeter Screen
            NSeriesText.text = seq.getSeries().ToString();
            NRepsText.text =  seq.getExercise(0).getNReps().ToString();
            RestTimerText.text = seq.getExercise(0).getRestTime().ToString();
        }

        // Fields on Overview Screen
        ExImg.SetImage(exTypeIndex);
        ArmText.text = GetArmString();
    }

    // This method could be directly on flow manager...
    // Concludes Editing a Sequence by setting the new parameters (flow manager checks if the fields were filled and calls this)
    public void FinishEditing(){
        _selectedListSequence.SetSequenceParameters(exTypeIndex, armIndex, int.Parse(nSeriesField.text), int.Parse(nRepsField.text), int.Parse(restTimerField.text));

    }

    // This method receives and sets the new values and UPDATES the fields on the OverViewScreen 
    // Called on the flow manager after EDITING or CANCELING
    public void UpdateOverViewScreen(int newTypeIndex, int newArmIndex, int newSeries, int newReps, int newRestTime){
        
        exTypeIndex = newTypeIndex;
        ExImg.SetImage(exTypeIndex);

        armIndex = newArmIndex;
        ArmText.text = GetArmString();

        NSeriesText.text = newSeries.ToString();
        NRepsText.text = newReps.ToString();
        RestTimerText.text = newRestTime.ToString();
    }

    private string GetArmString(){
        string armString ="";
        switch(armIndex){
            case -1:
                armString = "-";
                break;
            case 0:
                armString = "Esquerdo";
                break;
            case 1:
                armString = "Direito";
                break;
        }
        return armString;
    }
    
    // --------------------------------- Set Parameters ---------------------------------------------------
    
    // ExerciseCode: 0 Grid , 1 Left/Right , 2 Up/Down
    public void SetExerciseType(int exerciseCode){
        // Define the index
        exTypeIndex = exerciseCode;
        // next panel
        flowManager.NextPanel();
        
    }
    
    // Armcode: 0 Left , 1 Right
    public void SetArm(int armCode){
        // Define the index
        armIndex = armCode;
        // next panel
        flowManager.NextPanel();
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
                if((int.Parse(nRepsField.text) <= 1)) nRepsField.text = "1";
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
