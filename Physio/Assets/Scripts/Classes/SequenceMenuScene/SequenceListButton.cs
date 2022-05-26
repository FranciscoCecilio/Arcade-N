using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Esta classe Já nao é usada, passou a ser o SequenceListElement
public class SequenceListButton : MonoBehaviour
{
    private Sequence _sequence;

    public GameObject exerciseListText;
    public GameObject exerciseButtonTemplate;
    public GameObject runButton;
    public GameObject deleteButton;
    public GameObject addExerciseButton;


    private void renderExercisesList()
    {
        //Limpa lista de exercicios
        GameObject[] exercs = GameObject.FindGameObjectsWithTag("ExerciseName");
        foreach (GameObject exerc in exercs) GameObject.Destroy(exerc);

        //Procurar exercicios
        List<int> exercisesIds = _sequence.getExercisesIds();
        for (int i = 0; i < exercisesIds.Count; i++)
        {
            string exerciseName = "";
            string line = "";
            StreamReader reader = new StreamReader(Application.dataPath + "/Exercises/exercise" + exercisesIds[i].ToString() + ".txt");
            {
                line = reader.ReadLine();
                while (line != null)
                {
                    string[] data = line.Split('=');
                    if (data[0] == "name")
                    {
                        exerciseName = data[1];
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            GameObject button = Instantiate(exerciseButtonTemplate) as GameObject;
            button.tag = "ExerciseName";
            button.SetActive(true);
            button.GetComponent<ExerciseListButton>().SetText(exerciseName);
            button.transform.SetParent(exerciseButtonTemplate.transform.parent, false);
        }
    }

    public void OnClick()
    {
        //Define como sequencia ativa (a mostrar exercicios)
        GameObject sequencesList = GameObject.Find("Sequences");
        SequenceListControl script = (SequenceListControl) sequencesList.GetComponent(typeof(SequenceListControl));
        script.ActiveSequence(_sequence);
        SequenceManager.sequence = _sequence;

        //Renderiza a lista de exercicios
        exerciseListText.SetActive(true);

        runButton.SetActive(true);
        if (_sequence.getExercisesIds().Count == 0) runButton.GetComponent<Button>().interactable = false;
        else runButton.GetComponent<Button>().interactable = true;

        deleteButton.SetActive(true);

        addExerciseButton.SetActive(true);

        renderExercisesList();
    }
}
