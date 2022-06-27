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
    public BarCheckpointAnimation[] barCheckpoints;

    [Header("For Debugging")]
    public List<Sprite> sprites;

    // Hides all photographs
    public void HidePhotographs(){
        for(int i = 0; i < barCheckpoints.Length; i++){
            barCheckpoints[i].chapterImage.gameObject.SetActive(false);
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
                barCheckpoints[i].chapterImage.gameObject.SetActive(true);
                barCheckpoints[i].chapterImage.sprite = sprites[i];
                barCheckpoints[i].setGreen();
            }
            else if(SequenceManager.unlockedChaptersEncoding[i] == 1){
                // Unlock it
                barCheckpoints[i].chapterImage.gameObject.SetActive(true);
                barCheckpoints[i].chapterImage.sprite = sprites[i];
                barCheckpoints[i].setGreen();
                // and Set to code -1: unlocked 
                SequenceManager.unlockedChaptersEncoding[i] = -1;
            }
        }
    }

    // Animate Checkpoints to be unlocked
    public void AnimateCheckpoints(){
        // ATENTION: we don't want to animate if is the Final image of the chapter, hence the -1
        for(int i = 0; i< SequenceManager.unlockedChaptersEncoding.Count - 1; i++){
            if(SequenceManager.unlockedChaptersEncoding[i] == 1){
                // Animate it
                barCheckpoints[i].StartAnimationLoop();
            }
        }
    }
}
