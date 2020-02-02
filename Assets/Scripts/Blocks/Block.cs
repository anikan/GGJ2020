using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Interactable
{
    [SerializeField] private int hp;
    public bool isSafeToAttachTo = true;
    public float mass = 1;
    public bool visited; // for Block Manager use only

    public ResourceType matType;

    private GameObject resourceUIIcon;

    [SerializeField]
    private Vector3 resourceUIIconOffset;

    public override void OnUse(Player player)
    {
        // TO FILL
    }

    public void OnHit(int damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            //Break?
        }

        resourceUIIcon = GameObject.Instantiate<GameObject>(BlockPrefabs.instance.needResourceIconPrefab);
        resourceUIIcon.transform.localPosition = resourceUIIconOffset;
        resourceUIIcon.transform.parent = this.transform;
    }

    public void OnRepair()
    {
        if (resourceUIIcon)
        {
            Destroy(resourceUIIcon);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        Damaging damageable = collider.gameObject.GetComponent<Damaging>();
        if (damageable)
        {
            OnHit(damageable.strength);
        }
    }
}
