using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour
{
    // Keep AI behavior in FixedUpdate()
    // Keep damage and HP management in OnCollide()

    public int strength; // damage dealt on collision
    public bool destroyOnHit; // should it be destroyed on hit?
    public float minimumImpulse; // minimum impulse needed to apply damage

    [SerializeField]
    protected int maxHp;
    protected int currHp;
    [SerializeField]
    protected float stoppingForce; // the magnitude the force to apply to the ship on collision

    protected void Awake()
    {
        currHp = maxHp;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // For colliding with blocks
        {
            // Get reference to Box Manager
            BlockManager manager = collision.gameObject.GetComponentInParent<BlockManager>();

            if (manager != null)
            {
                // Apply a force to the Box Manager's Rigidbody to slow down the whole ship
                var parentRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();
                Vector2 appliedForce = Vector3.Normalize(parentRb.velocity) * -stoppingForce;
                parentRb.AddForce(appliedForce);

                // Do damage. By default, damage is scaled by momentum                
                Block victim = manager.GetBlockAtWorldPosition(collision.gameObject.transform.position);
                if(victim != null)
                {
                    // All BlockManagers require a Rigidbody2D
                    Rigidbody2D shipRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();
                    // Calculate the damage
                    float damage = Mathf.Max((shipRb.velocity.magnitude * shipRb.mass) - minimumImpulse, 0) * strength;
                    victim.OnHit((int)damage);
                }

            }
        }
    }
}
