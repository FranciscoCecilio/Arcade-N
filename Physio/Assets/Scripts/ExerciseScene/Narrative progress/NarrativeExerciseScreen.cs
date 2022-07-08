using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
 

public class NarrativeExerciseScreen : MonoBehaviour
{
    [Header("For Debugging")]
    public List<Sprite> loadedChapterImages = new List<Sprite>(); // loaded on start from resources folder
    
    [HideInInspector]
    public BarCheckpoints chosenBarCheckpoint;
    [Header("Bar Checkpoints")]
    public BarCheckpoints oddChapterCheckpoints; // has lastchapter_preview + 5 images TODO make class
    public BarCheckpoints evenChapterCheckpoints; // has lastchapter_preview + 4 images 

    [Header("Previous images")]
    public Image previousChapterImage;
    [Header("Final images")]
    public TMP_Text nextChapterText;
    public Image finalImage;
    public Material blur; // max size = 6 for the max blurr ; this blur is applied to this chapter last image
    Sprite previousChapterImg;

    [Header("Slider")]
    public Slider progressBar;
    public TMP_Text handleText;
    

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if(SequenceManager.GetCurrentChapter() %2 == 0){
            //"Number is even"
            evenChapterCheckpoints.gameObject.SetActive(true);
            oddChapterCheckpoints.gameObject.SetActive(false);
            chosenBarCheckpoint = evenChapterCheckpoints;
        }else{
            //"Number is odd"
            oddChapterCheckpoints.gameObject.SetActive(true);
            evenChapterCheckpoints.gameObject.SetActive(false);
            chosenBarCheckpoint = oddChapterCheckpoints;
        }
        
        // load images of current chapter from the Resources folder
        LoadChapterImages();
        // Hide all the images 
        chosenBarCheckpoint.HidePhotographs();
        // place the correct images into the chosenBarCheckpoints
        chosenBarCheckpoint.gameObject.SetActive(true);
        chosenBarCheckpoint.SetImages(loadedChapterImages);
        chosenBarCheckpoint.UnlockCheckpoints();
        
        // Place Previous and Final chapter images
        previousChapterImage.sprite = previousChapterImg;
        finalImage.sprite = loadedChapterImages[loadedChapterImages.Count-1];
        nextChapterText.text = "CAPÍTULO " + SequenceManager.GetCurrentChapter();

        // blurrs the last image according the current session_progress
        CalculateBlurSize();

        // update the slider (value: 0 to 1)
        progressBar.value = SequenceManager.GetSessionProgressionPerc();

        // update the number
        handleText.text = Mathf.RoundToInt(SequenceManager.GetSessionProgressionPerc() * 100).ToString() + "%";
    }

    // fetch the images from resource folder
    public void LoadChapterImages(){
        // Load current chapter images
        int chapterNum = SequenceManager.GetCurrentChapter();
        string chapterFolder = Application.dataPath + "/Resources/Narrative Materials/Chapter"+chapterNum.ToString();
        if(!Directory.Exists(chapterFolder)){
            Debug.LogError("ERROR: Chapter folder not found: " + chapterFolder);
            return;
        }
        string[] files = Directory.GetFiles(chapterFolder);
        int imageNum = 0;
        // iterate the chapter folder looking for images to load
        for(int i = 0; i < files.Length; i ++){
            // make sure its an image
            string[] filename = files[i].Split('.');
            if(filename.Length <= 2 && (filename[1].Equals("png") || filename[1].Equals("jpg") || filename[1].Equals("jpeg"))){
                // load image and store on the list
                imageNum++;
                Sprite sprite = Resources.Load<Sprite>("Narrative Materials/Chapter" + chapterNum.ToString() + "/" + imageNum.ToString());
                //Debug.Log("passou aqui: " + filename[0]);
                loadedChapterImages.Add(sprite);
            }
        }

        // Load previous chapter image
        chapterNum -= 1;
        chapterFolder = Application.dataPath + "/Resources/Narrative Materials/Chapter" + chapterNum.ToString();
        if(!Directory.Exists(chapterFolder)){
            return;
        }
        // we find the last image on the chapter folder
        string[] lastfiles = Directory.GetFiles(chapterFolder);
        for(int i = lastfiles.Length - 1; i > 0; i --){
            // make sure its an image
            string[] lastfilename = lastfiles[i].Split('.');
            if(lastfilename[1].Equals("png") || lastfilename[1].Equals("jpg") || lastfilename[1].Equals("jpeg")){
                // load image and store on the list
                if(chapterNum%2 == 0){
                    // chatpter is even and has 4 Images to unlock
                    imageNum = 4;
                }
                else{
                    // chatpter is odd and has 5 Images to unlock
                    imageNum = 5;
                }
                Sprite sprite = Resources.Load<Sprite>("Narrative Materials/Chapter" + chapterNum.ToString() + "/" + imageNum.ToString());
                previousChapterImg = sprite;
                break;
            }
        }
    }

    // this is called in the ExerciseManager whenever a repetition is completed.
    // here we set the enconding  
    public void IncNarrativePerc(){
        // updates sequenceManager
        SequenceManager.IncrementSessionProgression();
        // updates bar value
        progressBar.value = SequenceManager.GetSessionProgressionPerc();
        // updates value text
        handleText.text = ((int)(SequenceManager.GetSessionProgressionPerc() * 100) ).ToString() + "%";
        
        if(SequenceManager.hasImagesToUnlock){
            // updates an image to look like (Hey its unlocked and you will see it when you finish that serie!)
            // Play some ongoing animation
            chosenBarCheckpoint.AnimateCheckpoints();
            // shake the whole vis horizontally
            LeanTween.moveLocalX(gameObject, 10f, 0.2f).setLoopType(LeanTweenType.pingPong).setRepeat(2);
        }

        // progressively clears the blurry of the next chapter image
        CalculateBlurSize();
    }

    // blurrs the last image according the current session_progress
    void CalculateBlurSize(){
        float blursize;
        if(SequenceManager.GetSessionProgressionPerc() == 0){
            blursize = 6.0f;
        }
        else{
    	    blursize = 6.0f * (1.0f - 1.0f*SequenceManager.GetSessionProgressionPerc()); 
        }
        blur.SetFloat("_Size", blursize);
    }
}
