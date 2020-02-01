using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBoat : MonoBehaviour
{

    public float maxVelocity;

    public float forwardForce = 1.0f;
    public float sideForce = 5.0f;

    public float deceleration = 0.1f;

    private Rigidbody2D boatBody;

    // Start is called before the first frame update
    void Start()
    {
        boatBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            boatBody.AddForce(new Vector2(-sideForce, forwardForce));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            boatBody.AddForce(new Vector2(sideForce, forwardForce));
        }

        SlowVelocity();
    }

    public void SlowVelocity()
    {
        boatBody.velocity /= 1.0f + (deceleration * Time.deltaTime);
        float xVelocity = Mathf.Clamp(boatBody.velocity.x, -maxVelocity, maxVelocity);
        float yVelocity = Mathf.Clamp(boatBody.velocity.y, -maxVelocity, maxVelocity);
        boatBody.velocity = new Vector2(xVelocity, yVelocity);

    }
}
