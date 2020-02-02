using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sailbot : BoatPart
{
    public Transform sailTransform;
    public Transform clothTransform;
    public float sailRotateSpeed = 30.0f;
    public bool raised = false;

    public float undeployedSailWidth = 0.13f;
    public float deployedSailWidth = 0.75f;

    private Coroutine deployingRoutine;

    protected override void Start()
    {
        base.Start();
        UndeploySail();
    }

    protected override void CheckInputsAndSteer()
    {
        float angleToRotate = sailRotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            sailTransform.Rotate(Vector3.forward, angleToRotate);
        }

        if (Input.GetKey(KeyCode.D))
        {
            sailTransform.Rotate(Vector3.forward, -angleToRotate);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            DeploySail();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UndeploySail();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (raised)
        {
            forwardForce = Mathf.Clamp(Vector3.Dot(sailTransform.up, WindController.instance.currentWind), 0.0f, maxVelocity);
            ApplyForwardForce(sailTransform.up, sailTransform.position);
        }

    }

    protected void DeploySail()
    {
        if (deployingRoutine != null)
        {
            StopCoroutine(deployingRoutine);
        }
        deployingRoutine = StartCoroutine(DeploySail(deployedSailWidth, 0.3f));
        raised = true;

    }

    protected void UndeploySail()
    {
        if (deployingRoutine != null)
        {
            StopCoroutine(deployingRoutine);
        }
        deployingRoutine = StartCoroutine(DeploySail(undeployedSailWidth, 0.3f));
        raised = false;

    }

    protected IEnumerator DeploySail(float targetWidth, float duration)
    {
        float startScale = clothTransform.localScale.x;
        for (float i = 0; i < duration; i+=Time.deltaTime)
        {
            float newWidth = Mathf.Lerp(startScale, targetWidth, i / duration);
            clothTransform.localScale = new Vector3(newWidth, clothTransform.localScale.y, clothTransform.localScale.z);
            yield return null;
        }
    }
}
