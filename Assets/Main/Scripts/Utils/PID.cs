using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PID
{
    // Variables
    private float previousIntegralTerm;
    private float previousDifferentialTerm;
    private float currentOutput;

    public PID()
    {
        Initialize();
    }
    
    public PID(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
        Initialize();
    }

    public void Initialize()
    {
        previousIntegralTerm = 0;
        previousDifferentialTerm = 0;
        currentOutput = 0;
    }

    public float CalculatePID(float tgtVal, float prevVal, float limit)
    {
        float errorVal = tgtVal - prevVal;
        float PTerm = Kp * errorVal;
        float ITerm = Ki * (previousIntegralTerm + errorVal);
        float DTerm = Kd * (previousDifferentialTerm - errorVal);

        previousIntegralTerm = ITerm;
        previousDifferentialTerm = DTerm;

        currentOutput = (PTerm + ITerm + DTerm);
        //Debug.Log("PTerm = " + PTerm);
        //Debug.Log("ITerm = " + ITerm);
        //Debug.Log("DTerm = " + DTerm);
        if (Mathf.Abs(currentOutput) > limit)
            currentOutput = currentOutput < 0.1f ? -limit : limit;
        return currentOutput;
    }

    public float Kp { get; set; }
    public float Ki { get; set; }
    public float Kd { get; set; }
}
