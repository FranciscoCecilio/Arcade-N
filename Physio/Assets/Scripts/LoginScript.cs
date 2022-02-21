using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

    public GameObject username;
    private string Username;

    public GameObject notFoundError;

    // Use this for initialization
    void Start () {
	}

    public void login()
    {
        Username = username.GetComponent<InputField>().text;
        if (Username != "") //username preenchido
        {
            if (!System.IO.Directory.Exists(Application.dataPath + "/Users/")) //nao ha utilizadores
                notFoundError.SetActive(true);
            else
            {
                if (!System.IO.File.Exists(Application.dataPath + "/Users/" + Username + ".txt")) //nao existe utilizador
                {
                    notFoundError.SetActive(true);
                    Debug.LogWarning("Não há o utilizador registado.");
                }
                else //utilizador valido
                {
                    SessionInfo.setUsername(Username);
                    SessionInfo.createSessionPath();
                    notFoundError.SetActive(false);
                    SceneManager.LoadScene("MainMenu");
                }
            }
            username.GetComponent<InputField>().text = ""; //clear
        }
        else
        {
            Debug.LogWarning("Username field Empty");
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Username = username.GetComponent<InputField>().text;
            if (Username != "") login();
        }
    }
}
