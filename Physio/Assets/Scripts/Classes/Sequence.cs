using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Sequence
{
    private int _id;
    private string _name;
    private string _timestamp;

    private List<Exercise> _exerciseList = new List<Exercise>();

    public Sequence()
    {

    }

    public Sequence(string name)
    {
        _name = name;
    }

    public void setName(string name)
    {
        _name = name;
    }

    public string getName()
    {
        return _name;
    }

    public List<int> getExercisesIds()
    {
        List<int> _exercisesIdList = new List<int>();
        for (int i = 0; i < _exerciseList.Count; i++)
        {
            _exercisesIdList.Add(_exerciseList[i].getId());
        }
        return _exercisesIdList;
    }

    public void setId(int id)
    {
        _id = id;
    }

    public int getId()
    {
        return _id;
    }

    public int getLength()
    {
        return _exerciseList.Count;
    }

    public string getTimestamp()
    {
        return _timestamp;
    }

    public void setTimestamp(string timestamp)
    {
        _timestamp = timestamp;
    }

    public void addExercise(Exercise exercise)
    {
        _exerciseList.Add(exercise);
    }

    public Exercise getExercise(int index)
    {
        return _exerciseList[index];
    }

    // creates a new sequence file in the directory USER/SEQUENCES/"<timestamp>.txt"
    // this is called in the SequenceManager.newSequence(name)
    public void toFile()
    {
        if (!Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/"))
            Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/");
        
        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/" + _timestamp + ".txt";

        if (System.IO.File.Exists(filepath)) System.IO.File.Delete(filepath);

        using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine("timestamp=" + _timestamp);
            writer.WriteLine("name=" + _name);

            for (int i = 0; i < _exerciseList.Count; i++)
            {
                writer.WriteLine("exe=" + _exerciseList[i].toSequenceFile());
            }
        }
    }

    //TODO
    //LOG TO FILE
}