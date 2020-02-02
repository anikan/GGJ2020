using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownLazyFollow : MonoBehaviour
{
    public Transform boatTransform;
    public Transform playerTransform;
    public float playerViewYOffset = -10.0f;
    public float steeringViewYOffset = -25.0f;

    public float smoothTime;

    private Vector3 velocity;

    public bool isFollowingPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform objectToFollow = isFollowingPlayer ? playerTransform : boatTransform;
        Vector3 targetPosition = objectToFollow.transform.position + (new Vector3(0, 0, isFollowingPlayer? playerViewYOffset : steeringViewYOffset));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ZoomToPlayerView()
    {
        isFollowingPlayer = true;
    }

    public void ZoomToBoatView()
    {
        isFollowingPlayer = false;
    }
}
