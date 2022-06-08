using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChapterEntry : MonoBehaviour
{
    public int chapter;
    public int image;
    public TMP_Text text;
    public Image photograph;
    public bool unlocked;

    void Start()
    {
        if(!unlocked) 
            Hide();
        
    }

    public void Unlock(){
        unlocked = true;
        photograph.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }

    public void Hide(){
        photograph.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
