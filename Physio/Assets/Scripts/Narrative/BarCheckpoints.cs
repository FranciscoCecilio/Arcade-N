using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

// this object contains the checkpoints
public class BarCheckpoints : MonoBehaviour
{
    public Image previewImage;
    public Image lastImage;

    // capitulos pares: Lenght of 3; capitulos ímpares: Lenght of 4
    public Image[] chapterImages; 
    public Image[] numberBackgrounds;
    public TMP_Text[] numberTexts;

    [Header("For Debugging")]
    public List<Sprite> sprites;

    // Hides all photographs
    public void HidePhotographs(){
        for(int i = 0; i < chapterImages.Length; i++){
            chapterImages[i].gameObject.SetActive(false);
        }
    }

    // Load the images from Narrative Exercise Screen
    public void SetImages(List<Sprite> images){
        sprites = images;
    }

    // show the Bar checkpoints depending on our progression
    // Before this function, we had a transiction to the Narrative screen showing the pictures
    public void UnlockCheckpoints(){
        for(int i = 0; i< SequenceManager.unlockedChaptersEncoding.Count; i++){
            if(SequenceManager.unlockedChaptersEncoding[i] == -1){
                // Unlock it
                chapterImages[i].gameObject.SetActive(true);
                chapterImages[i].sprite = sprites[i];
            }
            else if(SequenceManager.unlockedChaptersEncoding[i] == 1){
                // Unlock it
                chapterImages[i].gameObject.SetActive(true);
                chapterImages[i].sprite = sprites[i];
                // TODO make some animations / color to the text and background

                // and Set to code -1: unlocked 
                SequenceManager.unlockedChaptersEncoding[i] = -1;
            }
        }
    }

    // Animate Checkpoints to be unlocked
    public void AnimateCheckpoints(){
        for(int i = 0; i< SequenceManager.unlockedChaptersEncoding.Count; i++){
            if(SequenceManager.unlockedChaptersEncoding[i] == 1){
                // Animate it
                chapterImages[i].gameObject.SetActive(true);
                chapterImages[i].sprite = sprites[i];
                // TODO make some animations / color to the text and background
                
                // and Set to code -1: unlocked 
                SequenceManager.unlockedChaptersEncoding[i] = -1;
            }
        }
    }
}
