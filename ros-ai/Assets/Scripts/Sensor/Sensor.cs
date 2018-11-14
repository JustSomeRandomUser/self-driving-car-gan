using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool Echo(float distance, out RaycastHit hit)
    {
        RaycastHit rayHit;
        Ray ray = new Ray(transform.position, transform.forward);
        bool isHit = Physics.Raycast(ray, out rayHit, distance);
        hit = rayHit;

        #if UNITY_EDITOR
            if(isHit)
                Debug.DrawRay(transform.position, transform.forward * distance, Color.red, 0.05f);
            else
                Debug.DrawRay(transform.position, transform.forward * distance, Color.blue, 0.05f);
        #endif

        if(isHit)
            return true;

        return false;
    }
}
