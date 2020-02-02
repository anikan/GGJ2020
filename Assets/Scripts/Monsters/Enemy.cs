using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int maxHp;
    protected int currHp;
    [SerializeField]
    protected float stoppingForce; // the magnitude the force to apply to the ship on collision

    // Keep AI behavior in FixedUpdate()
    // Keep damage and HP management in OnCollide()
}
