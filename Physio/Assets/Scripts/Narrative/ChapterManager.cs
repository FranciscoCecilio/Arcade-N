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

    public int currentPageIndex;
    public GameObject currentPage;
    public Transform pages;
    public SoundManager soundManager;

    void Start()
    {
        int currentXP = SessionInfo.getXP();
        // Open the correct page
        OpenCorrectPage(currentXP);
        // Hides the unkown chapters in the index page
        
    }

    void OpenCorrectPage(int currentXP){
        int level = currentXP / 100;
        switch(level){
            // index
            case 0:
                currentPageIndex = 0;
                currentPage = pages.GetChild(0).gameObject;
                currentPage.SetActive(true);
                break;
            // 1st chapter
            case 1:
                currentPageIndex = 1;
                currentPage = pages.GetChild(1).gameObject;
                currentPage.SetActive(true);
                break;
            // 2nd chapter
            case 2:
                currentPageIndex = 3;
                currentPage = pages.GetChild(3).gameObject;
                currentPage.SetActive(true);
                break;
            // 3rd chapter
            case 3:
                currentPageIndex = 5;
                currentPage = pages.GetChild(5).gameObject;
                currentPage.SetActive(true);
                break;
            // 4th chapter
            case 4:
                //pages.GetChild(3).gameObject.SetActive(true);
                break;
            // 5th chapter
            case 5:
                //pages.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }
    // open the next page
    public void TurnRight(){
        if(currentPageIndex + 1 < pages.childCount){
            // turn off the current page
            currentPage.SetActive(false);
            // set the variables
            currentPageIndex += 1;
            currentPage = pages.GetChild(currentPageIndex).gameObject;
            // open a new page
            currentPage.SetActive(true);
            // play page turning sound
            soundManager.PlayOneShot("PageFlip");
        }
    }
    // open the previous page
    public void TurnLeftt(){
        if(currentPageIndex - 1 > 0){
            // turn off the current page
            currentPage.SetActive(false);
            // set the variables
            currentPageIndex -= 1;
            currentPage = pages.GetChild(currentPageIndex).gameObject;
            // open a new page
            currentPage.SetActive(true);
            // play page turning sound
            soundManager.PlayOneShot("PageFlip");
        }
    }
}
