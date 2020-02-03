using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Interactable
{
    public int overkillHp; // After this HP hits, the object gets destroyed; should be negative
    public int maxHP; // After this HP hits, the object gets disconnected from the ship and becomes a resource
    public int hp; // current HP

    public bool isSafeToAttachTo = true;
    public float mass = 1;
    public bool visited; // for Block Manager use only

    public ResourceType matType;

    private GameObject resourceUIIcon;

    [SerializeField]
    private Vector3 resourceUIIconOffset;

    public bool wasRepaired = false;

    public virtual void OnAttach(Transform blockParent)
    {

    }

    public override void OnUse(Player player)
    {
        // TO FILL
    }

    public override void OnStopUsing(Player player)
    {
        // TO FILL
    }

    public void OnHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            // Remove it from the grid system
            var manager = this.GetComponentInParent<BlockManager>();
            var block = manager.RemoveBlock(this.transform.position);

            // Spawn the resource
            // TO DO

            if (hp <= overkillHp)
            {
                // Destroy the block
                Destroy(this);
            }
        }


        resourceUIIcon = GameObject.Instantiate<GameObject>(BlockPrefabs.instance.needResourceIconPrefab);
        resourceUIIcon.transform.parent = this.transform;
        resourceUIIcon.transform.localPosition = resourceUIIconOffset;
    }

    public void OnRepair()
    {
        wasRepaired = true;
        hp = maxHP;

        GetComponent<Renderer>().material = BlockPrefabs.instance.repairedMaterial;

        if (resourceUIIcon)
        {
            Destroy(resourceUIIcon);
        }
    }
}
