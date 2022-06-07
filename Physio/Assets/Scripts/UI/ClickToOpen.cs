using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Used to turn on/off NextExercisePanel AND TherapistInfo
public class ClickToOpen : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectToOpen;
    public bool isNextPanel;
    public bool isTherapistInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        objectToOpen.SetActive(!objectToOpen.activeSelf);

        if(isNextPanel){
            SequenceManager.isNextPanelOn = objectToOpen.activeSelf;
        }
        else if(isTherapistInfo){
            SequenceManager.isNextPanelOn = objectToOpen.activeSelf;
        }
    }
    
}
