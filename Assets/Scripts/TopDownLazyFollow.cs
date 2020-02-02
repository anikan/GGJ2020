using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownLazyFollow : MonoBehaviour
{
    public static TopDownLazyFollow gameCamera;
    public Camera waterCamera;

    public Transform boatTransform;
    public Transform playerTransform;
    public float playerViewYOffset = -10.0f;
    public float steeringViewYOffset = -25.0f;
    public float waterCameraStartingDepth = -50.0f;

    public float smoothTime;

    private Vector3 velocity;

    public bool isFollowingPlayer = true;

    private void Awake()
    {
        gameCamera = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform objectToFollow = isFollowingPlayer ? playerTransform : boatTransform;
        Vector3 targetPosition = objectToFollow.transform.position + (new Vector3(0, 0, isFollowingPlayer? playerViewYOffset : steeringViewYOffset));

        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Vector3 posDelta = newPos - transform.position;

        transform.position = newPos;
        Vector3 waterCameraPosDelta = new Vector3(posDelta.x, -posDelta.z, 0.0f);
        waterCamera.transform.position += waterCameraPosDelta;
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
