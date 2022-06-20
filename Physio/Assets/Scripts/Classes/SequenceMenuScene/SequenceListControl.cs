using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SequenceListControl : MonoBehaviour
{
    public GameObject listElementPrefab;

    private Sequence selectedSequence;

    public GameObject deleteDialogue;

    public GameObject clearDialogue;

    public Transform listContent;

    [SerializeField] ExerciseParametersPanel exPanel;
   
    public void ActiveSequence(Sequence _sequence)
    {
        selectedSequence = _sequence;
    }

    public void OpenDeleteDialogue(){
        deleteDialogue.SetActive(true);
    }

    public void CloseDeleteDialogue(){
        deleteDialogue.SetActive(false);
    }

    public void OpenClearDialogue(){
        clearDialogue.SetActive(true);
    }

    public void CloseClearDialogue(){
        clearDialogue.SetActive(false);
    }
    
    // Destroi o botao da lista
    public void DestroySequence()
    {
        //Apaga o botão da sequência
        GameObject button = listContent.Find(selectedSequence.getTimestamp() + "Button").gameObject;
        if(button == null) Debug.Log("Error trying to find the button to delete!");
        
        button.SetActive(false);
        Destroy(button);

        CloseDeleteDialogue();

        //Apaga o ficheiro da sequência - Já não é suposto porque o ficheiro não é criado quando o botão é criado, apenas quando se efetuam os exercícios.
        /*string sequencePath = Application.dataPath + "/Users/" + SessionInfo.getUsername() + SessionInfo.getSessionPath() + "/Sequences/" + selectedSequence.getTimestamp() + ".txt";
        if(!File.Exists(sequencePath)) Debug.Log("Error trying to find file to delete in: " + sequencePath);
        File.Delete(sequencePath);
        RefreshEditorProjectWindow();*/
    }

    public void ClearList(){
        // iteramos todos os botoes e apagamos um a um
        foreach (Transform eachChild in listContent) {
            Debug.Log("iterou");
            if (eachChild.name.Substring(eachChild.name.Length - 6) == "Button") {
                Debug.Log("entrou: "+eachChild.name);
                ActiveSequence(eachChild.gameObject.GetComponent<SequenceListElement>().GetSequence());
                DestroySequence();
                //eachChild.gameObject.GetComponent<SequenceListElement>().DeleteSequenceButton();
                Debug.Log ("Button deleted. Name: " + eachChild.name);
            }
        }
        CloseClearDialogue();
        exPanel.flowManager.CloseAllPanels();
    }

    // TODO: Gerar botoes de sequencia iguais aos da sessão mais recente
    // comparar Session folder names
    // Buscar todos os folders de sequencias 
    // Dentro de cada folder aproveitar o ficheiro de sequência se houver
    void Start()
    {

        /*if (!System.IO.Directory.Exists(Application.dataPath + "/Users/" + SessionInfo.getUsername() + SessionInfo.getSessionPath()+ "/Sequences/"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Users/" + SessionInfo.getUsername() + SessionInfo.getSessionPath() + "/Sequences/");
        }

        DateTime mostRecentTimestamp = DateTime.ParseExact("19000324T162543", "yyyyMMddTHHmmss", null); // inicializamos com um DateTime antigo (de 1900)
        string mostRecentTSfilename = "";
        // encontramos o ficheiro com o timestamp mais recente 
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/" + SessionInfo.getUsername() + SessionInfo.getSessionPath() + "/Sequences/")) //Ficheiros na pasta Sequences
        {
            string[] filename = file.Split('.');
            if (filename.Length == 2 && filename[1] == "txt") //Verifica que e um ficheiro txt e nao meta
            {
                string actualFilename = filename[0].Substring(filename[0].Length - 15);
                //Debug.Log("FilenameFound: " + actualFilename);
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
        //Debug.Log("mostrecent TS: " + mostRecentTimeStampString);

        // criamos um botao de sequencia igual ao da ultima sessão
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath + "/Users/" + SessionInfo.getUsername() + SessionInfo.getSessionPath() + "/Sequences/")) //Ficheiros na pasta Sequences
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
                        /*
                        string[] data = line.Split('=');

                        if (data[0] == "name") tempSequence.setName(data[1]);

                        else if (data[0] == "timestamp") tempSequence.setTimestamp("Sequence" + data[1]);

                        else if (data[0] == "series") tempSequence.setSeries(Int32.Parse(data[1]));

                        else if (data[0] == "restDuration") tempSequence.setRestDuration(Int32.Parse(data[1]));

                        else if (data[0] == "exe")
                        {
                            int armVar; // because some old files will still have "left" instead of "0"
                            if(data[4] == "left") {
                                armVar = 0;
                            }
                            else if(data[4] == "right"){
                                armVar = 1;
                            }
                            else{
                                armVar = Int32.Parse(data[4]);
                            }
                            tempSequence.addExercise(new Exercise(Int32.Parse(data[1]), data[2], data[3], armVar, Int32.Parse(data[5]), Int32.Parse(data[6]), Int32.Parse(data[7])));
                        }

                        line = reader.ReadLine();
                    }
                }
                reader.Close();
                // generate the button on the list
                GenerateSequenceButton(tempSequence);
            }
        }*/
    }

    // Generates a button (that contains a list element) AND opens EDITING
    public void GenerateSequenceButton(Sequence seq){
        if(seq == null){
            Debug.Log("ERROR: Trying to create a button for a null sequence");
            return;
        }
        //Instantiate prefab
        GameObject button = Instantiate(listElementPrefab) as GameObject;
        button.name = seq.getTimestamp() + "Button";
        button.SetActive(true);
        // Sets the sequence on SequenceListElement
        button.GetComponent<SequenceListElement>().SetSequence(seq);
        button.transform.SetParent(listContent, false);
        // Highlights the button (and starts EDITING)
        button.GetComponent<HighlightElement>().HighlightListElement();
        // Starts the Editing panel
        exPanel.SetPanelActive(button.GetComponent<SequenceListElement>());
    }

    
    void RefreshEditorProjectWindow()
    {
    #if UNITY_EDITOR
           UnityEditor.AssetDatabase.Refresh();
    #endif
    }
}
