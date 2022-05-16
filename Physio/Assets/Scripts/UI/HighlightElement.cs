using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image image;
    public Color highlightColor;
    Color startColor;
    SequenceListElement seqListElement;
    ExerciseParametersPanel exPanel;
    
    void Start()
    {
        startColor = image.color;

        seqListElement = GetComponent<SequenceListElement>();
        if(seqListElement == null) Debug.Log("ERROR: Could not find the SequenceListElement script locaclly.");

        exPanel = GameObject.FindGameObjectWithTag ("ExerciseParametersPanel").GetComponent<ExerciseParametersPanel>();
        if(exPanel == null) Debug.Log("ERROR: Could not find the exercise parameters panel tag.");

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //image.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //image.color = startColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        image.color = highlightColor;
        exPanel.SetPanelActive(seqListElement);
    }
    
}
