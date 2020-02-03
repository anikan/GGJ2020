using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBlock : Block
{
    public static SteeringBlock instance;
    public bool activelySteering = false;

    protected virtual void Awake()
    {
        if (instance)
        {
            GameObject.Destroy(instance);
        }
        instance = this;
    }

    protected virtual void Start()
    {
        stopsPlayerMovement = true;
    }

    public override void OnUse(Player player)
    {
        base.OnUse(player);
        TopDownLazyFollow.gameCamera.ZoomToBoatView();
        player.GetComponentInChildren<Collider2D>().enabled = false;
        activelySteering = true;
    }

    public override void OnStopUsing(Player player)
    {
        base.OnStopUsing(player);
        TopDownLazyFollow.gameCamera.ZoomToPlayerView();
        player.GetComponentInChildren<Collider2D>().enabled = true;
        activelySteering = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
