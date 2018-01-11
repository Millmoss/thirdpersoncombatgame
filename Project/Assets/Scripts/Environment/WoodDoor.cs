using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDoor : Door
{
    void Start ()
    {
        doorInfo = new DoorStatus('w');
    }
}