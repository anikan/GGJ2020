using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public static WindController instance;
    public Vector2 currentWind;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        
    }
}
