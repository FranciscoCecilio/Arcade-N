using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 

public class NarrativeExerciseScreen : MonoBehaviour
{
    [SerializeField] Sprite[] lastChapterImgs; // last images from each chapter
    [SerializeField] Sprite[] nextChapterImgs; // first images from each chapter
    public Image lastPlaceHolder;
    public Image nextPlaceHolder;
    public TMP_Text lastChapterText;
    public TMP_Text nextChapterText;
    public int level;
    public Slider progressBar;
    public Material blur; // max size = 6 for the max blurr
    public TMP_Text handleText;


    void Start()
    {
        level = SessionInfo.getXP() / 100; // (13 / 5) output: 2 therefore (130 / 100) output: 1
        // use the correct images
        lastPlaceHolder.sprite = lastChapterImgs[level];
        nextPlaceHolder.sprite = nextChapterImgs[level];
        // change the name (or number) of the chapters
        lastChapterText.text = "CAPÍTULO " + level.ToString();
        lastChapterText.text = "CAPÍTULO " + (level + 1).ToString();
        // blurry the next chapter image according the current session_progress
        CalculateBlurSize();
        // update the slider (value: 0 to 1)
        progressBar.value = SequenceManager.GetSessionProgressionPerc();
        // update the number
        handleText.text = ((int) SequenceManager.GetSessionProgressionPerc() * 100).ToString() + "%";
    }

    // this is called in the ExerciseManager whenever a repetition is completed 
    public void IncNarrativePerc(){
        // updates sequenceManager
        SequenceManager.IncrementSessionProgression();
        // updates bar
        progressBar.value = SequenceManager.GetSessionProgressionPerc();
        // updates number
        handleText.text = ((int) SequenceManager.GetSessionProgressionPerc() * 100).ToString() + "%";
        // progressively clears the blurry of the next chapter image
        CalculateBlurSize();
    }

    // blurry the next chapter image according the current session_progress
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
