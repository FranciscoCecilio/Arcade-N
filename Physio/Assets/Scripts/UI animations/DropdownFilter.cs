using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DropdownFilter : MonoBehaviour
 {
    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_Dropdown dropdown;

    [SerializeField]
    private Button startButton;
    
    private List<TMP_Dropdown.OptionData> dropdownOptions;
    
    private void Start()
    {
        dropdownOptions = dropdown.options;
        startButton.interactable = false;
    }    

    public void FilterDropdown( string input )
    {
        dropdown.Hide();
        //dropdown.gameObject.SetActive(false);
        dropdown.options = dropdownOptions.FindAll( option => option.text.IndexOf( input ) >= 0 );
        dropdown.RefreshShownValue();
        //dropdown.gameObject.SetActive(true);
        dropdown.Show();
        Debug.Log(dropdown.options[0].text);
        // when it becomes empty, for some reason, the input field deselects, so we fix that with this
        if(input.Equals("")){
            inputField.Select();
        }

    }

    float timer = 0;
    // making the inputfield always active to write
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            dropdown.Hide();
        }
        if(Input.GetMouseButtonDown(0)){
            timer = 0.5f;
        }
        timer -= Time.deltaTime;
    }

    public void ShowDropDown(){
        if(valueHasBeenSet){
            // meter o value atual no placeholder
            inputField.placeholder.GetComponent<TMP_Text>().text = inputField.text;
            inputField.text = "";
            valueHasBeenSet = false;
        }
        dropdown.Hide();
        dropdown.RefreshShownValue();
        dropdown.Show();
    }

    bool valueHasBeenSet = false;

    public void SelectOption(){
        Debug.Log("option selected");
        // the 1st option on the dropdown is not a user. The string we compare to is the one in "LoginMenu2.cs"
        if(dropdown.value == 0 && dropdown.options[dropdown.value].text.Equals("Escolha o perfil do utente")){
            Debug.Log("Bloqueou butao: " + dropdown.options[0]);
            startButton.interactable = false;
        }
        else{
            startButton.interactable = true;
        }
        inputField.text = dropdown.options[dropdown.value].text;
        valueHasBeenSet = true;
        dropdown.Hide();
    }

    public void OnDeselect(){
        Debug.Log("deselected. " );
        /*if(timer < 0){
            Debug.Log("reselected. " );
            inputField.Select();
            inputField.ActivateInputField();
        }*/
    }
    // quando se escolhe um value, eu quero escrevê-lo no input. (DONE)
    // se eu quiser mudar o value, clico no input para abrir o dropdown.(DONE)
    // quero meter o value atual no placeholder do input e apagar o texto (DONE)
    // e também ver a lista cheia (DONE)
    // 
 }
