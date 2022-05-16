using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ExerciseImage : MonoBehaviour
{
    //enum exerciseType {Grelha, Horizontal, Vertical};
    
    [SerializeField] Sprite[] images;

    public Image imgPlaceholder;

    public TMP_Text txtPlaceholder;

    public void SetImage(int exTypeCode){
        switch(exTypeCode){
            case -1:
                imgPlaceholder.sprite = images[0];
                txtPlaceholder.text = "Indefinido";
                break;
            case 0:
                imgPlaceholder.sprite = images[0];
                txtPlaceholder.text = "Grelha";
                break;
            case 1:
                imgPlaceholder.sprite = images[1];
                txtPlaceholder.text = "Horizontal";
                break;
            case 2:
                imgPlaceholder.sprite = images[2];
                txtPlaceholder.text = "Vertical";
                break;
        }
    }

    public string GetTypeText(){
        return txtPlaceholder.text;
    }

}
