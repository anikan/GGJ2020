using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : Damaging
{
    // Start is called before the first frame update
    void Start()
    {
        destroyOnHit = true;

        //Test initial velocity.
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.GetComponent<Block>())
        {
            Destroy(this.gameObject);
        }
    }
}
