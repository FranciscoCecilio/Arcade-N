using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowNextExercise : MonoBehaviour
{
    public GameObject info;

    public GameObject nextPanel;
    public GameObject nextButton;

    public GameObject emptyPanel;
    public GameObject finishButton;

    public ExerciseImage ExImg;
    public TMP_Text ArmText;
    public TMP_Text NSeriesText;
    public TMP_Text NRepsText;
    public TMP_Text RestTimerText;

    void Start()
    {
        PopulatePanel();
        info.SetActive(false);
    }

    public void PopulatePanel(){
        Exercise nextExercise = SequenceManager.GetNextExercise();
        // Hide the empty vis and finish button AND opens the Next vis and button
        if(nextExercise != null){
            nextPanel.SetActive(true);
            nextButton.SetActive(true);
            emptyPanel.SetActive(false);
            finishButton.SetActive(false);

            ExImg.SetImage(nextExercise.getId());

            int armIndex = nextExercise.isLeftArm() ? 0 : 1 ; // armIndex is 0 if isLeftArm is true
            ArmText.text = GetArmString(armIndex);

            NSeriesText.text = SequenceManager.sequence.getLength().ToString();

            NRepsText.text = nextExercise.getNReps().ToString();
            
            RestTimerText.text = nextExercise.getRestTime().ToString();
        }
        // Hide the next vis and button AND opens the Empty vis and finish button
        else{
            nextPanel.SetActive(false);
            nextButton.SetActive(false);
            emptyPanel.SetActive(true);
            finishButton.SetActive(true);
        }
    }

    private string GetArmString(int armIndex){
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
}
