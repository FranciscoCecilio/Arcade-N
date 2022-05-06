using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// This allows to move and reorder Sequence buttons inside the list 
public class DraggableComponent : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform currentTransform;
    public CanvasGroup canvasGroup; // to change alpha when selected
    private GameObject listContent; // parent of list Elements
    private Vector3 currentPosition;

    private int totalChild;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPosition = currentTransform.position;
        listContent = currentTransform.parent.gameObject;
        totalChild = listContent.transform.childCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        currentTransform.position =
            new Vector3(currentTransform.position.x, eventData.position.y, currentTransform.position.z);

        for (int i = 0; i < totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                Transform otherTransform = listContent.transform.GetChild(i);
                int distance = (int) Vector3.Distance(currentTransform.position,
                    otherTransform.position);
                if (distance <= 10)
                {
                    Vector3 otherTransformOldPosition = otherTransform.position;
                    otherTransform.position = new Vector3(otherTransform.position.x, currentPosition.y,
                        otherTransform.position.z);
                    currentTransform.position = new Vector3(currentTransform.position.x, otherTransformOldPosition.y,
                        currentTransform.position.z);
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    currentPosition = currentTransform.position;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        currentTransform.position = currentPosition;
    }
}