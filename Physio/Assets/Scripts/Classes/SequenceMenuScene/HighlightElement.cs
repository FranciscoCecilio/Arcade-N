using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    SequenceListElement seqListElement;

    public Image backgroundImage;
    
    [SerializeField] Color hoverColor;
    [SerializeField] Color highlightColor;
    Color startColor;

    ExerciseParametersPanel exPanel;

    private bool isSelected = false;
    
    void Awake()
    {
        // Assign start color
        startColor = backgroundImage.color;

        // Find Sequence List Element
        seqListElement = GetComponent<SequenceListElement>();
        if(seqListElement == null) 
            Debug.Log("ERROR: Could not find the SequenceListElement script locaclly.");

        // Find Exercise Parameters Panel
        exPanel = GameObject.FindGameObjectWithTag ("ExerciseParametersPanel").GetComponent<ExerciseParametersPanel>();
        if(exPanel == null) 
            Debug.Log("ERROR: Could not find the exercise parameters panel tag.");

    }

    // Hovering the element will make its color react
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isSelected)
            backgroundImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)
            backgroundImage.color = startColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HighlightListElement();
    }

    public void HighlightListElement(){
        // DeHighlight every other listElement
        GameObject listContent = transform.parent.gameObject;
        int totalChild = listContent.transform.childCount;

        for (int i = 0; i < totalChild; i++)
        {
            if (i != transform.GetSiblingIndex())
            {
                Transform otherTransform = listContent.transform.GetChild(i);
                otherTransform.gameObject.GetComponent<HighlightElement>().DeHighlightElement();
            }
        }

        // Update control variable
        isSelected = true;
        // Highlight the color
        backgroundImage.color = highlightColor;
        
        //  Set active sequence in the ExerciseParameterPanel
        exPanel.SetPanelActive(seqListElement);
    }

    // makes the color goes back to normal
    public void DeHighlightElement(){
        backgroundImage.color = startColor;
        isSelected = false;
    }
    
}
