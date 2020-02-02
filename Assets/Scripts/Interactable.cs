using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool stopsPlayerMovement;
    public abstract void OnUse(Player player);
    public abstract void OnStopUsing(Player player);
}
