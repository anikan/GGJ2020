using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public Interactable activeInteractableObject;
    public Grabbable activeGrabbableObject;

    public GameObject selectedBlockIndicator;

    // Start is called before the first frame update
    void Start()
    {
        selectedBlockIndicator.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Interactable>())
        {
            activeInteractableObject = other.gameObject.GetComponent<Interactable>();

            selectedBlockIndicator.SetActive(true);

            selectedBlockIndicator.transform.parent = activeInteractableObject.transform;
            selectedBlockIndicator.transform.localPosition = Vector3.zero;
        }

        if (other.gameObject.GetComponent<Grabbable>())
        {
            activeGrabbableObject = other.gameObject.GetComponent<Grabbable>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Interactable>() && activeInteractableObject == other.gameObject.GetComponent<Interactable>())
        {
            activeInteractableObject = null;

            selectedBlockIndicator.SetActive(false);
        }

        if (other.gameObject.GetComponent<Grabbable>() && activeGrabbableObject == other.gameObject.GetComponent<Grabbable>())
        {
            activeGrabbableObject = null;
        }
    }
}
