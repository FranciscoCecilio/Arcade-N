using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightCornerTimer : MonoBehaviour
{
    
    public TMP_Text time;

    // Update is called once per frame
    void Update()
    {
        time.text = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
    }
}
