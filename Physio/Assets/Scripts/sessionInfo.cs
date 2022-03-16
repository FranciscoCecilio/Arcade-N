﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

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
}