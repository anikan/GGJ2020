using UnityEngine;

public class Dispenser : Interactable
{
    public GameObject resourcePrefab;
    public ResourceType resourceType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnUse(Player player)
    {
        //Spawn item and have player hold it.
        GameObject resource = GameObject.Instantiate<GameObject>(resourcePrefab);

        resource.GetComponent<Resource>().SetResourceType(resourceType);

        player.GrabObject(resource);
    }
}
