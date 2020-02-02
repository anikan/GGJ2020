﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Interactable
{
    [SerializeField] private int hp;
    public bool isSafeToAttachTo = true;
    public float mass = 1;
    public bool visited; // for Block Manager use only

    public override void OnUse(Player player)
    {
        // TO FILL
    }
}
