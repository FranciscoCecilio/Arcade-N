using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// CHANGE FOREACH TO GETNAME instead of kiko12
public class SequenceListControl : MonoBehaviour
{
    public GameObject listElementPrefab;

    private Sequence selectedSequence;

    [SerializeField] Transform listContent;

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

    // Gera sequencias baseadas na ultima sessão
    // TODO: nos queremos criar apenas um botao com 1 sequencia da ultima sessão? ou queremos todas series da ultima sessao?
    void Start()
    {
        if (!System.IO.Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/");
        }

        DateTime mostRecentTimestamp = DateTime.ParseExact("19000324T162543", "yyyyMMddTHHmmss", null); // inicializamos com um DateTime antigo (de 1900)
        string mostRecentTSfilename = "";
        // encontramos o ficheiro com o timestamp mais recente 
        //foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/" + SessionInfo.getUsername() + "/Sequences/")) //Ficheiros na pasta Sequences
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/kiko12/Sequences/")) //Ficheiros na pasta Sequences
        {
            string[] filename = file.Split('.');
            if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
            {
                string actualFilename = filename[0].Substring(filename[0].Length - 15);
                Debug.Log("FilenameFound: " + actualFilename);
                // converter o timestamp para data time
                DateTime tempDate = DateTime.ParseExact(actualFilename, "yyyyMMddTHHmmss", null);
                // guardar o mais recente
                if(DateTime.Compare(mostRecentTimestamp, tempDate) < 0){
                    // entao o tempData é mais recente
                    mostRecentTimestamp = tempDate;
                    // e guardamos o filename
                    mostRecentTSfilename = filename[0];
                }
            }
        }
        
        string mostRecentTimeStampString = mostRecentTimestamp.ToString("yyyyMMddTHHmmss");
        Debug.Log("mostrecent TS: " + mostRecentTimeStampString);

        // criamos um botao de sequencia igual ao da ultima sessão
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/kiko12/Sequences/")) //Ficheiros na pasta Sequences
        {
            string[] filename = file.Split('.');
            if (filename.Length == 2 && filename[1] == "txt" && filename[0].Equals(mostRecentTSfilename,StringComparison.Ordinal)) //Verifica que e um ficheiro txt e nao meta e se é o ficheiro da sessão mais recente
            {

                //INFORMACOES DA SEQUENCIA
                Sequence tempSequence = new Sequence();

                string line = "";
                StreamReader reader = new StreamReader(file);
                {
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        /* exemplo de Sequencia.txt

                        timestamp=20220324  T162543
                        name=j
                        exe=2=Left/Right=Exercise2Scene=right=10=60=60

                        */
                        string[] data = line.Split('=');

                        if (data[0] == "name") tempSequence.setName(data[1]);

                        else if (data[0] == "timestamp") tempSequence.setTimestamp(data[1]);

                        else if (data[0] == "exe")
                        {
                            tempSequence.addExercise(new Exercise(Int32.Parse(data[1]), data[2], data[3], data[4], Int32.Parse(data[5]), Int32.Parse(data[6]), Int32.Parse(data[7])));
                        }

                        line = reader.ReadLine();
                    }
                }
                Debug.Log("chegou aqui");
                //BOTAO SEQUENCIA 
                GameObject button = Instantiate(listElementPrefab) as GameObject;
                button.name = tempSequence.getName() + "Button";
                button.SetActive(true);
                //button.GetComponent<SequenceListButton>().SetSequence(tempSequence);
                button.GetComponent<SequenceListElement>().SetSequence(tempSequence);
                // TODO perceber como instanciar Butoes sem estar praqui a fazer scaledowns
                button.transform.SetParent(listContent);
                Debug.Log("e aqui");

                reader.Close();
            }
        }
    }
}
