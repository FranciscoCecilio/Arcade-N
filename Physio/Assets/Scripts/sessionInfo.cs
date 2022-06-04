using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

// TODO kiko12 de _username
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
        _timestampSession = "Session" + DateTime.Now.ToString("yyyyMMddTHHmmss");
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + _username + "/" + _timestampSession);
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
        return "kiko12";
        //return _username;
    }

    public static string getName()
    {   
        return _name;
    }

    public static void loadUser()
    {   // This is called on MainMenu after the login.
        loadInfo();
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
        Debug.Log("LoadedInfo");
        reader.Close();
    }

    public static void DeleteUser(TMP_Text afterDeleteText) 
    {
        string filePath = Application.dataPath + "/Users/" + _username + ".txt";
        string fileMetaFilePath = Application.dataPath + "/Users/" + _username + ".txt.meta";
        string folderpath = Application.dataPath + "/Users/" + _username;
        string folderMetaFilePath = Application.dataPath + "/Users/" + _username + ".meta";
        afterDeleteText.text = "";
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
            try
            {
                File.Delete( filePath );
                if(File.Exists(fileMetaFilePath)){
                    File.Delete(fileMetaFilePath);
                    Debug.Log("Apagou o file metafile");
                }
                afterDeleteText.text =  "Ficheiro de "+ "\"" + _username + "\" encontrado! Apagando..." ;
                Debug.Log( "Ficheiro de "+ "\"" + _username + "\" encontrado! Apagando..." );
            }
            catch(IOException ioex){
                afterDeleteText.text = "ERRO ao apagar o ficheiro com nome: \"" + _username + ".txt\"";
                Debug.Log( ioex.Message);
                return;
            }
            //RefreshEditorProjectWindow();
        }

        if ( !Directory.Exists( folderpath ) )
        {
            //guiMessage = "no " + fileName + " file exists";
            afterDeleteText.text += "\nERRO: Nenhuma pasta \"" + _username + "\" encontrada";
            Debug.Log( "ERRO: Nenhuma pasta \"" + _username + "\" encontrada" );
        }
        else
        {
            try
            {
                Directory.Delete(folderpath, true);
                if(File.Exists(folderMetaFilePath)){
                    File.Delete(folderMetaFilePath);
                    Debug.Log("Apagou o folder metafile!!!");
                }
                //FileUtil.DeleteFileOrDirectory(folderpath);
                afterDeleteText.text += "\nPasta "+ "\"" + _username + "\" encontrada! Apagando..." ;
                Debug.Log( "Pasta "+ "\"" + _username + "\" encontrada! Apagando..." );
            }
            catch(IOException ioex){
                afterDeleteText.text += "\nERRO ao apagar a pasta com nome: \"" + _username + "\"";
                Debug.Log( "\nERRO ao apagar a pasta com nome: \"" + _username + "\"" );
                Debug.Log( ioex.Message);
            }
            // RefreshEditorProjectWindow();
        }
    }
     
     
    private static void  RefreshEditorProjectWindow() 
    {
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
    
    // This resets the State because we changed Project Settings> Editor > Enter Play Mode > (disable) Domain Reload
    // If Domain Reload was checked all static values would reset BUT sometimes we would have an infinite "Application.Reload" on entering play mode
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        _toView = "";
        _timestampSession = "";
        _username = "";
        _username = "kiko1244";
        _name= "";
        _age= "";
        _gender= "";
        _exerciseId = 1;
        Debug.Log("sessionInfo reset.");
    }
}