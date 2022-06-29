using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellDoneMessageAnimation : MonoBehaviour
{

    public GameObject objectToAnimate;
    public GameObject confettiBurst;

    void OnEnable()
    {   
       Animate(); 
    }


    public void Animate(){
        confettiBurst.SetActive(false);
        LeanTween.moveLocalY(objectToAnimate, 500 ,0f);

        LeanTween.moveLocalY(objectToAnimate, 0 , 1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(ActivateConfetti);
        LeanTween.moveLocalY(objectToAnimate, 500 , 2f).setDelay(3f).setEase(LeanTweenType.easeInOutElastic).setOnComplete(DeActivateConfetti);
    }

    public void ActivateConfetti(){
        confettiBurst.SetActive(true);
    }
    public void DeActivateConfetti(){
        confettiBurst.SetActive(false);
    }
}
