using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// OLD script. In the new version, deleting a list element is handled in SequenceListControl
public class DeleteSequence : MonoBehaviour {

    public GameObject confirmationBox;
    public GameObject exerciseListText;
    public GameObject runButton;
    public GameObject deleteButton;

    public void showDeleteConfirmationBox()
    {
        confirmationBox.SetActive(true);
    }

    public void cancelDeleteConfirmationBox()
    {
        confirmationBox.SetActive(false);
    }

    public void destroySequence()
    {
        GameObject sequencesList = GameObject.Find("Sequences");
        SequenceListControl script = (SequenceListControl)sequencesList.GetComponent(typeof(SequenceListControl));
        script.DestroySequence();

        confirmationBox.SetActive(false);
        exerciseListText.SetActive(false);
        runButton.SetActive(false);
        deleteButton.SetActive(false);
    }
}
