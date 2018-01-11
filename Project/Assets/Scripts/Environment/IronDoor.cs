using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronDoor : Door
{
    void Start()
    {
        doorInfo = new DoorStatus('i');
    }
}