using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SequenceListElement : MonoBehaviour
{
    // UI fields -----------------------
    [SerializeField] int typeIndex;
    public TMP_Text exerciseTypeText;
    public TMP_Text armText;
    public TMP_InputField nRepsField;
    public TMP_InputField nSeriesField;
    public TMP_InputField restTimerField;
    
    // Sequence -----------------------
    private Sequence _sequence;
    //public Text buttonName; // nao será preciso

    
    public void SetSequence(Sequence sequence)
    {
        _sequence = sequence;
        //buttonName.text = _sequence.getName();
    }

       
    // --------------------------------- Parameters Buttons ---------------------------------------------------
    public void ChangeExerciseType(){
        typeIndex++;
        if(typeIndex > 2){
            typeIndex = 0;
        }
        switch (typeIndex){
            case 0: 
                exerciseTypeText.text = "Grid";
                break;
            case 1: 
                exerciseTypeText.text = "L/R";
                break;
            case 2: 
                exerciseTypeText.text = "U/D";
                break;        
        }
    }

    public void ChangeArm(){
        if(armText.text.Equals("Esquerdo")) armText.text = "Direito";
        else if(armText.text.Equals("Direito")) armText.text = "Esquerdo";
        else armText.text = "Erro";
    }

    public void nSeriesUp()
    {
        if (nSeriesField != null)
        {
            if (!nSeriesField.text.Equals("")) nSeriesField.text = (int.Parse(nSeriesField.text) + 1).ToString();
            else nSeriesField.text = "2";
        }
    }

    public void nSeriesDown()
    {
        if (nSeriesField != null)
        {
            if (!nSeriesField.text.Equals("")) {
                if((int.Parse(nSeriesField.text) <= 1)) nSeriesField.text = "1";
                else nSeriesField.text = (int.Parse(nSeriesField.text) - 1).ToString();
            }
            else nSeriesField.text = "1";
        }
    }
    
    
    public void nRepsUp()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")) nRepsField.text = (int.Parse(nRepsField.text) + 1).ToString();
            else nRepsField.text = "2";
        }
    }

    public void nRepsDown()
    {
        if (nRepsField != null)
        {
            if (!nRepsField.text.Equals("")){
                if((int.Parse(nRepsField.text) <= 0)) nRepsField.text = "1";
                else nRepsField.text = (int.Parse(nRepsField.text) - 1).ToString();
            }
            else nRepsField.text = "1";
        }
    }

    public void restTimerUp()
    {
        if (restTimerField != null)
        {
            if (!restTimerField.text.Equals("")) restTimerField.text = (int.Parse(restTimerField.text) + 10).ToString();
            else restTimerField.text = "10";
        }
    }

    public void restTimerDown()
    {
        if (restTimerField != null)
        {
            if (!restTimerField.text.Equals("")){
                if((int.Parse(restTimerField.text) <= 0)) restTimerField.text = "0";
                else restTimerField.text = (int.Parse(restTimerField.text) - 10).ToString();
            }
            else restTimerField.text = "0";
        }
    }
    // --------------------------------- Delete Button ---------------------------------------------------
    
    // Olhar para o SequenceListButton e em vez de apagar exercicios, apagar o ficheiro TS?
    public void DeleteSequenceButton(){
        //Procura o script que controla os botoes da lista
        GameObject sequencesList = GameObject.Find("Sequences");
        if(sequencesList == null) Debug.Log("Error trying to find Sequences object in scene");
        SequenceListControl script = (SequenceListControl) sequencesList.GetComponent(typeof(SequenceListControl));
        // Seleciona esta sequencia como ativa e abre o delete dialogue
        script.ActiveSequence(_sequence);
        script.OpenDeleteDialogue();
    }
}

