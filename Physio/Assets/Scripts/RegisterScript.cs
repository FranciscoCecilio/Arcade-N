using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour {

    public GameObject username;
    private string Username;

    public GameObject alreadyTakenError;

    // Use this for initialization
    void Start () {
		
	}
	
    public void register()
    {
        Username = username.GetComponent<InputField>().text;
        if (Username != "")
        {
            if (!System.IO.Directory.Exists(Application.dataPath + "/Users/"))
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/");
            if (!System.IO.File.Exists(Application.dataPath + "/Users/" + Username + ".txt"))
            {
                System.IO.File.Create(Application.dataPath + "/Users/" + Username + ".txt");
                username.GetComponent<InputField>().text = ""; //clear
                alreadyTakenError.SetActive(false);
                SessionInfo.setUsername(Username);
                SceneManager.LoadScene("RegisterProfile");
            } else
            {
                alreadyTakenError.SetActive(true);
            }
        } else
        {
            Debug.LogWarning("Username field Empty");
        }
        
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Username = username.GetComponent<InputField>().text;
            if (Username != "") register();
        }
	}
}
