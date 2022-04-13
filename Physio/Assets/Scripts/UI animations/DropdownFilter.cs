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
        //dropdown.Hide();
        dropdown.gameObject.SetActive(false);
        dropdown.options = dropdownOptions.FindAll( option => option.text.IndexOf( input ) >= 0 );
        //dropdown.RefreshShownValue();
        dropdown.gameObject.SetActive(true);
        dropdown.Show();

    }

    // making the inputfield always active to write
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            dropdown.Hide();
        }
    }

    public void ShowDropDown(){
        if(valueHasBeenSet){
            // meter o value atual no placeholder
            inputField.placeholder.GetComponent<TMP_Text>().text = inputField.text;
            inputField.text = "";
            valueHasBeenSet = false;
        }
        dropdown.Show();
    }

    bool valueHasBeenSet = false;

    public void SelectOption(){
        Debug.Log(dropdown.value);
        if(dropdown.value != 0){
            startButton.interactable = true;
        }
        else{
            startButton.interactable = false;
        }
        inputField.text = dropdown.options[dropdown.value].text;
        valueHasBeenSet = true;
        dropdown.Hide();
    }

    
    // quando se escolhe um value, eu quero escrevê-lo no input. (DONE)
    // se eu quiser mudar o value, clico no input para abrir o dropdown.(DONE)
    // quero meter o value atual no placeholder do input e apagar o texto (DONE)
    // e também ver a lista cheia (DONE)
    // 
 }
