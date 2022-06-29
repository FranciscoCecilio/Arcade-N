using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEditionManager : MonoBehaviour
{
    public DragDrop[] targets;
    public DragDropX[] bars;

    public void UnLockEdition(){
        for(int i = 0; i < targets.Length; i++){
            targets[i].lockDragDrop(false);
        }
        for(int i = 0; i < bars.Length; i++){
            bars[i].lockDragDrop(false);
        }
    }

    public void LockEdition(){
        for(int i = 0; i < targets.Length; i++){
            targets[i].lockDragDrop(true);
        }
        for(int i = 0; i < bars.Length; i++){
            bars[i].lockDragDrop(true);
        }
    }
}
