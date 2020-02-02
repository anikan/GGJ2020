using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Rigidbody2D controllingRigidbody;
    public Transform boatBodyTransform;

    public float maxVelocity = 50.0f;
    public float forwardForce = 1.0f;
    public float sideForce = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!controllingRigidbody)
        {
            controllingRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        PreventSliding();
    }

    protected void ApplyForwardForce(Vector3 dir, Vector3 pos)
    {
        controllingRigidbody.AddForceAtPosition(dir * forwardForce, pos);
        PreventSliding();
    }

    protected void TurnLeft()
    {
        controllingRigidbody.angularVelocity = 0.0f;
        controllingRigidbody.AddTorque(sideForce, ForceMode2D.Impulse);
        PreventSliding();
    }

    protected void TurnRight()
    {
        controllingRigidbody.angularVelocity = 0.0f;
        controllingRigidbody.AddTorque(-sideForce, ForceMode2D.Impulse);
        PreventSliding();
    }

    private void PreventSliding()
    {
        Vector2 boatForward = boatBodyTransform.up;
        Vector2 currentVelocity = controllingRigidbody.velocity;
        controllingRigidbody.velocity = boatForward * Vector2.Dot(currentVelocity, boatForward);
    }
}
