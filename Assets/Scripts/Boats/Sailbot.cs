using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sailbot : Boat
{
    public Transform sailTransform;
    public float sailRotateSpeed = 30.0f;

    protected override void Update()
    {
        base.Update();

        float angleToRotate = sailRotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            sailTransform.Rotate(Vector3.forward, angleToRotate);
        }

        if (Input.GetKey(KeyCode.D))
        {
            sailTransform.Rotate(Vector3.forward, -angleToRotate);

        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        forwardForce = Mathf.Clamp(Vector3.Dot(sailTransform.up, WindController.instance.currentWind), 0.0f, maxVelocity);
        ApplyForwardForce(sailTransform.up, sailTransform.position);
    }
}
