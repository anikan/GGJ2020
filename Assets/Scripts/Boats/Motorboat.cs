using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorboat : BoatPart
{
    public Transform motorModelTransform;

    public float maxMotorAngle = 45.0f;
    public float motorRotateSpeed = 15.0f;

    private float currentRotationAngle = 0.0f;
    public ParticleSystem motorParticleSystem;
    public float maxParticleLifetime = 0.3f;
    public float particleRampSpeed = 0.2f;

    protected override void CheckInputsAndSteer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ApplyForwardForce(-motorModelTransform.up, motorModelTransform.position);
            ParticleSystem.MainModule particleModule = motorParticleSystem.main;
            float newLifetime = particleModule.startLifetime.constant + particleRampSpeed * Time.deltaTime;
            particleModule.startLifetime = new ParticleSystem.MinMaxCurve(Mathf.Clamp(newLifetime, 0.0f, maxParticleLifetime));
        }
        else
        {
            ParticleSystem.MainModule particleModule = motorParticleSystem.main;
            float newLifetime = particleModule.startLifetime.constant - (particleRampSpeed * Time.deltaTime);
            particleModule.startLifetime = new ParticleSystem.MinMaxCurve(Mathf.Clamp(newLifetime, 0.0f, maxParticleLifetime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            float angle = -motorRotateSpeed * Time.deltaTime;
            RotateMotorByDegrees(angle);
        }
        if (Input.GetKey(KeyCode.D))
        {
            float angle = motorRotateSpeed * Time.deltaTime;
            RotateMotorByDegrees(angle);
        }
    }

    protected override void Update()
    {
        base.Update();
        ParticleSystem.MainModule particleModule = motorParticleSystem.main;

        if (!activelyBeingUsed && !SteeringBlock.instance.activelySteering)
        {
            float newLifetime = particleModule.startLifetime.constant - particleRampSpeed * Time.deltaTime;
            particleModule.startLifetime = new ParticleSystem.MinMaxCurve(Mathf.Clamp(newLifetime, 0.0f, maxParticleLifetime));
        }
    }

    private void RotateMotorByDegrees(float degrees)
    {
        float newRotation = currentRotationAngle + degrees;
        if (newRotation > maxMotorAngle || newRotation < -maxMotorAngle)
        {
            degrees = newRotation < 0 ? -maxMotorAngle - currentRotationAngle : maxMotorAngle - currentRotationAngle;
        }
        motorModelTransform.Rotate(Vector3.forward, degrees);
        currentRotationAngle += degrees;
    }
}
