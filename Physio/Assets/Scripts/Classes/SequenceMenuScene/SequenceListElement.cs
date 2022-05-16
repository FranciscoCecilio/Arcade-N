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
    public TMP_Text nRepsField;
    public TMP_Text nSeriesField;
    public TMP_Text restTimerField;
    
    // Sequence -----------------------
    private Sequence _sequence;
    //public Text buttonName; // nao será preciso

    public void SetSequence(Sequence sequence)
    {
        _sequence = sequence;
        //buttonName.text = _sequence.getName();
    }

    public Sequence GetSequence(){
        if(_sequence == null){
            Debug.Log("Error: Tried to Get a null sequence.");
            return null;
        }
        else 
            return _sequence;
    }

    public void SetSequenceParameters(int exTypeIndex, int armIndex, int nSeries, int nReps, int restTimer){
        // place the info in the placeholders
        typeIndex = exTypeIndex;
        ChangeExerciseType();
        armText.text = (armIndex == 0) ? "Esquerdo" : "Direito";
        nSeriesField.text = nSeries.ToString();
        nRepsField.text = nReps.ToString();
        restTimerField.text = restTimerField.ToString();

        // save info on the sequence itself
        _sequence.setSeries(nSeries);
        // por cada serie juntar um exercicio com determinadas repetiçoes
        for(int i = 0; i < nSeries; i++){
            // exe=2=Left/Right=Exercise2Scene=right=10=60=60
            // Exercise(int id, string name, string scenePath, string arm, int nreps, int duration, int restTime)
            _sequence.addExercise(new Exercise(exTypeIndex,  exerciseTypeText.text, "Exercise"+exTypeIndex.ToString()+"Scene" , armIndex, nReps, 0, 0));
        }

        // save to a file
        _sequence.toFile();
    }

    public void ChangeExerciseType(){
        typeIndex++;
        if(typeIndex > 2){
            typeIndex = 0;
        }
        switch (typeIndex){
            case 0: 
                exerciseTypeText.text = "Grelha";
                break;
            case 1: 
                exerciseTypeText.text = "Horizontal";
                break;
            case 2: 
                exerciseTypeText.text = "Vertical";
                break;        
        }
    }

    // Envia pedido de delete ao SequenceListControl.cs
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

