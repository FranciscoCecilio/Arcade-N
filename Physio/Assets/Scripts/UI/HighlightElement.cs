using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Color highlightColor;
    Color startColor;
    
    void Start()
    {
        startColor = image.color;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = highlightColor;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = startColor;
        Debug.Log("Mouse exit");
    }

    
}
