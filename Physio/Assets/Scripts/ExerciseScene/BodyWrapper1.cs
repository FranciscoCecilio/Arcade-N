using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Francisco Cecílio

public class BodyWrapper1 : MonoBehaviour
{
    [SerializeField]
    private Transform _leftElbowPos;
    [SerializeField]
    private  Transform _leftShoulderPos;
    [SerializeField]
    private  Transform _leftWristPos;

    [SerializeField]
    private  Transform _rightElbowPos;
    [SerializeField]
    private  Transform _rightShoulderPos;
    [SerializeField]
    private  Transform _rightWristPos;

    [SerializeField]
    private  Transform _spineShoulderPos;
    [SerializeField]
    private  Transform _spineBasePos;

    // Getters

    public Vector3 leftElbowPos
    {
        get {return _leftElbowPos.position; }
    }
    public Vector3 leftShoulderPos
    {
        get {return _leftShoulderPos.position; }
    }
    public Vector3 leftWristPos
    {
        get {return _leftWristPos.position; }
    }

    public Vector3 rightElbowPos
    {
        get {return _rightElbowPos.position; }
    }
    public Vector3 rightShoulderPos
    {
        get {return _rightShoulderPos.position; }
    }
    public Vector3 rightWristPos
    {
        get {return _rightWristPos.position; }
    }

    public Vector3 spineShoulderPos
    {
        get {return _spineShoulderPos.position; }
    }
    public Vector3 spineBasePos
    {
        get {return _spineBasePos.position; }
    }
}
