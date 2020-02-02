using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Boulder is a dangerous obstacle. It doesn't move, but it will slice through your ship
 */
public class Boulder : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get reference to Box Manager
        BlockManager manager = collision.gameObject.GetComponentInParent<BlockManager>();

        if(manager != null)
        {
            // Apply a force to the Box Manager's Rigidbody to slow down the whole ship
            var parentRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();
            Vector2 appliedForce = Vector3.Normalize(parentRb.velocity) * stoppingForce;
            parentRb.AddForce(appliedForce);

            // Get the world position of the collided box and RemoveBlock
            var victimPos = collision.transform.position;
            var block = manager.RemoveBlock(victimPos);

        }
    }
}
