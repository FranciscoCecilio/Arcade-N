using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TabBetweenInputFields : MonoBehaviour
{
    public TMP_InputField nome;
    public TMP_InputField idade;
    public Toggle masculino;
    public Toggle feminino;
    public TMP_InputField nr_utente;
    public Button voltar;
    public Button guardar;
    public Color selectedColor;

    public int fieldSelected;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)){
            fieldSelected --;
            if(fieldSelected < 0){
                fieldSelected = 6;
            }
            SelectInputField();

        } else if(Input.GetKeyDown(KeyCode.Tab)){
            fieldSelected ++;
            if(fieldSelected > 6){
                fieldSelected = 0;
            }
            SelectInputField();
        }
    }

    void SelectInputField(){
        switch(fieldSelected){
            case 0: nome.Select();
                nome.ActivateInputField();
                //nome.selectionColor = selectedColor;
                break;
            case 1: idade.Select();
                idade.ActivateInputField();
                //idade.selectionColor = selectedColor;
                break;
            case 2: masculino.Select();
            ColorBlock cb = masculino.colors;
            cb.selectedColor = selectedColor;
            masculino.colors = cb;
                break;
            case 3: feminino.Select();
            ColorBlock cb1 = feminino.colors;
            cb1.selectedColor = selectedColor;
            feminino.colors = cb1;
                break;
            case 4: nr_utente.Select();
                nr_utente.ActivateInputField();
                //nr_utente.selectionColor = selectedColor;
                break;
            case 5: voltar.Select();
                ColorBlock cb2 = voltar.colors;
                cb2.selectedColor = selectedColor;
                voltar.colors = cb2;
                break;
            case 6: guardar.Select();
                ColorBlock cb3 = guardar.colors;
                cb3.selectedColor = selectedColor;
                guardar.colors = cb3;
                break;
        }
    }

    public void NomeSelected() => fieldSelected = 0;
    public void IdadeSelected() => fieldSelected = 1;
    public void MasculinoSelected() => fieldSelected = 2;
    public void FemininoSelected() => fieldSelected = 3;
    public void NrUtenteSelected() => fieldSelected = 4;
    public void VoltarSelected() => fieldSelected = 5;

    /* using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TabBetweenInputFields : MonoBehaviour
{
    public InputField nome;
    public InputField idade;
    public Toggle masculino;
    public Toggle feminino;
    public Button voltar;
    public Button guardar;

    public int fieldSelected;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)){
            fieldSelected --;
            if(fieldSelected < 0){
                fieldSelected = 5;
                SelectInputField();
            }

        } else if(Input.GetKeyDown(KeyCode.Tab)){
            fieldSelected ++;
            if(fieldSelected > 5){
                fieldSelected = 0;
                SelectInputField();
            }
        }
    }

    void SelectInputField(){
        switch(fieldSelected){
            case 0: nome.ActivateInputField();
                break;
            case 1: idade.ActivateInputField();
                break;
            case 2: masculino.Select();
                break;
            case 3: feminino.Select();
                break;
            case 4: voltar.Select();
                break;
            case 5: guardar.Select();
                break;
        }
    }

    // from outside the program, if we select an inputfield we have to update the variable
    public void InputFieldSelected(int index){
        fieldSelected = index;
    }
}
*/
}
