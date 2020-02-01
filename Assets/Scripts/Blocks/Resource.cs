﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, Fabric, Metal
}

public class Resource : Interactable
{
    public ResourceType type;
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
    }

    public void SetResourceType(ResourceType type)
    {
        this.type = type;

        //Set materials based on type.
    }
}