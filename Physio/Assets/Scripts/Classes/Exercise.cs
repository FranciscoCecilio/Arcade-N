using UnityEngine;
using System;
using System.IO;

// TODO Cecilio: 
//
// Uma Sequence são um conjunto de (X = _series) Exercícios, em que cada exercício é igual (tem o mesmo _id e _nReps).
// Ou seja: uma Sequence não é um conjunto de exercícios variados (de _ids diferentes), mas sim X séries do mesmo exercício.
// Por exemplo: 
// Sequence1: consiste em 3 séries de 1 repetição cada do exercício em Grelha
// Sequence2: consiste em 2 séries de 5 repetiçoes cada do exercício Horizontal
// Sequence3: consiste em 3 séries de 1 repetição cada do exercício Vertical

// Portanto, uma Série recebe determiando exercício, nr. de séries, nr. de repetições (mesmo valor para cada série), tempo descanso entre séries
public class Exercise
{
    private int _id; // 0 , 1 , 2 = Exercise Type
    private string _name; // Grid, Horizontal, Vertical

    private string _scenePath; // name of the scene
    private bool _loggedToFile = false;

    //Parameters
    bool _leftArm = true; //true if Left Arm selected, false if Right Arm selected

    int _nreps = 10; //number of repetitions
    int _duration = 60; //duration time limit (seconds)
    int _restTime = 60; //rest time (seconds)

    //variables
    bool _isCompleted = false;

    int _tries = 0;
    int _correctReps = 0;

    String _avgTime;
    String _totalTime;

    int _lShoulderComp = 0;
    int _rShoulderComp = 0;
    int _spineComp = 0;
    int _outOfPath = 0;

    private Vector3[] _targetPositions;
    Vector3 _pathPosition;

    //Constructor

    public Exercise()
    {
        /*int id = 1;
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Exercises/")) //Ficheiros na pasta Exercises
        {
            string[] filename = file.Split('.');
            if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
            {
                id++;
            }
        }

        _id = id;*/
    }

    public Exercise(int id, string name, string scenePath)
    {
        _id = id;
        _name = name;
        _scenePath = scenePath;
    }

    public Exercise(int id, string name, string scenePath, int armCode, int nreps, int duration, int restTime)
    {
        _id = id;
        _name = name;
        _scenePath = scenePath;
        if (armCode == 0) _leftArm = true;
        else if (armCode == 1) _leftArm = false;
        else Debug.Log("ERROR : armcode is not 0 or 1:" + armCode);
        _nreps = nreps;
        _duration = duration; // will it be used?
        _restTime = restTime; // will it be used?
    }

    //SETTERS

    public void setName(string name)
    {
        _name = name;
    }

    public void setArm(bool isLeft)
    {
        _leftArm = isLeft;
    }

    public void setNReps(int nReps)
    {
        if (nReps > 0) _nreps = nReps;
    }

    public void setDuration(int duration)
    {
        if (duration > 0) _duration = duration;
    }

    public void setRestTime(int duration)
    {
        if (!(duration < 0)) _restTime = duration;
    }

    public void setScenePath(string path)
    {
        _scenePath = path;
    }

    public void setCompleted(bool completed)
    {
        _isCompleted = completed;
        if (_isCompleted && !_loggedToFile) logToFile();
    }

    public void setAvgTime(String avgTime)
    {
        _avgTime = avgTime;
    }

    public void setTotalTime(int timeSeconds)
    {
        int minutes = timeSeconds / 60;
        int seconds = timeSeconds % 60;
        _totalTime = minutes.ToString("00") + ":" + seconds.ToString("00") + " m";
    }

    //GETTERS

    public string getName()
    {
        return _name;
    }

    public int getId()
    {
        return _id;
    }

    public bool isLeftArm()
    {
        return _leftArm;
    }

    public int getNReps()
    {
        return _nreps;
    }

    public int getDuration()
    {
        return _duration;
    }

    public int getRestTime()
    {
        return _restTime;
    }

    public string getScenePath()
    {
        return _scenePath;
    }

    public bool isCompleted()
    {
        return _isCompleted;
    }

    public int getCorrectReps()
    {
        return _correctReps;
    }

    public int getTries()
    {
        return _tries;
    }

    public int getLeftShoulderComp()
    {
        return _lShoulderComp;
    }

    public int getRightShoulderComp()
    {
        return _rShoulderComp;
    }

    public int getSpineComp()
    {
        return _spineComp;
    }

    public int getOutOfPath()
    {
        return _outOfPath;
    }

    //function
    public void incTries()
    {
        _tries++;
    }

    public void incCorrectReps()
    {
        _correctReps++;
    }

    public void incLeftShoulderComp()
    {
        _lShoulderComp++;
    }

    public void incRightShoulderComp()
    {
        _rShoulderComp++;
    }

    public void incSpineComp()
    {
        _spineComp++;
    }

    public void incOutOfPath()
    {
        _outOfPath++;
    }

    public void saveTargetPositions(Vector3[] targetPositions)
    {
        _targetPositions = targetPositions;
    }

    public void savePathPosition(Vector3 position)
    {
        _pathPosition = position;
    }

    public void restart()
    {
        _loggedToFile = false;
        _isCompleted = false;
        _tries = 0;
        _correctReps = 0;
        _avgTime = "";
        _totalTime = "";
        _lShoulderComp = 0;
        _rShoulderComp = 0;
        _spineComp = 0;
        _outOfPath = 0;
    }

    // we want to create a text file inside: Users/<user>/<SessionTS>/<SequenceTS>/
    public void logToFile()
    {
        _loggedToFile = true;

        string _arm;
        if (_leftArm) _arm = "left";
        else _arm = "right";

        //Users/<user>/<SessionTS>/<SequenceTS>/ExerciseTS.txt
        String filename = "Exercise" + DateTime.Now.ToString("yyyyMMddTHHmmss");
        if(!Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/" + SequenceManager.sequence.getTimestamp())){
            Debug.Log("Tentou criar um ficheiro de Exercicio mas nao existe o folder Sessão>Sequencia");
            return;
        }
        String filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" +
         SessionInfo.getSessionPath() + "/" + SequenceManager.sequence.getTimestamp() + "/" + filename + ".txt";
        Debug.Log(filepath);
        using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine("timestamp=" + filename.Replace("Exercise", string.Empty)); //"Level_01".Replace("Level_", string.Empty);
            writer.WriteLine("name=" + _name);
            writer.WriteLine("arm=" + _arm);
            writer.WriteLine("nrReps=" + _nreps);
            writer.WriteLine("duration=" + _duration);
            writer.WriteLine("restTime=" + _restTime);
            writer.WriteLine("tries=" + _tries);
            writer.WriteLine("correctReps=" + _correctReps);
            writer.WriteLine("outOfPath=" + _outOfPath);
            writer.WriteLine("avgTime=" + _avgTime);
            writer.WriteLine("totalTime=" + _totalTime);
            //TODO
            writer.WriteLine("regShouldComp=" + State.registerShoulderComp);
            writer.WriteLine("regSpineComp=" + State.registerSpineComp);
            //
            writer.WriteLine("leftShoulderComp=" + _lShoulderComp);
            writer.WriteLine("rightShoulderComp=" + _rShoulderComp);
            writer.WriteLine("spineComp=" + _spineComp);

            writer.Close();
            // FC should i close stream too?
            stream.Close();
        }

        //Copy to "Last" folder TODO
        // FC - não creio que valha a pena ter um LAST folder para exercicios

        /*
        if (!System.IO.Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/" ))
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/" );
        filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Last/" + _scenePath + ".txt";
        if (System.IO.File.Exists(filepath)) System.IO.File.Delete(filepath);
        using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine("timestamp=" + filename);
            //writer.WriteLine("name=" + _name);
            writer.WriteLine("arm=" + _arm);
            writer.WriteLine("nrReps=" + _nreps);
            //writer.WriteLine("duration=" + _duration);
            //writer.WriteLine("restTime=" + _restTime);
            writer.WriteLine("tries=" + _tries);
            writer.WriteLine("correctReps=" + _correctReps);
            //writer.WriteLine("outOfPath=" + _outOfPath);
            //writer.WriteLine("avgTime=" + _avgTime);
            //writer.WriteLine("totalTime=" + _totalTime);
            //TODO
            //writer.WriteLine("regShouldComp=" + State.registerShoulderComp);
            //writer.WriteLine("regSpineComp=" + State.registerSpineComp);
            //
            //writer.WriteLine("leftShoulderComp=" + _lShoulderComp);
            //writer.WriteLine("rightShoulderComp=" + _rShoulderComp);
            //writer.WriteLine("spineComp=" + _spineComp);

            if (_targetPositions != null)
            {
                for (int i = 0; i < _targetPositions.Length; i++)
                {
                    writer.WriteLine("target" + i + "=" + _targetPositions[i].ToString());
                }
            }
            if (_pathPosition != null)
            {
                writer.WriteLine("pathPosition=" + _pathPosition.ToString());
            }

            writer.Close();
            // FC should i close stream too?
            stream.Close();
        }*/
    }
    
    
    public string toSequenceFile()
    {
        string arm = "left";
        if (!_leftArm) arm = "right";

        return _id + "=" + _name + "=" + _scenePath + "=" + arm + "=" + _nreps + "=" + _duration + "=" + _restTime;
    }    
}
