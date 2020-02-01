using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownLazyFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public float smoothTime;
    public float yOffset = -20f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = objectToFollow.transform.position + (new Vector3(0, 0, yOffset));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
