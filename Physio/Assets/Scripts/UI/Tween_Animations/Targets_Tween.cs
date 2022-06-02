using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets_Tween : MonoBehaviour
{
    void Start()
    {
        Shake_Target();
    }
    // Update is called once per frame
    void Update()
    {
        //2
        //LeanTween.cancel(gameObject);
        //3
        //LeanTween.moveX(gameObject, transform.position.x - 0.05f, 0.5f).setEaseShake(); 
    }

    public void Shake_Target(){
        //2
        LeanTween.cancel(gameObject);
        //3
        LeanTween.moveX(gameObject, transform.position.x - 0.5f, 0.5f).setEaseShake(); 
    }
}
