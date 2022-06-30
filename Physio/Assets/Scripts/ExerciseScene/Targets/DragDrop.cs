using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used to Move the targets vertically
public class DragDrop : MonoBehaviour {
    public GameObject exerciseBox;
    public Transform otherTarget;

    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 screenPointAO;
    private Vector3 offsetAO;

    private bool locked = false;

    [Header("Debugging")]
    [HideInInspector]
    public float y_value;
    public float otherTarget_y;
    public float barInitialHeight;
    
    public float x_value;
    public float otherTarget_x;
    public float barInitialWidth;

    public float barInitialZScale;
    public bool dragDetected = false;
    public bool isVerticalScene;

    // Use this for initialization
    void Start () {
		
        barInitialZScale = exerciseBox.transform.localScale.z;

        if( SceneManager.GetActiveScene().name == "Exercise2Scene"){
            isVerticalScene = true;
            y_value = transform.position.y;
            otherTarget_y = otherTarget.position.y;
            barInitialHeight = Mathf.Abs(otherTarget_y - y_value);
        }
        else{
            isVerticalScene = false;
            x_value = transform.position.x;
            otherTarget_x = otherTarget.position.x;
            barInitialWidth = Mathf.Abs(otherTarget_x - x_value);
        }

	}
	
    void OnMouseDown()
    {
        if (!locked)
        {
            if(isVerticalScene){
                y_value = transform.position.y;
                otherTarget_y = otherTarget.position.y;

                screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                screenPointAO = Camera.main.WorldToScreenPoint(exerciseBox.GetComponent<Transform>().position);

                offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z));
                offsetAO = exerciseBox.GetComponent<Transform>().position - Camera.main.ScreenToWorldPoint(new Vector3(screenPointAO.x, Input.mousePosition.y, screenPointAO.z));
            }
            else{ // same thing but horizontally
                x_value = transform.position.x;
                otherTarget_x = otherTarget.position.x;

                screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                screenPointAO = Camera.main.WorldToScreenPoint(exerciseBox.GetComponent<Transform>().position);

                offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3( Input.mousePosition.x, screenPoint.y, screenPoint.z));
                offsetAO = exerciseBox.GetComponent<Transform>().position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, screenPointAO.y, screenPointAO.z));
            }
        }
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            if(isVerticalScene){
                Vector3 curScreenPoint = new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                // Targets get closer to each other
                transform.position = curPosition;
                otherTarget.position = new Vector3(otherTarget.position.x, otherTarget_y - (curPosition.y - y_value), otherTarget.position.z); 

                // rescale the green bar
                float zScale =  Mathf.Abs(otherTarget.position.y - transform.position.y) * barInitialZScale / barInitialHeight ;
                exerciseBox.transform.localScale = new Vector3(exerciseBox.transform.localScale.x, exerciseBox.transform.localScale.y, zScale);

                // We want to check if any changes were made
                dragDetected = true;
            }
            else{
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                // Targets get closer to each other
                transform.position = curPosition;
                otherTarget.position = new Vector3(otherTarget_x - (curPosition.x - x_value), otherTarget.position.y, otherTarget.position.z); 

                // rescale the green bar
                float zScale =  Mathf.Abs(otherTarget.position.x - transform.position.x) * barInitialZScale / barInitialWidth ;
                exerciseBox.transform.localScale = new Vector3(exerciseBox.transform.localScale.x, exerciseBox.transform.localScale.y, zScale);

                // We want to check if any changes were made
                dragDetected = true;
            }
        }
    }

    public void lockDragDrop(bool intention)
    {
        locked = intention;
        // We want to check if any changes were made
        if(locked == true && dragDetected == true){
            dragDetected = false;
            ExercisePreferencesSetup.preferencesChanged = true;
        }
    }
}
