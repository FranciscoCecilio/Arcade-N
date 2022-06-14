using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float tweenTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        //Tween();
    }

    public void ScaleUp(){
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 1.1f, tweenTime).setEase(LeanTweenType.easeInBack);
    }

    public void ScaleBack(){
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, tweenTime);
    }

    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("enter");
        ScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData){
        Debug.Log("exit");
        ScaleBack();
    }
}
