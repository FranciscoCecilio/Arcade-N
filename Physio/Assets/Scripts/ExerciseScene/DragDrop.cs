using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float y_value;
    public float otherTarget_y;
    public float barInitialHeight;

    // Use this for initialization
    void Start () {
		y_value = transform.position.y;
        otherTarget_y = otherTarget.position.y;
        barInitialHeight = Mathf.Abs(otherTarget_y - y_value);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (!locked)
        {
            y_value = transform.position.y;
            otherTarget_y = otherTarget.position.y;

            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            screenPointAO = Camera.main.WorldToScreenPoint(exerciseBox.GetComponent<Transform>().position);

            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z));
            offsetAO = exerciseBox.GetComponent<Transform>().position - Camera.main.ScreenToWorldPoint(new Vector3(screenPointAO.x, Input.mousePosition.y, screenPointAO.z));
        }
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            Vector3 curScreenPoint = new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            // Targets get closer to each other
            transform.position = curPosition;
            otherTarget.position = new Vector3(otherTarget.position.x, otherTarget_y - (curPosition.y - y_value), otherTarget.position.z); 

            // rescale the green bar
            float zScale = Mathf.Abs(otherTarget.position.y - transform.position.y) / barInitialHeight ;
            exerciseBox.transform.localScale = new Vector3(exerciseBox.transform.localScale.x, exerciseBox.transform.localScale.y, zScale);

            /*Vector3 curScreenPointAO = new Vector3(screenPointAO.x, Input.mousePosition.y, screenPointAO.z);
            Vector3 curPositionAO = Camera.main.ScreenToWorldPoint(curScreenPointAO) + offsetAO;*/
            //associatedObj.GetComponent<Transform>().position = curPositionAO;
            
        }
    }

    public void lockDragDrop(bool intention)
    {
        locked = intention;
    }
}
