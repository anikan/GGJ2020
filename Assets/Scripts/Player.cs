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

    private GameObject grabbedObject;

    [SerializeField]
    public GameObject boat;

    [SerializeField]
    private BlockManager manager;

    public int money = 0;

    public Interactable currentlyUsingInteractable;

    private Vector2 velocity;

    private Vector2 facingDirection;

    public GameObject newBlockIndicator;

    public Animator spriteAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Facing right initially
        facingDirection = new Vector2(1.0f, 0.0f);
        newBlockIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentlyUsingInteractable || !currentlyUsingInteractable.stopsPlayerMovement)
        {
            HandleMovement();
        }

        else
        {
            spriteAnimator.SetBool("isWalking", false);

            velocity = Vector2.zero;
        }

        transform.rotation = Quaternion.identity;

        //If holding an undeployed block and no interactable is found, place the indicator.
        if (grabbedObject && grabbedObject.GetComponent<UndeployedBlock>() && manager.IsPositionAvailable(grabbedObject.transform.position))
        {
            newBlockIndicator.transform.parent = boat.transform;

            Vector2Int gridIndex = manager.GetGridIndex(grabbedObject.transform.position);
            newBlockIndicator.transform.localPosition = new Vector3(gridIndex.x, gridIndex.y);

            newBlockIndicator.SetActive(true);
        }

        else
        {
            newBlockIndicator.SetActive(false);
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
                // Try picking up grabbable.
                Grabbable grabbableObject = zoneComponent.activeGrabbableObject;

                //If the object is grabbable, set it's parent to the interact zone and reset its position.
                if (grabbableObject)
                {
                    GrabObject(grabbableObject.gameObject);
                    return;
                }

                Interactable interactableObject = zoneComponent.activeInteractableObject;
                if (interactableObject && (!currentlyUsingInteractable || currentlyUsingInteractable != interactableObject))
                {
                    if (currentlyUsingInteractable)
                    {
                        currentlyUsingInteractable.OnStopUsing(this);
                    }

                    if (interactableObject.stopsPlayerMovement)
                    {
                        currentlyUsingInteractable = zoneComponent.activeInteractableObject;
                    }
                    interactableObject.OnUse(this);
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
        float horizontalChangeInVelocity = Input.GetAxis("Horizontal") * accelerationSpeed * Time.deltaTime;
        float verticalChangeInVelocity = Input.GetAxis("Vertical") * accelerationSpeed * Time.deltaTime;

        spriteAnimator.SetBool("isWalking", Mathf.Abs(horizontalChangeInVelocity) > .1f || Mathf.Abs(verticalChangeInVelocity) > .1f);

        if (Input.GetAxis("Vertical") != 0)
        {
            velocity.y = Mathf.Clamp(velocity.y + verticalChangeInVelocity, -maxSpeed, maxSpeed);

            if (Input.GetAxis("Vertical") > 0.1)
            {
                facingDirection = new Vector2(0, interactZoneOffset);
            }

            else if (Input.GetAxis("Vertical") < -0.1f)
            {
                facingDirection = new Vector2(0, -interactZoneOffset);
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            velocity.x = Mathf.Clamp(velocity.x + horizontalChangeInVelocity, -maxSpeed, maxSpeed);

            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                facingDirection = new Vector2(interactZoneOffset, 0.0f);
            }

            else if (Input.GetAxis("Horizontal") < -0.1f)
            {
                facingDirection = new Vector2(-interactZoneOffset, 0.0f);
            }
        }

        velocity += -velocity * drag * Time.deltaTime;

        interactZone.transform.localPosition = facingDirection;

        SetAnimationDirection(facingDirection);
    }

    private void SetAnimationDirection(Vector2 direction)
    {
        spriteAnimator.SetBool("isFacingLeft", direction.x < 0);
        spriteAnimator.SetBool("isFacingRight", direction.x > 0);
        spriteAnimator.SetBool("isFacingUp", direction.y > 0);
        spriteAnimator.SetBool("isFacingDown", direction.y < 0);
    }

    private void FixedUpdate()
    {
        if (velocity != Vector2.zero)
        {
            Vector3 newPosition = transform.position + new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime);
            Vector2Int index = manager.GetGridIndex(newPosition);
            if (manager.grid.ContainsKey(index))
            {
                transform.position = newPosition;
            }
            else
            {
                velocity = Vector2.zero;
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
