using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Rigidbody2D controllingRigidbody;
    public float maxVelocity = 50.0f;
    public float forwardForce = 1.0f;
    public float sideForce = 2.0f;

    public Transform forceSourceAnchor;
    public bool invertForward;

    // Start is called before the first frame update
    void Start()
    {
        if (!controllingRigidbody)
        {
            controllingRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    protected void ApplyForwardForce(Transform sourceAnchor, bool invertForward)
    {
        this.forceSourceAnchor = sourceAnchor;
        this.invertForward = invertForward;
        Vector3 dir = invertForward ? -sourceAnchor.up : sourceAnchor.up;
        controllingRigidbody.AddForceAtPosition(dir * forwardForce, sourceAnchor.position);
    }

    protected void TurnLeft()
    {
        controllingRigidbody.angularVelocity = 0.0f;
        controllingRigidbody.AddTorque(sideForce, ForceMode2D.Impulse);
    }

    protected void TurnRight()
    {
        controllingRigidbody.angularVelocity = 0.0f;
        controllingRigidbody.AddTorque(-sideForce, ForceMode2D.Impulse);
    }

    private void PreventSliding()
    {
        Vector3 forwardDir = invertForward ? -forceSourceAnchor.up : forceSourceAnchor.up;
        controllingRigidbody.velocity *= forwardDir;
    }
}
