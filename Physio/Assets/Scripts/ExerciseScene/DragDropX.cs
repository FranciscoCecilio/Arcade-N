using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropX : MonoBehaviour {

    public GameObject associatedObj;

    public float min;
    public float max;

    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 screenPointAO;
    private Vector3 offsetAO;

    private bool locked = false;

    void OnMouseDown()
    {
        if (!locked)
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            screenPointAO = Camera.main.WorldToScreenPoint(associatedObj.GetComponent<Transform>().position);

            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z));
            offsetAO = associatedObj.GetComponent<Transform>().position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, screenPointAO.y, screenPointAO.z));
        }
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            /*if (curPosition.x > max) curPosition.x = max;
            else if (curPosition.x < min) curPosition.x = min;*/
            transform.position = curPosition;

            Vector3 curScreenPointAO = new Vector3(Input.mousePosition.x, screenPointAO.y, screenPointAO.z);
            Vector3 curPositionAO = Camera.main.ScreenToWorldPoint(curScreenPointAO) + offsetAO;

            /*if (curPositionAO.x > max) curPositionAO.x = max;
            else if (curPositionAO.x < min) curPositionAO.x = min;*/
            associatedObj.GetComponent<Transform>().position = curPositionAO;
        }
    }

    public void lockDragDrop(bool intention)
    {
        locked = intention;
    }
}
