using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float accelerationSpeed = 1.0f;

    [SerializeField]
    private GameObject interactZone;

    [SerializeField]
    private float interactZoneOffset;

    private Rigidbody2D rigidbody;

    private GameObject grabbedObject;

    [SerializeField]
    private GameObject boat;

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
                interactZone.transform.localPosition = new Vector3(0, interactZoneOffset, 0);
            }

            else
            {
                interactZone.transform.localPosition = new Vector3(0, -interactZoneOffset, 0);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            inputPressed = true;
            rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0), ForceMode2D.Impulse);

            if (Input.GetAxis("Horizontal") > 0.0)
            {
                interactZone.transform.localPosition = new Vector3(interactZoneOffset, 0, 0);
            }

            else
            {
                interactZone.transform.localPosition = new Vector3(-interactZoneOffset, 0, 0);
            }
        }

        //Cap at max velocity.
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            //If not holding anything and there's an interactable object in front, use it.
            if (!grabbedObject && interactZone.GetComponent<InteractZone>().activeInteractableObject)
            {
                Interactable interactableObject = interactZone.GetComponent<InteractZone>().activeInteractableObject;
                if (interactableObject.isGrabbable)
                {
                    grabbedObject = interactableObject.gameObject;
                    grabbedObject.transform.parent = this.transform;
                }

                interactZone.GetComponent<InteractZone>().activeInteractableObject.OnUse();
            }

            //If holding something.
            else if (grabbedObject)
            {
                grabbedObject.transform.parent = boat.transform;
            }
        }
    }
}
