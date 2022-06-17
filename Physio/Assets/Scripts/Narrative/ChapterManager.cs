using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

// This script manages what chapters the user sees when they enter the narrative Scene.
// When we enter this scene from different places, we want to watch different things:
// 1) If we are starting the session (we come from Session Creation Scene), we want to 1. look at the preview of the _lastchapter; 2. turn the page; 3. Read the title and 1st image; 4. go to exercise screen
// 2) If we finished one serie of exercise (before SequenceManager.nextExercise()), we want to 1. unlock the image or images according to the XP and session_progress; 2. nextExercise()
// 3) If we come from Main Menu, we want to jump to the page according to the XP.
public class ChapterManager : MonoBehaviour
{
    // XP is a number from 0 to 500 (it is a multiple of 100) and defines the current chapter
    int currentChapter;
    // Session progress is a percentage (0 to 1) and defines the current image of a given chapter
    int currentImage; 
    // There are 4 types of pages: 1) Index; 2) Chapter name and 1st Image; 3) Two images 4) Left image and empty page (TODO: this could have the chapter preview)
    
    public GameObject[] pageTypes;
    
    [Header("1) Index")]
    //public TMP_Text indice;
    [Header("2) Chapter name and 1st Image")]
    public TMP_Text chapterName_text;
    public ChapterEntry right_chapter_2;
    [Header("3) Two images")]
    public ChapterEntry left_chapter_3;
    public ChapterEntry right_chapter_3;
    [Header("4) Left image and empty page")]
    public ChapterEntry left_chapter_4;
    public TMP_Text preview_text;
    
    [Header("SoundManager")]
    public SoundManager soundManager;

    public int currentPageIndex;
    public int currentPageType;
    public GameObject currentPage;

    // we can enter this 
    public void ShowNarrative(){
        currentChapter = 1;
        currentImage = 1;
        //currentChapter = SessionInfo.getXP() % 100 + 1;
        //currentImage = GetCurrentImage();
        
        // 1) If we are starting the session (we come from Session Creation Scene), we want to:
        // 1. look at the preview of the _lastchapter; 2. turn the page; 3. Read the title and 1st image; 4. go to exercise screen
        currentPageType = 2;
        SetPage(currentPageType);

    }

    public void SetPage(int pagetype){
        // show current page and set variables
        ShowPage(pagetype);
        // place the correct vis or titles
        switch(pagetype){
            case 1: // index
                // TODO
                break;
            case 2: // 2) Chapter name and 1st Image
                SetChapterTitle(chapterName_text);
                SetChapterUI(right_chapter_2);
                break;
            case 3: // 3) Two images
                SetChapterUI(left_chapter_3);
                SetChapterUI(right_chapter_3);
                break;
            case 4: // 4) Left image and empty page
                SetChapterUI(left_chapter_4);
                SetChapterTitle(preview_text);
                break;
        }
    }

    public void ShowPage(int pagetype){
        // hide all pages
        for(int i = 0; i < pageTypes.Length; i++){
            pageTypes[i].SetActive(false);
        }
        // reveal the correct
        pageTypes[pagetype - 1].SetActive(true);
        // set the currentPage
        currentPage = pageTypes[pagetype - 1];
    }

    public void SetChapterTitle(TMP_Text title){
        string titlePath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/Text/Title.txt";

        if (System.IO.File.Exists( titlePath)){ 
            //Read the title directly from the Title.txt
            StreamReader reader = new StreamReader(titlePath);
            title.text = reader.ReadToEnd();
            reader.Close();
        }
        else{
            Debug.Log("ERROR: " + titlePath + " not found.");
            title.text = "ERRO: " + titlePath + " not found.";
        }
    }

    public void SetPreviewText(TMP_Text prev){
        string previewPath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/Text/Preview.txt";

        if (System.IO.File.Exists( previewPath)){ 
            //Read the title directly from the Title.txt
            StreamReader reader = new StreamReader(previewPath);
            prev.text = reader.ReadToEnd();
            reader.Close();
        }
        else{
            Debug.Log("ERROR: " + previewPath + " not found.");
            prev.text = "ERRO: " + previewPath + " not found.";
        }
    }

    // modifies a chapterVis with the correct photograph and text
    public void SetChapterUI(ChapterEntry vis){
        vis.SetPhotograph(currentChapter,currentImage);
        vis.SetText(currentChapter,currentImage);
    }

    // gives the number of the unlocked image
    int GetCurrentImage(){
        bool isCapituloImpar;
        // check the level and progress to unlock the image
        switch(SessionInfo.getXP()){
            case 0: // o que fazer?? quando vem do main menu
                return 1;
            case 1:
                isCapituloImpar = false;
                break;
            case 2:
                isCapituloImpar = true;
                break;
            case 3:
                isCapituloImpar = false;
                break;
            case 4:
                isCapituloImpar = true;
                break;
            case 5:
                isCapituloImpar = false;
                break;
            default:
                Debug.Log("XP marado: não é multiplo de 100 entre 0 e 500."+SessionInfo.getXP());
                return -1;
        }
        if(isCapituloImpar){
            // 5 images per chapter - we want to unlock one image every 20% sessionProgression is completed
            switch(SequenceManager.GetSessionProgressionPerc()){
                case >= 1f:
                    return 5;
                case >= 0.80f:
                    return 4;
                case >= 0.60f:
                    return 3;
                case >= 0.40f:
                    return 2;
                case >= 0.20f:
                    return 1;
                default:
                    Debug.Log("sessionProgress is too low to unlock images: " + SequenceManager.GetSessionProgressionPerc());
                    return -1;
            }
        }
        else{
            // 4 images per chapter - we want to unlock one image every 25% sessionProgression is completed
            switch(SequenceManager.GetSessionProgressionPerc()){
                case >= 1f:
                    return 4;
                case >= 0.75f:
                    return 3;
                case >= 0.50f:
                    return 2;
                case >= 0.25f:
                    return 1;
                default:
                    Debug.Log("sessionProgress is too low to unlock images: " + SequenceManager.GetSessionProgressionPerc());
                    return -1;
            }
        }
    }


    void Start()
    {
        // Open the correct page
        ShowNarrative();
        
    }

    // open the next page
    public void TurnRight(){
        /*if(currentPageIndex + 1 < pageTypes.Length){
            // turn off the current page
            currentPage.SetActive(false);
            // set the variables
            currentPageIndex += 1;
            currentPage = pages.GetChild(currentPageIndex).gameObject;
            // open a new page
            currentPage.SetActive(true);
            // play page turning sound
            soundManager.PlayOneShot("PageFlip");
        }*/
    }
    // open the previous page
    public void TurnLeftt(){
        /*if(currentPageIndex - 1 > 0){
            // turn off the current page
            currentPage.SetActive(false);
            // set the variables
            currentPageIndex -= 1;
            currentPage = pages.GetChild(currentPageIndex).gameObject;
            // open a new page
            currentPage.SetActive(true);
            // play page turning sound
            soundManager.PlayOneShot("PageFlip");
        }*/
    }
}
