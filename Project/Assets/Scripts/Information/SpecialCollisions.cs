using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCollisions : MonoBehaviour
{
    public GameObject parentObj;

    void Start()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "SwordBoxA")
            parentObj.SendMessage("parried", collision);
    }
}
