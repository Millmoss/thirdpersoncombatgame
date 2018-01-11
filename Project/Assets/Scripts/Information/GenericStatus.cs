using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStatus : MonoBehaviour
{
    private bool dangerous;

    void Start()
    {
        dangerous = false;
    }

    void Update()
    {

    }

    public bool getDangerous()
    {
        return dangerous;
    }

    public void setDangerous(bool d)
    {
        dangerous = d;
    }
}
