using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject player;
    public Transform handle;
    public Rigidbody doorBody;
    public Rigidbody hingeBody;
    public HingeJoint doorHinge;
    public ControllerInput playerInput;
    protected DoorStatus doorInfo;
    protected float damageTime;
    protected float closeTime;
    protected bool closingDoor;
    protected bool broken;
    public bool trapped;
    protected bool locked;
    //public GameObject key;
    public int keyID;
    public GameObject sightStopper;

    void Start()
    {
        playerInput = player.GetComponent<ControllerInput>();
        damageTime = 0;
        closeTime = 0;
        closingDoor = false;
        broken = false;
        locked = false;
        gameObject.layer = 8;
    }

    void Update()
    {
        if (Vector3.Distance(handle.transform.position, player.transform.position) < 1 && playerInput.getInteractPressed() && !broken && !locked)
        {
            if (doorHinge.useSpring)
            {
                doorHinge.useSpring = false;
                gameObject.layer = 0;
                sightStopper.SetActive(false);
            }
            else
            {
                closingDoor = true;
                doorHinge.useSpring = true;
            }
        }

        if (doorInfo.getHealth() <= 0 && !broken)
        {
            doorBody.constraints = RigidbodyConstraints.None;
            doorHinge.breakForce = 0;
            doorHinge.breakTorque = 0;
            broken = true;
        }

        if (closingDoor && !broken)
        {
            JointSpring s = doorHinge.spring;
            s.spring = 10000;
            doorHinge.spring = s;
            closeTime += Time.deltaTime;
            if (closeTime >= .8f)
            {
                if (Vector3.Distance(handle.transform.position, player.transform.position) < 1 || trapped)
                {
                    s.spring = 1000000;
                    doorHinge.spring = s;
                    closingDoor = false;
                    gameObject.layer = 8;
                    sightStopper.SetActive(true);
                }
                else
                {
                    doorHinge.useSpring = false;
                    closingDoor = false;
                }
                closeTime = 0;
            }
        }
        damageTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider c)
    {
        if (damageTime > .2f)
        {
            if (c.tag == "Weapon Part" && c.GetComponentInParent<GenericStatus>().getDangerous())
                doorInfo.changeHealth(c.GetComponentInParent<Weapon>().getDamage());
            else if (c.tag == "Weapon" && c.GetComponentInParent<GenericStatus>().getDangerous())
                doorInfo.changeHealth(c.gameObject.GetComponent<Weapon>().getDamage());
            damageTime = 0;
        }
    }

    public void lockDoor()
    {
        if (!locked)
        {
            closingDoor = true;
            locked = true;
            doorHinge.useSpring = true;
        }
    }
}