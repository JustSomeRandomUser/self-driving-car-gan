using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorProcessor : MonoBehaviour
{
    [SerializeField]
    private Sensor[] sensors;
    public Sensor[] Sensors
    {
        get { return sensors; }
        set { sensors = value; }
    }

    [SerializeField]
    private float distance;
    public float Distance
    {
        get { return distance; }
        set { distance = value; }
    }

    public List<float> Update()
    {
        List<float> data = new List<float>();

        foreach(Sensor sensor in sensors)
        {
            RaycastHit hit;

            if(sensor.Echo(distance, out hit))
                data.Add(hit.distance / distance);
            else
                data.Add(0f);
        }

        return data;
    }
}
