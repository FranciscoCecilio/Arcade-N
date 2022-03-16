using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public static class SessionInfo
{
    private static string _toView = "";
    private static string _timestampSession = "";

    //TODO: APAGAR DEFAULTS
    private static string _username; // txt file name 
    private static string _name; // nome *bonito* dentro do .txt
    private static string _age;
    private static string _gender;

    private static int _exerciseId = 1;

    public static void setUsername(string username)
    {
        _username = username;
    }

    public static void createSessionPath()
    {
        _timestampSession = DateTime.Now.ToString("yyyyMMddTHHmmss");
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + _username + "/Sessions/" + _timestampSession);
    }

    public static string getSessionPath()
    {
        return _timestampSession;
    }

    public static void setView(string view)
    {
        _toView = view;
    }

    public static string getUsername()
    {
        return _username;
    }

    public static string getName()
    {   // UNCOMMENT THIS LINE TO WORK (COMMENT WHEN DEBUGGING)
        loadInfo();
        return _name;
    }

    public static int getExerciseId()
    {
        return _exerciseId;
    }

    public static string getAge()
    {
        return _age;
    }

    public static string getGender()
    {
        return _gender;
    }

    public static string toView()
    {
        return _toView;
    }

    private static void loadInfo()
    {
        string line = "";
        StreamReader reader = new StreamReader(Application.dataPath + "/Users/" + _username + ".txt");
        {
            line = reader.ReadLine();
            while (line != null)
            {
                string[] data = line.Split('=');
                if (data[0] == "Name") _name = data[1];
                else if (data[0] == "Age") _age = data[1];
                else if (data[0] == "Gender") _gender = data[1];
                line = reader.ReadLine();
            }
        }
    }

    public static void DeleteUser(TMP_Text afterDeleteText) 
    {
        string filePath = Application.dataPath + "/Users/" + _username + ".txt";
        string folderpath = Application.dataPath + "/Users/" + _username;
        
        /*int var = 0;
        foreach (string file in Directory.GetFiles( Application.dataPath + "/Users/"))
            {
                if(var == 4){
                    Debug.Log(file);
                    File.Delete(file);
                    
                }
                var ++;
            }*/

        // check if file exists
        if ( !File.Exists( filePath ) )
        {
            //guiMessage = "no " + fileName + " file exists"; 
            afterDeleteText.text = "ERRO: Nenhum ficheiro \"" + _username + "\" encontrado";
            Debug.Log( "ERRO: Nenhum ficheiro \"" + _username + "\" encontrado" );
        }
        else
        {
            
            //guiMessage = fileName + " file exists, deleting..."; 
            afterDeleteText.text =  "Ficheiro de "+ "\"" + _username + "\" encontrado! Apagando..." ;
            Debug.Log( "Ficheiro de "+ "\"" + _username + "\" encontrado! Apagando..." );
            
            File.Delete( filePath );
            RefreshEditorProjectWindow();
        }

        if ( !Directory.Exists( folderpath ) )
        {
            //guiMessage = "no " + fileName + " file exists";
            afterDeleteText.text += "\nERRO: Nenhuma pasta \"" + _username + "\" encontrada";
            Debug.Log( "ERRO: Nenhuma pasta \"" + _username + "\" encontrada" );
        }
        else
        {
            //guiMessage = fileName + " file exists, deleting..."; 
            afterDeleteText.text += "\nPasta de "+ "\"" + _username + "\" encontrado! Apagando..." ;
            Debug.Log( "Pasta de "+ "\"" + _username + "\" encontrado! Apagando..." );
            
            Directory.Delete(folderpath, true);
            RefreshEditorProjectWindow();
        }
    }
     
     
    private static void  RefreshEditorProjectWindow() 
    {
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
}