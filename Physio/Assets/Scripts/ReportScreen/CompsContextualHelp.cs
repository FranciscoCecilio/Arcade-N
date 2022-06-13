using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CompsContextualHelp : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text message;
    public GameObject box;
    public int specificComp; //0 spine, 1 right, 2 left

    public void ShowBox(){
        box.SetActive(true);
        switch(specificComp){
            case 0:
                ShowSpineCompText();
                break;
            case 1:
                ShowRightCompText();
                break;
            case 2:
                ShowLeftCompText();
                break;
        }
    }

    public void HideBox(){
        box.SetActive(false);
    }

    public void ShowSpineCompText(){
        message.text = "Número de compensações da coluna";
    }

    public void ShowRightCompText(){
        message.text = "Número de compensações do ombro direito";
    }

    public void ShowLeftCompText(){
        message.text = "Número de compensações do ombro esquerdo";
    }

     // Hovering the element will make its color react
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowBox();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideBox();
    }


}
