using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class ChapterEntry : MonoBehaviour
{
    public Image photograph;
    public TMP_Text caption;
    public TextWriter textWriter;
    public bool isOverviewPanel;
    void Awake()
    {
        if(isOverviewPanel) return;
        photograph.gameObject.SetActive(false);
        caption.gameObject.SetActive(false);
    }

    public void SetPhotograph(int chapterNum, int chapterImg, bool shouldAnimate){
        
        Sprite sprite = Resources.Load<Sprite>("Narrative Materials/Chapter"+chapterNum.ToString() +"/"+ chapterImg.ToString());

        if(sprite == null){
            Debug.LogError("ERROR: Sprite not found in Resources/Narrative Materials/Chapter"+chapterNum.ToString() +"/"+ chapterImg.ToString()+ " not found.");
            // TODO put a default Is missing picture
        }
        else{
            photograph.gameObject.SetActive(true);
            if(shouldAnimate){
                // TODO set animation!
                photograph.sprite = sprite;
            }
            else{
                photograph.sprite = sprite;
            }
            
        }
    } 

    public void SetText(int chapterNum, int chapterImg, bool shouldAnimate){
        string textPath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+chapterNum.ToString() +"/Text/"+ chapterImg.ToString()+".txt";

        if (System.IO.File.Exists( textPath)){ 
            //Read the text from the .txt file
            caption.gameObject.SetActive(true);
            StreamReader reader = new StreamReader(textPath);
            string textToWrite = reader.ReadToEnd();
            reader.Close();
            if(shouldAnimate){
                textWriter.AddWriter(caption, textToWrite, .05f, true);
            }
            else{
                caption.text = textToWrite;
            }
        }
        else{
            Debug.LogError("ERROR: Text not found in " + textPath + " not found.");
            // TODO put a default Text
            caption.text = "ERRO: " + textPath + " not found.";
        }
    } 
}
