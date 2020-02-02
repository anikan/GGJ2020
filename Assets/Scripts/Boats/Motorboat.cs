using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorboat : Boat
{
    public float maxMotorAngle = 45.0f;
    public float motorRotateSpeed = 15.0f;

    private float currentRotationAngle = 0.0f;
    public ParticleSystem motorParticleSystem;
    public float maxParticleLifetime = 0.3f;
    public float particleRampSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.W))
        {
            ApplyForwardForce(-transform.up, transform.position);
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

    private void RotateMotorByDegrees(float degrees)
    {
        float newRotation = currentRotationAngle + degrees;
        if (newRotation > maxMotorAngle || newRotation < -maxMotorAngle)
        {
            degrees = newRotation < 0 ? -maxMotorAngle - currentRotationAngle : maxMotorAngle - currentRotationAngle;
        }
        transform.Rotate(Vector3.forward, degrees);
        currentRotationAngle += degrees;
    }
}
