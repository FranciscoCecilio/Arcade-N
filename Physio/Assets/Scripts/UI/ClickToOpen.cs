using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToOpen : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] objectsToOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach(GameObject obj in objectsToOpen){
            obj.SetActive(!obj.activeSelf);
        }
    }
    
}
