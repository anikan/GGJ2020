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

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Shark Behavior:
    // Aims for the ship, but can only move in circles
    // When it collides, it bounces off the ship and tries to come back again
    void FixedUpdate()
    {
        // Target velocity is something that's perpendicular to the circle
        Vector2 toTarget = target.position - this.transform.position;

        // Get direction of force, which is perpendicular to direction of the vector to target
        Vector2 force = new Vector2(toTarget.y, -toTarget.x);

        // Calculate the magnitude of the force according to the new radius we want
        float newRadius = Mathf.Max(toTarget.magnitude, 0.01f);
        float forceMagnitude = (sharkSpeed * sharkSpeed) / newRadius;

        rb.AddForce(force * forceMagnitude);
    }
}
