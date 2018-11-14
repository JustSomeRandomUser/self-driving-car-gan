using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Htw.SelfDriving.GAN.Gym;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float rayMaxDistance;
    public SensorProcessor sensorProcessor;
    public Agent neuralController;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Awake()
    {
        this.neuralController = GetComponent<Agent>();
    }

    public void FixedUpdate()
    {
        List<float> input = new List<float>();

        foreach(float data in sensorProcessor.Update())
            input.Add(data);

        List<float> output = neuralController.Evaluate(input);

        float motor = maxMotorTorque * output[0];
        float steering = maxSteeringAngle * output[1];

        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}
