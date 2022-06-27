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

    private static string _username; // txt file name 
    private static string _name; // nome *bonito* dentro do .txt
    private static string _age;
    private static string _gender;
    private static string _nrSaude; // we don't use it outside login

    private static int _exerciseId = 1;

    // DEFINITIONS
    private static int _XP = 0; // (0 to 500 in multiples of 100)
    private static bool _isMusicOn = true;
    private static bool _isVoiceOn = true;

    public static void setUsername(string username)
    {
        _username = username;
    }

    public static void createSessionPath()
    {
        _timestampSession = "Session" + DateTime.Now.ToString("yyyyMMddTHHmmss");
        if(_username.Equals(string.Empty)) _username = "kiko12";
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

     // This is called on MainMenu after the login
    public static void loadUser()
    { 
        string line = "";
        Debug.Log("username: " + _username);
        if(_username == string.Empty) _username = "kiko12"; // For testing
        StreamReader reader = new StreamReader(Application.dataPath + "/Users/" + _username + ".txt");
        {
            line = reader.ReadLine();
            while (line != null)
            {
                string[] data = line.Split('=');
                if (data[0] == "Name") _name = data[1];
                else if (data[0] == "Age") _age = data[1];
                else if (data[0] == "Gender") _gender = data[1];
                else if (data[0] == "Nr_Utente") _nrSaude = data[1];
                else if (data[0] == "XP") _XP = int.Parse(data[1]);
                else if (data[0] == "MusicOn") {
                    _isMusicOn = (data[1] == "True");
                }
                else if (data[0] == "VoicOn") _isVoiceOn = (data[1] == "True");
                line = reader.ReadLine();
            }
        }
        Debug.Log("LoadedInfo - musicOn:" + _isMusicOn);
        reader.Close();
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

    public static int getXP()
    {
        return _XP;
    }

    public static void setXP(int XP){
        _XP = XP;
    }

    public static string toView()
    {
        return _toView;
    }

    public static void setMusic(bool intention){
        _isMusicOn = intention;
    }

    public static bool isMusicOn()
    {
        return _isMusicOn;
    }

    public static bool isVoiceOn()
    {
        return _isVoiceOn;
    }

    public static void setVoice(bool intention){
        _isVoiceOn = intention;
    }

    // save session file in the end of the session 
    // (called when last exercise from last sequence OR Quitting during the exercises)
    public static void saveSession(){
        saveUserProgress();
    }

    // This is called when we end session or QUIT
    public static void saveUserProgress(){
        string userfile = Application.dataPath + "/Users/" + _username + ".txt";

        if(File.Exists(userfile)) File.Delete(userfile);

        // write to the user file the settings and XP
        StreamWriter writer = new StreamWriter(userfile);
        // old stuff (always the same)
        writer.WriteLine("Username=" + _username);
        writer.WriteLine("Name=" + _name);
        writer.WriteLine("Age=" + _age);
        writer.WriteLine("Gender=" + _gender);
        if(_nrSaude != "") writer.WriteLine("Nr_Utente=" + _nrSaude);
        // new stuff
        writer.WriteLine("XP=" + _XP);
        writer.WriteLine("MusicOn=" + _isMusicOn);
        writer.WriteLine("VoiceOn=" + _isVoiceOn);

        writer.Close();
    } 

    public static void DeleteUser(TMP_Text afterDeleteText) 
    {
        string filePath = Application.dataPath + "/Users/" + _username + ".txt";
        string fileMetaFilePath = Application.dataPath + "/Users/" + _username + ".txt.meta";
        string folderpath = Application.dataPath + "/Users/" + _username;
        string folderMetaFilePath = Application.dataPath + "/Users/" + _username + ".meta";
        afterDeleteText.text = "";
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
        _username = "";
        _name= "";
        _age= "";
        _gender= "";
        _exerciseId = 1;
        _XP = 0;
        _isMusicOn = true;
        _isVoiceOn = true;
        Debug.Log("sessionInfo reset.");
    }
}