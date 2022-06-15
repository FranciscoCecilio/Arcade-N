using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// hovering this object will show a message
public class HoverSetActive :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject objectToShow;
    float tweenTime = 0.2f;
    SoundManager soundManager; // if the user clicks on the music buttons, we tell SoundManager

    // Start is called before the first frame update
    void Start()
    {
        objectToShow.SetActive(false);
    }

    public void ShowObject(){
        objectToShow.SetActive(true);
        LeanTween.cancel(objectToShow);
        // scale width
        objectToShow.transform.localScale = new Vector3(1,0,0);
        //scale height
        LeanTween.scale(objectToShow, new Vector3(1, 1, 1), tweenTime).setEase(LeanTweenType.easeInBack);
    }

    public void HideObject(){
        objectToShow.SetActive(false);
        objectToShow.transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData){
        ShowObject();
    }

    public void OnPointerExit(PointerEventData eventData){
        HideObject();
    }
}
