using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : MonoBehaviour
{
    public Door toLock;

	void Start ()
    {

	}
	
	void Update ()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
            toLock.SendMessage("lockDoor");
    }
}
