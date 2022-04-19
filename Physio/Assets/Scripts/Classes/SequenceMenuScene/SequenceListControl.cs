using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 
public class SequenceListControl : MonoBehaviour
{
    public GameObject buttonTemplate;

    private Sequence selectedSequence;

    public void ActiveSequence(Sequence _sequence)
    {
        selectedSequence = _sequence;
    }

    public void DestroySequence()
    {
        //Apaga o botão da sequência
        GameObject button = GameObject.Find(selectedSequence.getName() + "Button");
        button.SetActive(false);

        //Limpa lista de exercicios
        GameObject[] exercs = GameObject.FindGameObjectsWithTag("ExerciseName");
        foreach (GameObject exerc in exercs) GameObject.Destroy(exerc);

        //Apaga o ficheiro da sequência
        string sequencePath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/" + selectedSequence.getTimestamp() + ".txt";
        File.Delete(sequencePath);
        RefreshEditorProjectWindow();
    }

    void RefreshEditorProjectWindow()
    {
    #if UNITY_EDITOR
           UnityEditor.AssetDatabase.Refresh();
    #endif
    }

    void Start()
    {
        if (!System.IO.Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/");
        }
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/")) //Ficheiros na pasta Sequences
        {
            string[] filename = file.Split('.');
            if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
            {
                //INFORMACOES DA SEQUENCIA
                Sequence tempSequence = new Sequence();

                string line = "";
                StreamReader reader = new StreamReader(file);
                {
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] data = line.Split('=');

                        if (data[0] == "name") tempSequence.setName(data[1]);

                        if (data[0] == "timestamp") tempSequence.setTimestamp(data[1]);

                        if (data[0] == "exe")
                        {
                            tempSequence.addExercise(new Exercise(Int32.Parse(data[1]), data[2], data[3], data[4], Int32.Parse(data[5]), Int32.Parse(data[6]), Int32.Parse(data[7])));
                        }

                        line = reader.ReadLine();
                    }
                }

                //BOTAO SEQUENCIA
                GameObject button = Instantiate(buttonTemplate) as GameObject;
                button.name = tempSequence.getName() + "Button";
                button.SetActive(true);
                button.GetComponent<SequenceListButton>().SetSequence(tempSequence);
                button.transform.SetParent(buttonTemplate.transform.parent, false);

                reader.Close();
            }
        }
    }
}
