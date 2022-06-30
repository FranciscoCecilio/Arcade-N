using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class NewUserProfileScript : MonoBehaviour {

    private bool _fieldsEmpty;

    public GameObject Name;
    public GameObject EmptyNameMessage;
    private string _name;

    public GameObject Age;
    public GameObject EmptyAgeMessage;
    private string _age;

    //Toggle gender gameObjects
    public Toggle IsMale;
    public Toggle IsFemale;
    public Toggle IsOther;
    private string _gender;

    private void setName()
    {
        string tempName = Name.GetComponent<InputField>().text;
        if (tempName != "")
        {
            _name = tempName;
            EmptyNameMessage.SetActive(false);
        }
        else
        {
            EmptyNameMessage.SetActive(true);
            _fieldsEmpty = true;
        }
    }

    private void setAge()
    {
        string tempAge = Age.GetComponent<InputField>().text;
        if (tempAge != "")
        {
            _age = tempAge;
            EmptyAgeMessage.SetActive(false);
        }
        else
        {
            EmptyAgeMessage.SetActive(true);
            _fieldsEmpty = true;
        }
    }

    private void setGender()
    {
        if (IsMale.isOn) _gender = "masculino";
        else if (IsFemale.isOn) _gender = "feminino";
        else if (IsOther.isOn) _gender = "Other";
    }

    private void saveInfo()
    {
        string username = SessionInfo.getUsername();
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Users/" + username + ".txt");

        writer.WriteLine("Username=" + username);
        writer.WriteLine("Name=" + _name);
        writer.WriteLine("Age=" + _age);
        writer.WriteLine("Gender=" + _gender);

        writer.Close();
    }

    public void onSubmit()
    {
        _fieldsEmpty = false;
        setName();
        setAge();
        setGender();
        if (!_fieldsEmpty)
        {
            saveInfo();
            SessionInfo.createSessionPath();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
