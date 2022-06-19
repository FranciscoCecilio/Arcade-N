using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class ChapterEntry : MonoBehaviour
{
    public TMP_Text caption;
    public Image photograph;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetPhotograph(int chapterNum, int chapterImg){
        
        Sprite sprite = Resources.Load<Sprite>("Narrative Materials/Chapter"+chapterNum.ToString() +"/"+ chapterImg.ToString());

        if(sprite == null){
            Debug.LogError("ERROR: Sprite not found in Resources/Narrative Materials/Chapter"+chapterNum.ToString() +"/"+ chapterImg.ToString()+ " not found.");
            // TODO put a default Is missing picture
        }
        else{
            photograph.sprite = sprite;
        }
    } 

    public void SetText(int chapterNum, int chapterImg){
        string textPath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+chapterNum.ToString() +"/Text/"+ chapterImg.ToString()+".txt";

        if (System.IO.File.Exists( textPath)){ 
            //Read the text from the .txt file
            StreamReader reader = new StreamReader(textPath);
            caption.text = reader.ReadToEnd();
            reader.Close();
        }
        else{
            Debug.LogError("ERROR: Text not found in " + textPath + " not found.");
            // TODO put a default Text
            caption.text = "ERRO: " + textPath + " not found.";
        }
    } 
}
