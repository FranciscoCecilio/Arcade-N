using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class State {
    public static Exercise exercise;
    
    public static int sessionTimeInt;

    public static int currentTarget;
    
    public static int restCount;

    public static bool leftArmSelected = true;
    public static bool hasSecondaryCursor;

    public static bool isTherapyOnGoing;
    public static bool isRestingTime;
    public static bool hasStartedExercise;

    public static bool registerShoulderComp = false;
    public static bool registerSpineComp = false;

    //public static Vector3[] targetPositions;

    public static bool compensationInCurrentRep;
    
    public static string space = "Social";
    public static string heatM = "none";
    public static string touch = "none";

    public static void resetState() {
        isRestingTime = false;
        isTherapyOnGoing = false;
        compensationInCurrentRep = false;
        hasStartedExercise = false;
        hasSecondaryCursor = false;
        restCount = 0;
        heatM = "none";
        //registerShoulderComp = false;
        //registerSpineComp = false;
    }

    // This resets the State because we changed Project Settings > Editor > Enter Play Mode > (disable) Domain Reload
    // If Domain Reload was checked all static values would reset BUT sometimes we would have an infinite "Application.Reload" on entering play mode
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        resetState();
        exercise = null;
        registerShoulderComp = false;
        registerSpineComp = false;
        currentTarget = 0;
        sessionTimeInt = 0;
        space = "Social";
        heatM = "none";
        touch = "none";
        Debug.Log("State reset.");
    }
}
