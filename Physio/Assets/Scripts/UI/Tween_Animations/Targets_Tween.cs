using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets_Tween : MonoBehaviour
{
    void Start()
    {
        //Shake_Target();
        SquashAndStretch();
    }

    public void Shake_Target(){
        //2
        LeanTween.cancel(gameObject);
        //3
        LeanTween.moveX(gameObject, transform.position.x - 0.5f, 0.5f).setEaseShake(); 
    }

    public void SquashAndStretch(){
        gameObject.transform.localScale = new Vector3(10f,10f,10f);
        LeanTween.scale(gameObject, new Vector3(35f,35f,35f), 2f).setEase(LeanTweenType.easeOutElastic);
    }
}
