using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This static class's atribute is used in certain scenes (e.g. Main Menu) to know the scene that loaded it.
// E.g. Main menu doesn't play the UI animations unless it was loaded from the Login Scene. 
// _lastSceneIndex needs to be set via script 

public class LastScene : MonoBehaviour
{
    public static int _lastSceneIndex;
    
}
