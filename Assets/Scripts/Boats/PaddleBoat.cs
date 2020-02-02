using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBoat : BoatPart
{
    public Transform leftPaddle;
    public Transform rightPaddle;
    public Transform leftTip;
    public Transform rightTip;

    public float paddleRotateSpeed = 80.0f;
    public float maxPaddleAngle = 45.0f;
    private float currentLeftRotation = 0.0f;
    private float currentRightRotation = 0.0f;

    private Coroutine leftRoutine;
    private Coroutine rightRoutine;

    protected override void CheckInputsAndSteer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            float angle = paddleRotateSpeed * Time.deltaTime;
            RotatePaddleByDegrees(leftPaddle, angle);
            if (currentLeftRotation < maxPaddleAngle - 1.0f)
            {
                ApplyForwardForce(leftTip.transform.up, leftTip.transform.position);
            }
        }
        else
        {
            float angle = -paddleRotateSpeed * Time.deltaTime;
            RotatePaddleByDegrees(leftPaddle, angle);
        }

        if (Input.GetKey(KeyCode.D))
        {
            float angle = -paddleRotateSpeed * Time.deltaTime;
            RotatePaddleByDegrees(rightPaddle, angle);
            if (currentRightRotation > -maxPaddleAngle + 1.0f)
            {
                ApplyForwardForce(rightTip.transform.up, rightTip.transform.position);
            }
        }
        else
        {
            float angle = paddleRotateSpeed * Time.deltaTime;
            RotatePaddleByDegrees(rightPaddle, angle);
        }
    }

    private void RotatePaddleByDegrees(Transform paddle, float degrees)
    {
        float currentRotationAngle = paddle == rightPaddle ? currentRightRotation : currentLeftRotation;
        float newRotation = currentRotationAngle + degrees;
        if (newRotation > maxPaddleAngle || newRotation < -maxPaddleAngle)
        {
            degrees = newRotation < 0 ? -maxPaddleAngle - currentRotationAngle : maxPaddleAngle - currentRotationAngle;
        }
        paddle.transform.Rotate(Vector3.forward, degrees);

        if (paddle == rightPaddle)
        {
            currentRightRotation += degrees;
        }
        else
        {
            currentLeftRotation += degrees;
        }
    }
}
