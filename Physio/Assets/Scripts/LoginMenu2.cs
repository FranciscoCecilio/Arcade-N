using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenu2 : MonoBehaviour
{ 
    public Dropdown _namesDropdown;
    private List<string> _usernames = new List<string>();
    private List<string> _names = new List<string>();
    private int _selectedNameIndex = 0;

    // Start is called before the first frame update
    void Start()
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
        _usernames.Add("");
        _names.Add("Select a profile name");

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
                                if (data[0] == "Name")
                                {
                                    _name = data[1];
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
    }

    public void Dropdown_IndexChanged(int index)
    {
        _selectedNameIndex = index;
    }

    public void createAccount()
    {
        SceneManager.LoadScene("RegisterProfile2");
    }
}
