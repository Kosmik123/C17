using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusControl : MonoBehaviour {

    /// <summary>
    /// This is an example script for controlling the doors and signs on the bus. 
    /// You may wish to create a more specific implementation consistent with your game.
    /// </summary>

    public Animator Door1;
    public Animator Door2;
    public Animator StopSign1;
    public Animator StopSign3;

    public float OpenCloseSpeed;

    private bool isOpen = false;
    
    public void Open(bool open = true)
    {
        string action = open ? "Open" : "Close";
        SetAnimationTriggers(action);
        isOpen = open;
    }

    private void SetAnimationTriggers(string action)
    {
        Door1.SetTrigger(action);
        Door2.SetTrigger(action);
        StopSign1.SetTrigger(action);
        StopSign3.SetTrigger(action);
    }

    [Button]
    public void ToggleDoor()
    {
        Open(!isOpen);
    }

}
