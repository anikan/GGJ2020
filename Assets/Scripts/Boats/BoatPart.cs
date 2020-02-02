using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatPart : MonoBehaviour
{
    public Rigidbody2D controllingRigidbody;
    public Transform boatBodyTransform;

    public float maxVelocity = 50.0f;
    public float forwardForce = 1.0f;

    // Start is called before the first frame update
    protected virtual void Start()
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

    private void PreventSliding()
    {
        Vector2 boatForward = boatBodyTransform.up;
        Vector2 currentVelocity = controllingRigidbody.velocity;
        controllingRigidbody.velocity = boatForward * Vector2.Dot(currentVelocity, boatForward);
    }
}
