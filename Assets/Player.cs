using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float accelerationSpeed = 1.0f;

    [SerializeField]
    private GameObject grabZone;

    [SerializeField]
    private float grabZoneOffset;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool inputPressed = false;
        if (Input.GetAxis("Vertical") != 0)
        {
            inputPressed = true;
            rigidbody.AddForce(new Vector2(0, Input.GetAxis("Vertical")), ForceMode2D.Impulse);

            if (Input.GetAxis("Vertical") > 0.0)
            {
                grabZone.transform.localPosition = new Vector3(0, grabZoneOffset, 0);
            }

            else
            {
                grabZone.transform.localPosition = new Vector3(0, -grabZoneOffset, 0);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            inputPressed = true;
            rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0), ForceMode2D.Impulse);

            if (Input.GetAxis("Horizontal") > 0.0)
            {
                grabZone.transform.localPosition = new Vector3(grabZoneOffset, 0, 0);
            }

            else
            {
                grabZone.transform.localPosition = new Vector3(-grabZoneOffset, 0, 0);
            }
        }

        //Cap at max velocity.
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
    }
}
