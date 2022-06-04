using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Sequence
{
    private int _id; // never used?
    private string _name;
    private string _timestamp;
    // New variable - Francisco Cecílio
    private int _series; // _series is equal to the Lenght of _exerciseList
    // New variable - Francisco Cecílio
    private int _restDuration; // rest time between series (or exercises from the list)

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

    // It must receive a string in this format "SequenceyyyyMMddTHHmmss"
    public void setTimestamp(string timestamp)
    {
        _timestamp = timestamp;
    }

    public void setSeries(int numSeries)
    {
        _series = numSeries;
    }

    public int getSeries()
    {
        return _series;
    }

    public void setRestDuration(int duration)
    {
        _restDuration = duration;
    }

    public int getRestDuration()
    {
        return _restDuration;
    }

    public void addExercise(Exercise exercise)
    {
        _exerciseList.Add(exercise);
    }

    public void clearExerciseList()
    {
        _exerciseList.Clear();
    }

    public Exercise getExercise(int index)
    {
        if(index >= _exerciseList.Count){
            Debug.Log("ERROR: Tried to GetExercise that does not exist.");
            return null;
        }
        return _exerciseList[index];
    }

    public List<Exercise> GetExerciseList(){
        return _exerciseList;
    }
   
    // Before: creates a new directory with a sequence file in the directory USER/username/<SessionTS>/Sequences/"<SequenceTS>.txt"
    // Now: creates a new directory with a sequence file in the directory Users/<username>/<SessionTS>/<SequenceTS>/"<SequenceTS>.txt"

    // NOT ANYMORE: Called on SequenceManager.newSequence(name) e nesse caso a sequencia nao tem exercicios...
    // NOT ANYMORE: Called after Editing/Creating a new Sequence. (but will no longer be)

    public void toFile()
    {
        Debug.Log("Sequence is going to be saved in "+Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/" + _timestamp);
        // Create directory <SequenceTS>
        if (!Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/"  + _timestamp))
            Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/" + _timestamp);
        
        // Create file <SequenceTS>.txt
        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername()+ "/"  + SessionInfo.getSessionPath() + "/" + _timestamp + "/" + _timestamp + ".txt";
        if (System.IO.File.Exists(filepath)) 
            System.IO.File.Delete(filepath);

        using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine("timestamp=" + _timestamp.Replace("Sequence", string.Empty) );
            writer.WriteLine("name=" + _name);  
            writer.WriteLine("series=" + _series);  
            writer.WriteLine("restDuration=" + _restDuration);  

            for (int i = 0; i < _exerciseList.Count; i++)
            {
                writer.WriteLine("exe=" + _exerciseList[i].toSequenceFile());
            }

            writer.Close();
            stream.Close();
        }
        RefreshEditorProjectWindow();
    }

    private static void  RefreshEditorProjectWindow() 
    {
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }
    
    //TODO
    //LOG TO FILE
}