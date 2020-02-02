using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalBoat : BoatPart
{
    public Transform leftPedal;
    public Transform rightPedal;
    public float depressedOffset = 0.03f;
    private float regularZValue;

    protected override void Start()
    {
        regularZValue = rightPedal.position.z;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            ApplyForwardForce(leftPedal.transform.up + -leftPedal.transform.right, leftPedal.transform.position);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            ApplyForwardForce(rightPedal.transform.up + rightPedal.transform.right, rightPedal.transform.position);
        }

        if (Input.GetKey(KeyCode.A))
        {
            leftPedal.position = new Vector3(leftPedal.position.x, leftPedal.position.y, regularZValue + depressedOffset);
        }
        else
        {
            leftPedal.position = new Vector3(leftPedal.position.x, leftPedal.position.y, regularZValue);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightPedal.position = new Vector3(rightPedal.position.x, rightPedal.position.y, regularZValue + depressedOffset);

        }
        else
        {
            rightPedal.position = new Vector3(rightPedal.position.x, rightPedal.transform.position.y, regularZValue);

        }
    }
}
