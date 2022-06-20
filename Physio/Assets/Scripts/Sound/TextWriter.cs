using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    private TMP_Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;

    public void AddWriter(TMP_Text uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters){
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;
    }

    public void Pila(){
        
    }
    // Update is called once per frame
    void Update()
    {
        if(uiText != null){
            timer -= Time.deltaTime;
            while(timer <= 0f){
                // Display next character
                timer += timePerCharacter;
                characterIndex ++;
                string text = textToWrite.Substring(0, characterIndex);
                uiText.text = text;
                if(invisibleCharacters){
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                if(characterIndex >= textToWrite.Length){
                    // Entire string displayed
                    uiText = null;
                    return;
                }
            }
        }        
    }
}
