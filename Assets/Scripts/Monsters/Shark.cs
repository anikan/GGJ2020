using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Shark : Damaging
{
    public Transform target = null; // target the shark is trying to hit
    public float sharkSpeed = 10.0f; // speed of the shark
    private Rigidbody2D rb = null;
    private float bouncing = 0.0f;
    private float transition = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector2(sharkSpeed, 0);
        target = GameObject.FindObjectOfType<BlockManager>().transform;

        Debug.Assert(target != null);
    }

    // Shark Behavior:
    // Aims for the ship, but can only move in circles
    // When it collides, it bounces off the ship and tries to come back again
    void FixedUpdate()
    {
        // Target velocity is something that's perpendicular to the circle
        Vector2 force = target.position - this.transform.position;

        if(force.magnitude > 10.0f)
        {
            return;
        }

        // Calculate the magnitude of the force according to the new radius we want
        float newRadius = Mathf.Max(force.magnitude, 0.01f);
        float forceMagnitude = (sharkSpeed * sharkSpeed) / newRadius;

        rb.AddForce(force * forceMagnitude);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // Always bounce yourself away
        if (collision.GetComponentInParent<BlockManager>() != null)
            bouncing = 0.3f;
        else
            bouncing = 1.0f;

        Vector2 appliedForce = Vector3.Normalize(this.rb.velocity) * -30 * stoppingForce;
        rb.AddForce(appliedForce);
    }
}
