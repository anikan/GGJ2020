using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float accelerationSpeed = 1.0f;
    public float drag = 0.5f;

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

    public int money = 0;

    private Interactable currentlyUsingInteractable;

    private Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentlyUsingInteractable || !currentlyUsingInteractable.stopsPlayerMovement)
        {
            HandleMovement();
        }

        //Cap at max velocity.
        /*
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
        */

        if (Input.GetKeyDown(KeyCode.E))
        {
            //If not holding anything and there's an interactable object in front, use it.
            if (!grabbedObject && interactZone.TryGetComponent<InteractZone>(out InteractZone zoneComponent))
            {
                Interactable interactableObject = zoneComponent.activeInteractableObject;
                if (interactableObject && (!currentlyUsingInteractable || currentlyUsingInteractable != interactableObject))
                {
                    //If the object is grabbable, set it's parent to the interact zone and reset its position.
                    if (interactableObject.isGrabbable)
                    {
                        GrabObject(interactableObject.gameObject);
                    }

                    if (currentlyUsingInteractable)
                    {
                        currentlyUsingInteractable.OnStopUsing(this);
                    }

                    currentlyUsingInteractable = zoneComponent.activeInteractableObject;
                    currentlyUsingInteractable.OnUse(this);
                }
                else if (currentlyUsingInteractable)
                {
                    currentlyUsingInteractable.OnStopUsing(this);
                    currentlyUsingInteractable = null;
                }
            }

            //If holding something, let go.
            else if (grabbedObject)
            {
                //If this is a block, try to place it.
                if (grabbedObject.GetComponent<UndeployedBlock>())
                {
                    if (manager.IsPositionAvailable(grabbedObject.transform.position))
                    {
                        GameObject prefab = grabbedObject.GetComponent<UndeployedBlock>().blockPrefab;
                        GameObject gameObj = Instantiate(prefab);
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

                //If holding a resource over a block, try to repair if it needs it.
                if (grabbedObject.TryGetComponent<Resource>(out Resource resource) && 
                    interactZone.GetComponent<InteractZone>().activeInteractableObject && 
                    interactZone.GetComponent<InteractZone>().activeInteractableObject.TryGetComponent<Block>(out Block activeBlock))
                {
                    if (activeBlock.hp < activeBlock.maxHP)
                    {
                        activeBlock.OnRepair();

                        Destroy(grabbedObject);
                        grabbedObject = null;
                        return;
                    }
                }

                LetGoOfObject();
            }
        }
    }

    private void HandleMovement()
    {
        float changeInVelocity = accelerationSpeed * Time.deltaTime;

        if (Input.GetAxis("Vertical") != 0)
        {
            // rigidbody.AddForce(new Vector2(0, Input.GetAxis("Vertical")), ForceMode2D.Impulse);
            if (Input.GetAxis("Vertical") > 0.1)
            {
                velocity.y = Mathf.Clamp(velocity.y + changeInVelocity, -maxSpeed, maxSpeed);
                interactZone.transform.localPosition = new Vector3(0, interactZoneOffset, 0);
            }

            else if (Input.GetAxis("Vertical") < -0.1f)
            {
                velocity.y = Mathf.Clamp(velocity.y - changeInVelocity, -maxSpeed, maxSpeed);
                interactZone.transform.localPosition = new Vector3(0, -interactZoneOffset, 0);
            }
            else
            {
                {
                    if (velocity.x > 0)
                    {
                        velocity.x = Mathf.Clamp(velocity.x - changeInVelocity, 0.0f, velocity.x);
                    }
                    else if (velocity.x < 0)
                    {
                        velocity.x = Mathf.Clamp(velocity.x + changeInVelocity, velocity.x, 0.0f);
                    }
                }
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            //rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0), ForceMode2D.Impulse);

            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                velocity.x = Mathf.Clamp(velocity.x + changeInVelocity, -maxSpeed, maxSpeed);
                interactZone.transform.localPosition = new Vector3(interactZoneOffset, 0, 0);
            }

            else if (Input.GetAxis("Horizontal") < -0.1f)
            {
                velocity.x = Mathf.Clamp(velocity.x - changeInVelocity, -maxSpeed, maxSpeed);
                interactZone.transform.localPosition = new Vector3(-interactZoneOffset, 0, 0);
            }
        }
        else
        {
            if (velocity.x > 0)
            {
                velocity.x = Mathf.Clamp(velocity.x - changeInVelocity, 0.0f, velocity.x);
            }
            else if (velocity.x < 0)
            {
                velocity.x = Mathf.Clamp(velocity.x + changeInVelocity, velocity.x, 0.0f);
            }
        }

        velocity += -velocity * drag * Time.deltaTime;

    }

    private void FixedUpdate()
    {

        if (velocity != Vector2.zero)
        {
            transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime);
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
