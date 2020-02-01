using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script for testing if the AddBlock() functionality works
[RequireComponent(typeof(Rigidbody))]
public class TestBlockScript : MonoBehaviour
{
    public Block prefab = null;
    public BlockManager manager = null;
    public Rigidbody rb = null;
    public float targetSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // If the E button is pressed, it submits a new block at its position
    // Otherwise, WASD to move
    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.E))
        {
            Block block = Instantiate(prefab);
            manager.AddBlock(block, this.transform.position);
        }
        if(Input.GetKey(KeyCode.W))
        {
            targetVelocity.x += 1;
        }
        if(Input.GetKey(KeyCode.A))
        {
            targetVelocity.y -= 1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            targetVelocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetVelocity.y += 1;
        }

        targetVelocity = Vector3.Normalize(targetVelocity) * targetSpeed;
        rb.AddForce((targetVelocity - rb.velocity) / Time.fixedDeltaTime);
    }
}
