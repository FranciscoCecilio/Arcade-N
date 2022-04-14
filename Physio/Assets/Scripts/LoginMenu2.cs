using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenu2 : MonoBehaviour
{ 
    public TMP_Dropdown _namesDropdown;
    private List<string> _usernames = new List<string>(); // the list with usernames that correspond to .txt files, that will help login (ex. tiagorodrigues31)
    private List<string> _names = new List<string>(); // names to be on the dropdown list (ex. Tiago Rodrigues - 123456789)
    private List<TMP_Dropdown.OptionData> dropdownOptions;
    
    private int _selectedNameIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        populateUsersDropdown();
    }

    public void login()
    {
        SessionInfo.setUsername(_usernames[_selectedNameIndex]);
        SessionInfo.createSessionPath();
        // we want to know if the main menu was loaded from login to animate or not
        LastScene._lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void populateUsersDropdown()
    {
        //_usernames.Add("");
        //_names.Add("Escolha o perfil do utente");

        string folderpath = Application.dataPath + "/Users/";
        if (Directory.Exists(folderpath))
        {
            foreach (string file in Directory.GetFiles(folderpath))
            {
                string[] filename = file.Split('.');
                if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
                {
                    filename = filename[0].Split('/');
                    string _name = "";
                    string username = filename[filename.Length - 1];
                    string filepath = Application.dataPath + "/Users/" + username + ".txt";
                    if (File.Exists(filepath))
                    {
                        string line = "";
                        StreamReader reader = new StreamReader(filepath);
                        {
                            line = reader.ReadLine();
                            while (line != null && line != "")
                            {
                                string[] data = line.Split('=');
                                //for(int i=0; i < data.Length; i++) Debug.Log(data[i]);
                                if (data[0] == "Name")
                                {
                                    _name = data[1];
                                    //break;
                                }
                                if (data[0] == "Nr_Utente")
                                {
                                    _name += " - " + data[1];
                                    break;
                                }
                                line = reader.ReadLine();
                            }
                        }
                        reader.Close();
                    }
                    _names.Add(_name);
                    _usernames.Add(username);
                }
            }
        } else
        {
            Directory.CreateDirectory(folderpath);
        }
        _namesDropdown.AddOptions(_names);
        dropdownOptions = _namesDropdown.options;
    }

    public void Dropdown_IndexChanged(int index)
    {
        string namePicked = _namesDropdown.options[index].text;
        for(int i = 0; i < dropdownOptions.Count; i ++){
            if(namePicked.Equals(dropdownOptions[i].text)){
                _selectedNameIndex = i;
            }
        }
    }

    public void createAccount()
    {
        SceneManager.LoadScene("RegisterProfile2");
    }

    public string GetNameByIndex(int index){
        return _names[index];
    }
}
