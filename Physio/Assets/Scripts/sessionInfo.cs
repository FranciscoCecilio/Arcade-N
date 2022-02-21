using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class SessionInfo
{
    private static string _toView = "";
    private static string _timestampSession = "";

    //TODO: APAGAR DEFAULTS
    private static string _username;
    private static string _name;
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
    {
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
}