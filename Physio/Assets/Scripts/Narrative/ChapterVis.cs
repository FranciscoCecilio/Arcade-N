using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChapterVis : MonoBehaviour
{
    public Image image;
    public Text notFoundErrorText;
    
    
    public void SetupVis(int chapterNum){
        if (!System.IO.Directory.Exists(Application.dataPath + "/Narrative Materials/Chapter"+chapterNum.ToString())){ //nao ha o chapter
            notFoundErrorText.text = "Erro: " + Application.dataPath + "/Narrative Materials/Chapter"+chapterNum.ToString() + " nao encontrado.";
        }
        else{

        }
    } 
}
