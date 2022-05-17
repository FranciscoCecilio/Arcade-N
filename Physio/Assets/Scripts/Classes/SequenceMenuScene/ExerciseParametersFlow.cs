using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ExerciseParametersFlow : MonoBehaviour
{
    [Header("Debugging")]
    public bool isEditing = false;
    public int panelCode; //0 to 4 that represent the screens
    public ExerciseParametersPanel exPanel;

    [Header("Flow")]
    public GameObject overview_screen;
    public GameObject exercise_screen;
    public GameObject arm_screen;
    public GameObject parameters_screen;

    [Header("Flow Buttons")]
    public Button EditButton;
    public Button ConfirmButton;
    public Button CancelButton;

    [Header("Exercise Selection")]
    public Button[] ExerciseButtons;
    
    [Header("Arm Selection")]
    public Button[] ArmButtons;

    // we want to EDIT in two cases: 
    // 1. After creating a new Sequence in SequenceMenuScript.confirmNameSequence()
    // 2. Clicking button EDIT on the overview_screen
    public void StartEditing(){
        if(isEditing){
            Debug.Log("Cannot edit right now.");
            return;
        }
        // Close all possible screens
        overview_screen.SetActive(false);
        arm_screen.SetActive(false);
        parameters_screen.SetActive(false);
        // Open Exercise Type Selection
        exercise_screen.SetActive(true);
        // Enable and Disable buttons
        EditButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(true);
        ConfirmButton.gameObject.SetActive(true);
        // Set the control variables
        isEditing = true;
        panelCode = 1;
    }

    // Finish Edition (called by the CONFIRM button)
    public void ConfirmChanges(){
        // Close all possible screens
        exercise_screen.SetActive(false);
        arm_screen.SetActive(false);
        parameters_screen.SetActive(false);
        // Open Overview_Screen
        overview_screen.SetActive(true);
        // Set the control variables
        isEditing = false;
        panelCode = 0;
        // Enable and Disable buttons
        EditButton.gameObject.SetActive(true);
        CancelButton.gameObject.SetActive(false);
        ConfirmButton.gameObject.SetActive(false);
        // Calls method on exPanel to set the changes
        exPanel.EditConclusion();
    }

    // Called by the "back" arrow in the panel
    public void PreviousPanel(){
        switch(panelCode){
            case 0:
                Debug.Log("ERROR: there is a bug with panelCode: 0");
                break;
            case 1:
                Debug.Log("ERROR: there is a bug with panelCode: 1");
                break;
            case 2:
                arm_screen.SetActive(false);
                exercise_screen.SetActive(true);
                panelCode = 1;
                PanelButtonsSelection();
                break;
            case 3:
                parameters_screen.SetActive(false);
                arm_screen.SetActive(true);
                panelCode = 2;
                PanelButtonsSelection();
                break;
        }
    }

    // Called by the "next" arrow in the panel
    public void NextPanel(){
        switch(panelCode){
            case 0:
                Debug.Log("ERROR: there is a bug with panelCode: 0");
                break;
            case 1:
                exercise_screen.SetActive(false);
                arm_screen.SetActive(true);
                panelCode = 2;
                PanelButtonsSelection();
                break;
            case 2:
                arm_screen.SetActive(false);
                parameters_screen.SetActive(true);
                panelCode = 3;
                break;
            case 3:
                Debug.Log("ERROR: there is a bug with panelCode: 3");
                break;
        }
    }

    // When a the Type and Arm panels are opened, this method is called to select (or hihglight) the buttons that were assigned
    public void PanelButtonsSelection(){
        // Buttons on Exercise Screen
        if(panelCode == 1){
            for(int i = 0; i < ExerciseButtons.Length; i++){
                if(i == exPanel.exTypeIndex){
                    ExerciseButtons[i].Select();
                }
                else{
                    ExerciseButtons[i].OnDeselect(null);
                }
            }
        }
        else if(panelCode == 2){
            // Buttons on Arm Screen
            for(int i = 0; i < ArmButtons.Length; i++){
                if(i == exPanel.armIndex){
                    ArmButtons[i].Select();
                }
                else{
                    ArmButtons[i].OnDeselect(null);
                }
            }
        }
    }
    
    // checks if the CONFIRM button should be enabled (if exercise type and arm were selected)
    void Update()
    {
        if(isEditing){
            bool exercise_was_selected = false;
            bool arm_was_selected = false;
            
            // Fetch these values from Panel
            int exTypeIndex = exPanel.exTypeIndex; 
            int armIndex = exPanel.armIndex;

            // Exercise type
            if(exTypeIndex == 0 || exTypeIndex == 1 || exTypeIndex == 2){
                exercise_was_selected = true;
            }

            // Arm
            if(armIndex == 0 || armIndex == 1){
                arm_was_selected = true;
            }

            // Enable 
            if(exercise_was_selected && arm_was_selected){
                ConfirmButton.interactable = true;
            }
            // Disable
            else{
                ConfirmButton.interactable = false;
            }
        }
    }
}