using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


// This script manages what chapters the user sees when they enter the narrative Scene.
// When we enter this scene from different places, we want to watch different things:
// 1) If we are starting the session (we come from Session Creation Scene), we want to // 1. look at the preview of the _lastchapter; 2. go to exercise screen
// 2) If we finished one serie of exercise (before SequenceManager.nextExercise()), we want to // 1. unlock the image or images according to the XP and session_progress; 2. nextExercise()
// 3) If we come from Main Menu, we want to jump to the page according to the XP.
public class ChapterManager : MonoBehaviour
{
    // XP is a number from 0 to 500 (it is a multiple of 100) and defines the current chapter
    int currentChapter;
    // Session progress is a percentage (0 to 1) and defines the current image of a given chapter
    int currentImage; 
    // There are 4 types of pages: 1) Index; 2) Chapter name and 1st Image; 3) Two images 4) Left image and empty page (TODO: this could have the chapter preview)
    
    public GameObject[] pageTypes;
    public Button skipButton;

    [Header("1) Overview")]
    //public TMP_Text indice;
    [Header("2) Chapter name and 1st Image")]
    public TMP_Text chapterName_text;
    public ChapterEntry right_chapter_2;
    [Header("3) Two images")]
    public ChapterEntry left_chapter_3;
    public ChapterEntry right_chapter_3;
    [Header("4) Left image and empty")]
    public ChapterEntry left_chapter_4;
    [Header("5) Preview Image and Text")]
    public ChapterEntry preview_image;
    public TMP_Text preview_text;
    
    [Header("Voice Assistant")]
    public VoiceAssistant voiceAssistant;
    public SoundManager soundManager;

    [Header("Debugging")]
    public int currentPageIndex;
    public int currentPageType;
    public GameObject currentPage;
    public bool isChapterOdd;

    bool skipFlag_showChapter = false;
    void Start()
    {
        // when there are no more chapters to unlock we just show overview "the end"
        if(SequenceManager.showOverview){
            SequenceManager.showOverview = false;
            ShowPage(1);
        }
        else{
            ShowNarrative();
        }
    }

    public void ShowNarrative(){

        currentChapter = SequenceManager.GetCurrentChapter();

        if(currentChapter % 2 == 0){
            isChapterOdd = false;
        }
        else{
            isChapterOdd = true;
        }

        // we can enter from the exercise: 
        // 1) to see the preview 
        if(SequenceManager.hasPreviewToUnlock){
            SequenceManager.hasPreviewToUnlock = false;
            currentChapter -= 1; 
            if(currentChapter == 1){
                // SHOW entire 1st chapter (introduction)
                // 1. look at the title and 1st page ; 2. flip pages ; 3. go to exercise screen
                StartCoroutine(ShowEntireChapter(1));
            }
            else{
                // ANIMATE preview 
                // 1. look at the preview of the _lastchapter; 2. go to exercise screen
                //StartCoroutine(ShowPreview());
                
                // UPDATE : shoe entire chapter instead of a preview text
                StartCoroutine(ShowEntireChapter(currentChapter));
            }
            
        }
        // 2) to see the unlocked images
        else if(SequenceManager.hasImagesToUnlock){
            // ANIMATE images 
            // we want to 1. unlock the image or images according to the XP and session_progress; 2. nextExercise()
            SequenceManager.hasImagesToUnlock = false;
            // Set the SKIP button to false (because there is no time to code when we need to unlock images)
            skipButton.gameObject.SetActive(false);
            StartCoroutine(ShowAndReadUnlockedImages());
        }
        // or from the Main Menu
        else{
            // Show the entire last chapter
            currentChapter =  SessionInfo.getXP() / 100 - 1;
            // Set the SKIP button to false (because we only use it to go back to exercise screen)
            skipButton.gameObject.SetActive(false);

            if(currentChapter < 1) currentChapter = 1;
            StartCoroutine(ShowEntireChapter(currentChapter));
        }
    }

    
    // unlocks all images from a given chapter. plays a voice line for each image (used to preview previous chapter)
    private IEnumerator ShowEntireChapter(int chapterToSee){
        float clipLength;
        int chapterImages = 0;
        currentChapter = chapterToSee;
        if(currentChapter % 2 == 0){
            isChapterOdd = false;
            chapterImages = 4;
        }
        else{
            isChapterOdd = true;
            chapterImages = 5;
        }
    
        for(int i = 0; i < chapterImages; i++){
            currentImage = i + 1;
            clipLength = ShowImageAndText(currentImage);
            yield return new WaitForSeconds(clipLength);
        }
        // we showed all the unlocked images - Bye Narrative screen: Return to Exercise!!
        if(SequenceManager.GetCurrentChapter() < 2) SequenceManager.SetCurrentChapter(2);
        if(SequenceManager.sequence != null) SequenceManager.nextExercise();
    }

     // unlocks all images that were unlocked in exercise. plays a voice line for each image
    private IEnumerator ShowAndReadUnlockedImages(){
        float clipLength;
        for(int i = 0; i < SequenceManager.unlockedChaptersEncoding.Count; i ++){
            if(SequenceManager.unlockedChaptersEncoding[i] == 1){
                currentImage = i + 1;
                clipLength = ShowImageAndText(currentImage);
                yield return new WaitForSeconds(clipLength);
            }
        }
        // we showed all the unlocked images - Bye Narrative screen: Return to Exercise!!
        if(SequenceManager.sequence != null) SequenceManager.nextExercise();
    }

    // returns the clipLenght
    public float ShowImageAndText(int imageToShow){
        float clipLength = 0;
        currentImage = imageToShow;
        Debug.Log("CurrentImage: "+ currentImage);
        switch(currentImage){
            case 1: // 2) Chapter name and 1st Image
                ShowPage(2); // shows the correct pagetype
                SetChapterTitle(chapterName_text); // sets the title of the chapter
                SetChapterUI(right_chapter_2, true); // grabs imageToShow and places it on the chapterEntry photograph 
                soundManager.PlayOneShot("PageFlip");
                clipLength = voiceAssistant.PlayVoiceLine("cap"+currentChapter+"img"+currentImage); // i.e. cap5img3
                break;
            case 2: // 3) Two images (first one)
                ShowPage(3); 
                SetChapterUI(left_chapter_3, true);
                right_chapter_3.gameObject.SetActive(false);
                soundManager.PlayOneShot("PageFlip");
                clipLength = voiceAssistant.PlayVoiceLine("cap"+currentChapter+"img"+currentImage); // i.e. cap5img3
                break;
            case 3: // 3) Two images (first and second)
                ShowPage(3); 
                currentImage -= 1; // [special] scenario where we want to load the first photo 
                SetChapterUI(left_chapter_3, false); 
                currentImage += 1;
                SetChapterUI(right_chapter_3, true); 
                clipLength = voiceAssistant.PlayVoiceLine("cap"+currentChapter+"img"+currentImage); // i.e. cap5img3
                break;
            case 4: 
                ShowPage(3); // 3) Two images (first)
                SetChapterUI(left_chapter_3, true); 
                right_chapter_3.gameObject.SetActive(false);
                soundManager.PlayOneShot("PageFlip");
                clipLength = voiceAssistant.PlayVoiceLine("cap"+currentChapter+"img"+currentImage); // i.e. cap5img3
                break;
            case 5: // Oddchapter only: 3) Two images (first and second)
                ShowPage(3); 
                currentImage -= 1; // [special] scenario where we want to load the first photo 
                SetChapterUI(left_chapter_3, false); 
                currentImage += 1;
                SetChapterUI(right_chapter_3, true); 
                clipLength = voiceAssistant.PlayVoiceLine("cap"+currentChapter+"img"+currentImage); // i.e. cap5img3
                break;
        }
        return clipLength;
    }

    // Sets page object active and closes others
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

    // modifies a chapterVis with the correct photograph and text
    public void SetChapterUI(ChapterEntry vis, bool shouldAnimate){
        if(!vis.gameObject.activeSelf) vis.gameObject.SetActive(true);
        vis.SetPhotograph(currentChapter,currentImage, shouldAnimate);
        vis.SetText(currentChapter,currentImage, shouldAnimate);
    }

    public void BackToMainMenuButton(){
        SceneManager.LoadScene("MainMenu");
    }
    
    public void SkipShowingNarrative(){
        // it's like we showed all the unlocked images - Bye Narrative screen: Return to Exercise!!
        if(SequenceManager.GetCurrentChapter() < 2) SequenceManager.SetCurrentChapter(2);
        if(SequenceManager.sequence != null) SequenceManager.nextExercise();
    }

    ///////////////////////////////////////////////////////////////////// OLD functions ////////////////////////////////////////////////////////////////////
    // fetches the chapter title and the title text
    public void SetChapterTitle(TMP_Text title){
        string titlePath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/Text/Title.txt";

        if (System.IO.File.Exists( titlePath)){ 
            //Read the title directly from the Title.txt
            StreamReader reader = new StreamReader(titlePath);
            title.text = "CAPÍTULO " + currentChapter +"\n" + reader.ReadToEnd();
            reader.Close();
        }
        else{
            Debug.Log("ERROR: " + titlePath + " not found.");
            title.text = "ERRO: " + titlePath + " not found.";
        }
    }

    // NOTE: It will not be used anymore
    // displays the last chapter last image and shows a preview text on the right page and plays a voice line
    private IEnumerator ShowPreview(){
        // set the current chapter to previous
        currentChapter -= 1;

        currentPageType = 5;
        ShowPage(5);
        SetPreviewText(preview_text);
        if(currentChapter %2 == 0){ // total of 4 images
            currentImage = 4;
        }
        else{ // total of 5 images
            currentImage = 5;
        }
        SetChapterUI(preview_image, false);

        // set the current chapter to previous
        currentChapter += 1;

        float clipLength = voiceAssistant.PlayVoiceLine("prvcap"+currentChapter); // i.e. prvcap2
        yield return new WaitForSeconds(clipLength);
        // we showed the preview - Bye Narrative screen: Start Session!!
        if(SequenceManager.sequence != null) SequenceManager.nextExercise();
    }

    // NOTE: it will not be used anymore
    // fetches the preview text and places 
    public void SetPreviewText(TMP_Text prev){
        string previewPath = Application.dataPath + "/Resources/Narrative Materials/Chapter"+currentChapter.ToString() +"/Text/Preview.txt";

        if (System.IO.File.Exists( previewPath)){ 
            //Read the title directly from the Title.txt
            StreamReader reader = new StreamReader(previewPath);
            prev.text = reader.ReadLine();
            reader.Close();
        }
        else{
            Debug.Log("ERROR: " + previewPath + " not found.");
            prev.text = "ERRO: " + previewPath + " not found.";
        }
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
