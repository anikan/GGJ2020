using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoatPart : Block
{
    [Space(10)]
    [Header("Boat Config")]

    public Rigidbody2D controllingRigidbody;

    public float maxVelocity = 50.0f;
    public float forwardForce = 1.0f;

    [HideInInspector]
    public bool activelyBeingUsed;
    public bool allowControlFromWheel = true;


public override void OnUse(Player player)
    {
        base.OnUse(player);
        activelyBeingUsed = true;
        TopDownLazyFollow.gameCamera.ZoomToBoatView();
        player.GetComponentInChildren<Collider2D>().enabled = false;
    }

    public override void OnStopUsing(Player player)
    {
        base.OnStopUsing(player);
        activelyBeingUsed = false;
        TopDownLazyFollow.gameCamera.ZoomToPlayerView();
        player.GetComponentInChildren<Collider2D>().enabled = true;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!controllingRigidbody)
        {
            controllingRigidbody = GetComponentInChildren<Rigidbody2D>();
        }
        stopsPlayerMovement = true;
    }

    public override void OnAttach(Transform blockParent)
    {
        base.OnAttach(blockParent);
        controllingRigidbody = blockParent.GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (SteeringBlock.activelySteering || activelyBeingUsed)
        {
            CheckInputsAndSteer();
        }
    }


    protected abstract void CheckInputsAndSteer();
   

    protected virtual void FixedUpdate()
    {
        PreventSliding();
    }

    protected void ApplyForwardForce(Vector3 dir, Vector3 pos)
    {
        controllingRigidbody.AddForceAtPosition(dir * forwardForce, pos);
        Debug.DrawRay(pos, dir * forwardForce);
        PreventSliding();
    }

    private void PreventSliding()
    {
        Vector2 boatForward = controllingRigidbody.transform.up;
        Vector2 currentVelocity = controllingRigidbody.velocity;
        //controllingRigidbody.velocity = boatForward * Vector2.Dot(currentVelocity, boatForward);
    }
}
