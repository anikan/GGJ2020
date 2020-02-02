﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sailbot : BoatPart
{
    public Transform sailTransform;
    public float sailRotateSpeed = 30.0f;
    public bool raised = false;

    protected override void CheckInputsAndSteer()
    {
        float angleToRotate = sailRotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            sailTransform.Rotate(Vector3.forward, angleToRotate);
        }

        if (Input.GetKey(KeyCode.D))
        {
            sailTransform.Rotate(Vector3.forward, -angleToRotate);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            raised = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            raised = false;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (raised)
        {
            forwardForce = Mathf.Clamp(Vector3.Dot(sailTransform.up, WindController.instance.currentWind), 0.0f, maxVelocity);
            ApplyForwardForce(sailTransform.up, sailTransform.position);
        }

    }
}
