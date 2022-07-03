using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridPaternManager : MonoBehaviour
{
    [Header("Encoding")]
    public int[] spiralPatern = new int[12] {0,1,5,9,11,10,6,2,3,4,8,7};
    public int[] horizontalSPatern = new int[12] {0,1,5,4,3,2,6,7,8,9,11,10};
    public int[] verticalSPatern = new int[12] {6,2,0,3,7,10,11,8,4,1,5,9};

    [Header("Button Highlight Images")]
    public Image spiralImage;
    public Image horizontalSImage;
    public Image verticalSImage;

    [Header("Control Variables")]
    public int[] chosenPatern; 
    public string chosenPaternName;
    int targetHitCounter = 0; // the indicator of progress (we increment it from 0 to 11 everytime we hit the correct target)

    public BodyScreenController bodyController; // to restart
    public ExercisePreferencesSetup exercisePreferences; // to restart

    public int GetCurrentTargetID(){
        // returns the id of the current target (any number 0 to 11)
        if(targetHitCounter >= chosenPatern.Length) return chosenPatern[chosenPatern.Length - 1];
        return chosenPatern[targetHitCounter];
    }

    public int GetTargetHitCounter(){
        // returns the indicator of progress 
        return targetHitCounter;
    }

    public void IncrementTargetHitCounter(){
        targetHitCounter ++;
    }

    // used to reset the counter between repetitions of the same serie
    public void SetTargetHitCounter(int count){
        targetHitCounter = count;
    } 

    // ----------------------------------- when we are Editing we have a button for every patern. OR at START by ExercisePreferences script
    public void SetChosenPatern(string paterName){
        // Clear the highlighted buttons
        spiralImage.gameObject.SetActive(false);
        horizontalSImage.gameObject.SetActive(false);
        verticalSImage.gameObject.SetActive(false);

        switch(paterName){
            case "spiralPatern":
                chosenPatern = spiralPatern;
                spiralImage.gameObject.SetActive(true);
                break;
            case "horizontalSPatern":
                chosenPatern = horizontalSPatern;
                horizontalSImage.gameObject.SetActive(true);
                break;
            case "verticalSPatern":
                chosenPatern = verticalSPatern;
                verticalSImage.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Unkown paternName: " + paterName);
                return;
        }
        // define the chosen patern name
        chosenPaternName = paterName;

        if(State.isTherapyOnGoing){
            StartCoroutine(SaveAndRestart());
        }
    }

    IEnumerator SaveAndRestart(){
        // (button only) if the exercise has already started... then we must save the correct patern + restart the grid exercise.
        exercisePreferences.SavePreferencesToFile();
        yield return new WaitForSeconds(1);
        bodyController.Restart();
    }

}
