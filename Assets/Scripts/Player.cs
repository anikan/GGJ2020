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

    [SerializeField]
    private BlockManager manager;

    [SerializeField]
    private GameObject blockPrefab;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            //If not holding anything and there's an interactable object in front, use it.
            if (!grabbedObject && interactZone.GetComponent<InteractZone>().activeInteractableObject)
            {
                
                Interactable interactableObject = interactZone.GetComponent<InteractZone>().activeInteractableObject;

                //If the object is grabbable, set it's parent to the interact zone and reset its position.
                if (interactableObject.isGrabbable)
                {
                    GrabObject(interactableObject.gameObject);
                }

                interactZone.GetComponent<InteractZone>().activeInteractableObject.OnUse(this);
            }

            //If holding something, let go.
            else if (grabbedObject)
            {
                //If this is a block, try to place it.
                if (grabbedObject.GetComponent<Block>())
                {
                    if (manager.IsPositionAvailable(grabbedObject.transform.position))
                    {
                        GameObject gameObj = Instantiate(blockPrefab);
                        Block block = gameObj.GetComponent<Block>();
                        bool successful = manager.AddBlock(block, grabbedObject.transform.position);
                        if (!successful)
                            Destroy(gameObj);
                        else
                        {
                            Destroy(grabbedObject);
                            grabbedObject = null;
                            return;
                        }
                    }
                }

                LetGoOfObject();
            }
        }
    }

    public void GrabObject(GameObject interactableObject)
    {
        grabbedObject = interactableObject;
        grabbedObject.transform.parent = interactZone.transform;
        grabbedObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void LetGoOfObject()
    {
        grabbedObject.transform.parent = boat.transform;
        grabbedObject = null;
    }
}
