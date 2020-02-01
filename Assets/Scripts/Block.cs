using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private int hp;
    public bool isSafeToAttachTo = true;
    public float mass = 1;
}
