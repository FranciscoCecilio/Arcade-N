using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseListButton : MonoBehaviour
{
    public Text buttonName;

    public void SetText(string textString)
    {
        buttonName.text = textString;
    }

    public void OnClick()
    {

    }
}
