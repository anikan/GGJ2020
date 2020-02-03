using UnityEngine;

using TMPro;

public class TopDownLazyFollow : MonoBehaviour
{
    public static TopDownLazyFollow gameCamera;

    public float startingYValue = 0.0f;
    public Camera waterCamera;

    public Transform boatTransform;
    public Transform playerTransform;
    public float playerViewYOffset = -10.0f;
    public float steeringViewYOffset = -25.0f;
    public float waterCameraStartingDepth = -50.0f;
    public float unityToFakeMeterScale = 0.01f;
    public float winMeters = 10.0f;

    public Transform winConditionParent;
    public TextMeshProUGUI totalTimeMesh;
    public TextMeshProUGUI totalDistanceMesh;

    [HideInInspector]
    public float totalDistanceTraveled, totalTimeElapsed = 0.0f;

    private Vector3 prevBoatPos = Vector3.zero;

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
        if (!playerTransform || !boatTransform)
        {
            playerTransform = BlockPrefabs.instance.playerRef.transform;
            boatTransform = BlockPrefabs.instance.playerRef.boat.transform;
        }
        startingYValue = boatTransform.position.y;
        prevBoatPos = boatTransform.position;
 
        winConditionParent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GetDistanceTraveled() > winMeters) {
            OnWinCondition();
        }

        Vector3 boatPos = boatTransform.position;
        float movementSinceLastFrame = Vector3.Distance(boatPos, prevBoatPos);
        totalDistanceTraveled += (unityToFakeMeterScale * movementSinceLastFrame);
        prevBoatPos = boatPos;

        totalTimeElapsed += Time.deltaTime;
        int mins = Mathf.FloorToInt(totalTimeElapsed / 60.0f);
        float seconds = totalTimeElapsed % 60.0f;
        string timeString = string.Format("{0}:{1}", mins.ToString(), seconds.ToString("F1"));
        totalTimeMesh.text = "Total Time: " + timeString;
        totalDistanceMesh.text = "Distance Traveled: " + totalDistanceTraveled.ToString("F1") + "km";

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform objectToFollow = isFollowingPlayer ? playerTransform : boatTransform;
        Vector3 targetPosition = objectToFollow.transform.position + (new Vector3(0, 0, isFollowingPlayer? playerViewYOffset : steeringViewYOffset));

        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Vector3 posDelta = newPos - transform.position;

        transform.position = newPos;

        if (!isFollowingPlayer)
        {
            Vector3 waterCameraPosDelta = new Vector3(posDelta.x, -posDelta.z, posDelta.y);
            waterCamera.transform.position += waterCameraPosDelta;
        }
        else
        {
            Vector3 waterCameraPosDelta = new Vector3(0.0f, -posDelta.z, 0.0f);
            waterCamera.transform.position += waterCameraPosDelta;

        }


    }

    public float GetDistanceTraveled()
    {
        return unityToFakeMeterScale * (boatTransform.position.y - startingYValue);
    }

    public void ZoomToPlayerView()
    {
        isFollowingPlayer = true;
    }

    public void ZoomToBoatView()
    {
        isFollowingPlayer = false;
    }

    public void OnWinCondition()
    {
        winConditionParent.gameObject.SetActive(true);
    }

    public float GetDistanceLeft()
    {
        return 10.0f - GetDistanceTraveled(); 
    }
}
