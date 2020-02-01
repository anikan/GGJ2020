using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBoat : Boat
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
          //  ApplyForwardForce();
            controllingRigidbody.angularVelocity = 0.0f;
            controllingRigidbody.AddTorque(sideForce);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
           // ApplyForwardForce();
            controllingRigidbody.angularVelocity = 0.0f;
            controllingRigidbody.AddTorque(-sideForce);
        }
    }
}
