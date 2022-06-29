using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AvatarImage : MonoBehaviour
{
    public Sprite maleAvatar;
    public Sprite femaleAvatar;
    public Image placeholder;
    // Start is called before the first frame update
    void Start()
    {
        PlaceImage();
    }

    void OnEnable()
    {
        //PlaceImage();
    }

    void PlaceImage(){
        if(SessionInfo.getGender().Equals("masculino")) placeholder.sprite = maleAvatar;
        else if(SessionInfo.getGender().Equals("feminino")) placeholder.sprite = femaleAvatar;
        else{
            Debug.LogError("Error: SessionInfo.getGender() = " + SessionInfo.getGender() + " is neither masculino ou feminino");
            placeholder.sprite = maleAvatar;
        } 
    }
    
}
