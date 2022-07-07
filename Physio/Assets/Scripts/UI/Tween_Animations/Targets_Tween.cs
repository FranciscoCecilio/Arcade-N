using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Targets_Tween : MonoBehaviour
{
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        if(!SceneManager.GetActiveScene().name.Equals("Exercise0Scene"))    Shake_Target();
        SquashAndStretch();
        StartCoroutine(SetInitialSize());
    }

    public void Shake_Target(){
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalX(gameObject, 10f, 0.2f).setLoopType(LeanTweenType.pingPong).setRepeat(4);
    }

    public void SquashAndStretch(){
        // scale down
        gameObject.transform.localScale = new Vector3(initialScale.x * 0.3f,initialScale.y * 0.3f,initialScale.z * 0.3f);
        // scale up elastic
        LeanTween.scale(gameObject, initialScale, 1f).setEase(LeanTweenType.easeOutElastic);
    }

    // sometimes the ball would be little... this is a prevention method
    public IEnumerator SetInitialSize(){
        yield return new WaitForSeconds(2);
        //gameObject.transform.localScale = initialScale; ITS NOT WORKING
        gameObject.transform.localScale = new Vector3(35,35,35);

    }


    
}
