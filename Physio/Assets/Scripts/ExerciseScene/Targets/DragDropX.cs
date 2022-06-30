﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to Translate the green Box (exercise trajectory) in both x and y
public class DragDropX : MonoBehaviour {

    public GameObject associatedObj; // targets

    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 screenPointAO;
    private Vector3 offsetAO;

    private bool locked = false;
    [Header("Debugging")]
    public bool dragDetected = false;

    void Start()
    {
        foreach(Transform target in associatedObj.transform){
            if(target.gameObject.tag.Equals("TargetCollider")){
                target.localPosition = new Vector3(0,target.localPosition.y,0);
            }
        }
        associatedObj.transform.position = transform.position;
    }

    void OnMouseDown()
    {
        if (!locked)
        {
            
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            //screenPointAO = Camera.main.WorldToScreenPoint(associatedObj.GetComponent<Transform>().position);

            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            //offsetAO = associatedObj.GetComponent<Transform>().position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPointAO.z));
        }
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            transform.position = curPosition;

            //Vector3 curScreenPointAO = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPointAO.z);
            //Vector3 curScreenPointAO = new Vector3(Input.mousePosition.x, screenPointAO.y, screenPointAO.z);
            //Vector3 curPositionAO = Camera.main.ScreenToWorldPoint(curScreenPointAO) + offsetAO;
            
            // also translate the Target balls
            associatedObj.transform.position = curPosition;
            
            // We want to check if any changes were made
            dragDetected = true;
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
