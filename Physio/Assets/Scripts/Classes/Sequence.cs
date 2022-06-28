using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

// Just a reminder: a Sequence has a list of Exercises (each exercise is exactly the same). 
// When a Sequence is played, we are playing the same Exercise <_nSeries> times.
public class Sequence
{
    private string _name;
    private string _timestamp; // format: SequenceyyyyMMddTHHmmss
    private int _series; // _series is equal to the Lenght of _exerciseList
    private int _restDuration; // rest time in seconds between series
    private int totalRepetitions = 0; // this variable is usefull to calculate the Percentage_per_Repetition during the Exercise Screen
    
    // calculated to save in file
    private string _totalDuration;
    private double _performance;

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

    // represents the exercise type 0 grid, 1 horizontal, 2 vertical
    public int getExercisesId()
    {
        return _exerciseList[0].getId();
    }

    public int getLength()
    {
        return _exerciseList.Count;
    }

    public string getTimestamp()
    {
        return _timestamp;
    }

    public string getTotalDuration(){
        return _totalDuration;
    }

    public double getPerformance(){
        return _performance;
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
        totalRepetitions += exercise.getNReps(); // everytime we add an exercise we change the value of totalRepetitions
        _exerciseList.Add(exercise);
        _series = _exerciseList.Count;
        _restDuration = exercise.getRestTime();
    }

    public int getTotalRepetitions(){
        return totalRepetitions;
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
        // Create directory <SequenceTS>
        if (!Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/"  + _timestamp))
            Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/" + SessionInfo.getSessionPath() + "/" + _timestamp);
        
        // Create file <SequenceTS>.txt
        string filepath = Application.dataPath + "/Users/" + SessionInfo.getUsername()+ "/"  + SessionInfo.getSessionPath() + "/" + _timestamp + "/" + _timestamp + ".txt";
        if (System.IO.File.Exists(filepath)) 
            System.IO.File.Delete(filepath);

        // Calculate the total duration of the sequence
        System.DateTime timestamp = DateTime.ParseExact(_timestamp.Replace("Sequence",string.Empty), "yyyyMMddTHHmmss",System.Globalization.CultureInfo.InvariantCulture); 
        System.DateTime  now = System.DateTime.Now;
        TimeSpan difference = now - timestamp;
        _totalDuration = string.Format("{0:D2}:{1:D2}:{2:D2} horas", difference.Hours, difference.Minutes, difference.Seconds);

        // We need the overall Performance of Sequence (correctReps / tries)
        List<int> exPerformances = new List<int>();

        using (var stream = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
        using (var writer = new StreamWriter(stream))
        {
            writer.WriteLine("timestamp=" + _timestamp.Replace("Sequence", string.Empty) );
            writer.WriteLine("totalDuration=" + _totalDuration );
            writer.WriteLine("name=" + _name);  
            writer.WriteLine("series=" + _series);  
            writer.WriteLine("restDuration=" + _restDuration);  

            for (int i = 0; i < _exerciseList.Count; i++)
            {
                writer.WriteLine("exe=" + _exerciseList[i].toSequenceFile());
                // calculate each exercise performance
                int exPerformance = _exerciseList[i].getCorrectReps() / _exerciseList[i].getTries();
                exPerformances.Add(exPerformance);
                Debug.Log("Calculated performance for exercise"+i+" : " + exPerformance);
            }
            // calculate average performance
            double average = exPerformances.Count > 0 ? exPerformances.Average() : 0.0;
            double rounded_avg = Math.Round(average, 2); //rounds 1.5362 to 1.54
            _performance = rounded_avg;
            Debug.Log("Calculated avg performance for sequence: " + rounded_avg);
            writer.WriteLine("performance=" + _performance);  // goes from 0 to 1

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