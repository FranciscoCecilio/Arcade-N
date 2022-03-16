using UnityEngine;
using System.Collections;
using System.Reflection;

/*
 * DESCRIPTION
 * 
 * This script will associate the positions and orientations to the respective GameObjects, this is, the joints 
 * of the skeleton.
 * 
 * Two ways were used to associate the positions and orientations: for the first, the KinectData is attached and 
 * it will take the position by comparing the name of this skeleton joint with the variables of KinectData; for the 
 * second, it is also based on comparation but instead of the JointSkeleton's name is with a string that we have to 
 * write.
 * 
 * This script is attached to all the joints in Skeleton.
 */

public class Movement : MonoBehaviour {
    public BodyWrapper position;

    private string gameObjectName;

    private Vector3 jointPosition;

    void Start() {
        gameObjectName = gameObject.name;
    }

    void Update() {
        jointPosition = (Vector3)position.GetType().GetField(gameObjectName).GetValue(position);
        transform.position = jointPosition;
    }
}
