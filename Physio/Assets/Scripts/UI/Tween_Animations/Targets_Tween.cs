using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets_Tween : MonoBehaviour
{
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        Shake_Target();
        SquashAndStretch();
    }

    public void Shake_Target(){
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalX(gameObject, 0f, 0.5f).setEaseShake(); 
    }

    public void SquashAndStretch(){
        // scale down
        gameObject.transform.localScale = new Vector3(initialScale.x * 0.3f,initialScale.y * 0.3f,initialScale.z * 0.3f);
        // scale up elastic
        LeanTween.scale(gameObject, initialScale, 1f).setEase(LeanTweenType.easeOutElastic);
    }


    
}
