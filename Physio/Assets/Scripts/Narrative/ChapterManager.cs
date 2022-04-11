using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public GameObject ChapterUI;

    public void SetChapterUI(int chapterNum){
        if(ChapterUI.activeSelf) ChapterUI.SetActive(false);
        else{
            ChapterUI.SetActive(true);
            ChapterUI.GetComponent<ChapterVis>().SetupVis(chapterNum);
           // Assets/Narrative Materials/Chapter1
        }
    }
}
