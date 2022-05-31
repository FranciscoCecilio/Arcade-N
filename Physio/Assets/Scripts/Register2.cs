using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Register2 : MonoBehaviour
{
    private bool _fieldsEmpty;

    public GameObject Name; 
    public GameObject EmptyNameMessage;
    public GameObject EmptyNameBox;
    private string _name;

    public GameObject Age;
    public GameObject EmptyAgeMessage;
    public GameObject EmptyAgeBox;
    private string _age; 

    public GameObject Nr_Saude;
    private string _nrSaude;

    //Toggle gender gameObjects
    public Toggle IsMale;
    public Toggle IsFemale;
    private string _gender;

    private void setName()
    {
        string tempName = Name.GetComponent<TMP_InputField>().text;
        if (tempName != "")
        {
            _name = tempName;
            EmptyNameMessage.SetActive(false);
            EmptyNameBox.SetActive(false);
        }
        else
        {
            EmptyNameMessage.SetActive(true);
            EmptyNameBox.SetActive(true);
            _fieldsEmpty = true;
        }
    }

    private void setAge()
    {
        string tempAge = Age.GetComponent<TMP_InputField>().text;
        if (tempAge != "")
        {
            _age = tempAge;
            EmptyAgeMessage.SetActive(false);
            EmptyAgeBox.SetActive(false);
        }
        else
        {
            EmptyAgeMessage.SetActive(true);
            EmptyAgeBox.SetActive(true);
            _fieldsEmpty = true;
        }
    }
    
    private void setNrSaude()
    {
        _nrSaude = Nr_Saude.GetComponent<TMP_InputField>().text;
    }

    private void setGender()
    {
        if (IsMale.isOn) _gender = "Male";
        else if (IsFemale.isOn) _gender = "Female";
    }

    private void saveInfo()
    {
        string username = createUsername(_name);

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Users/" + username + ".txt");

        writer.WriteLine("Username=" + username);
        writer.WriteLine("Name=" + _name);
        writer.WriteLine("Age=" + _age);
        writer.WriteLine("Gender=" + _gender);
        if(_nrSaude != "") writer.WriteLine("Nr_Utente=" + _nrSaude);

        writer.Close();

        SessionInfo.setUsername(username);
    }

    public static string createUsername(string str)
    {
        StringBuilder sb = new StringBuilder();
        BooleanGenerator boolGen = new BooleanGenerator();
        while (sb.Length == 0)
        {
            foreach (char c in str)
            {
                if (((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_'))
                {
                    sb.Append(c);
                }
            }
        }
        int uID = UnityEngine.Random.Range(1,100);
        sb.Append(uID.ToString());
        
        return sb.ToString();
    }

    public void onSubmit()
    {
        _fieldsEmpty = false;
        setName();
        setAge();
        setGender();
        setNrSaude();
        if (!_fieldsEmpty)
        {
            saveInfo();
            SessionInfo.createSessionPath();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void cancel()
    {
        SceneManager.LoadScene("LoginMenu2");
    }
}

public class BooleanGenerator
{
    System.Random rnd;

    public BooleanGenerator()
    {
        rnd = new System.Random();
    }

    public bool NextBoolean()
    {
        return Convert.ToBoolean(rnd.Next(0, 2));
    }
}