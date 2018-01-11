using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadAnimate : MonoBehaviour
{
    public GameObject arm1a;
    public GameObject arm1b;
    public GameObject arm1c;
    public GameObject arm1d;
    public GameObject arm1e;
    public GameObject arm2a;
    public GameObject arm2b;
    public GameObject arm2c;
    public GameObject arm2d;
    public GameObject arm2e;
    public GameObject leftHand;

    private Vector3[] arm1Pos;
    private Vector3[] arm2Pos;
    private Vector3 leftHandPos;

    public string weaponType;
    public bool busy;
    private bool xPressed;
    private bool yPressed;
    private bool bPressed;
    private float eTime;

    void Start()
    {
        busy = false;
        xPressed = false;
        yPressed = false;
        bPressed = false;
        eTime = 0f;
        weaponType = "estoc";

        arm1Pos = new Vector3[5];
        arm2Pos = new Vector3[5];
        arm1Pos[0] = arm1a.transform.localPosition;
        arm1Pos[1] = arm1b.transform.localPosition;
        arm1Pos[2] = arm1c.transform.localPosition;
        arm1Pos[3] = arm1d.transform.localPosition;
        arm1Pos[4] = arm1e.transform.localPosition;
        arm2Pos[0] = arm2a.transform.localPosition;
        arm2Pos[1] = arm2b.transform.localPosition;
        arm2Pos[2] = arm2c.transform.localPosition;
        arm2Pos[3] = arm2d.transform.localPosition;
        arm2Pos[4] = arm2e.transform.localPosition;
        leftHandPos = leftHand.transform.localPosition;

    }

    void Update()
    {
        if (Input.GetKeyDown("joystick 1 button 5")) //check for press of x
        {
            if (busy == false)
            {
                xPressed = true;
                busy = true;
            }
        }
        else if (Input.GetAxis("RT") == 1) //check for press of y
        {
            if (busy == false)
            {
                yPressed = true;
                busy = true;
            }
        }
        else if (Input.GetKeyDown("joystick 1 button 4")) //check for press of b
        {
            if (busy == false)
            {
                bPressed = true;
                busy = true;
            }
        }
        if (xPressed == true)
            primaryAttack();
        else if (yPressed == true)
            secondaryAttack();
        else if (bPressed == true)
            tertiaryAttack();
        else
            idle();
    }

    void idle()
    {
        arm1a.transform.localPosition = arm1Pos[0];
        arm1b.transform.localPosition = arm1Pos[1];
        arm1c.transform.localPosition = arm1Pos[2];
        arm1d.transform.localPosition = arm1Pos[3];
        arm1e.transform.localPosition = arm1Pos[4];
        arm2a.transform.localPosition = arm2Pos[0];
        arm2b.transform.localPosition = arm2Pos[1];
        arm2c.transform.localPosition = arm2Pos[2];
        arm2d.transform.localPosition = arm2Pos[3];
        arm2e.transform.localPosition = arm2Pos[4];
        leftHand.transform.localPosition = leftHandPos;
    }

    void primaryAttack()
    {
        if (weaponType == "estoc")
        {
            eTime += Time.deltaTime;
            if (eTime < .1f)
            {
                arm1a.transform.localPosition = Vector3.Lerp(arm1Pos[0], new Vector3(arm1Pos[0].x, arm1Pos[0].y, arm1Pos[0].z + .01f), eTime / .1f);
                arm1b.transform.localPosition = Vector3.Lerp(arm1Pos[1], new Vector3(arm1Pos[1].x, arm1Pos[1].y, arm1Pos[1].z + .01f), eTime / .1f);
                arm1c.transform.localPosition = Vector3.Lerp(arm1Pos[2], new Vector3(arm1Pos[2].x, arm1Pos[2].y, arm1Pos[2].z + .02f), eTime / .1f);
                arm1d.transform.localPosition = Vector3.Lerp(arm1Pos[3], new Vector3(arm1Pos[3].x, arm1Pos[3].y, arm1Pos[3].z + .03f), eTime / .1f);
                arm1e.transform.localPosition = Vector3.Lerp(arm1Pos[4], new Vector3(arm1Pos[4].x, arm1Pos[4].y, arm1Pos[4].z + .05f), eTime / .1f);
                arm2a.transform.localPosition = Vector3.Lerp(arm2Pos[0], new Vector3(arm2Pos[0].x, arm2Pos[0].y, arm2Pos[0].z + .01f), eTime / .1f);
                arm2b.transform.localPosition = Vector3.Lerp(arm2Pos[1], new Vector3(arm2Pos[1].x, arm2Pos[1].y, arm2Pos[1].z + .01f), eTime / .1f);
                arm2c.transform.localPosition = Vector3.Lerp(arm2Pos[2], new Vector3(arm2Pos[2].x, arm2Pos[2].y, arm2Pos[2].z + .02f), eTime / .1f);
                arm2d.transform.localPosition = Vector3.Lerp(arm2Pos[3], new Vector3(arm2Pos[3].x, arm2Pos[3].y, arm2Pos[3].z + .03f), eTime / .1f);
                arm2e.transform.localPosition = Vector3.Lerp(arm2Pos[4], new Vector3(arm2Pos[4].x, arm2Pos[4].y, arm2Pos[4].z + .05f), eTime / .1f);
                leftHand.transform.localPosition = Vector3.Lerp(leftHandPos, new Vector3(leftHandPos.x, leftHandPos.y, leftHandPos.z + .05f), eTime / .1f);
            }
            else if (eTime >= .1f && eTime < .17f)
            {
                arm1a.transform.localPosition = Vector3.Lerp(arm1Pos[0], new Vector3(arm1Pos[0].x - .05f, arm1Pos[0].y, arm1Pos[0].z + 1f), (eTime - .1f) / .07f);
                arm1b.transform.localPosition = Vector3.Lerp(arm1Pos[1], new Vector3(arm1Pos[1].x - .1f, arm1Pos[1].y + .025f, arm1Pos[1].z + 2f), (eTime - .1f) / .07f);
                arm1c.transform.localPosition = Vector3.Lerp(arm1Pos[2], new Vector3(arm1Pos[2].x - .15f, arm1Pos[2].y + .05f, arm1Pos[2].z + 3f), (eTime - .1f) / .07f);
                arm1d.transform.localPosition = Vector3.Lerp(arm1Pos[3], new Vector3(arm1Pos[3].x - .2f, arm1Pos[3].y + .1f, arm1Pos[3].z + 4f), (eTime - .1f) / .07f);
                arm1e.transform.localPosition = Vector3.Lerp(arm1Pos[4], new Vector3(arm1Pos[4].x - .25f, arm1Pos[4].y + .2f, arm1Pos[4].z + 5f), (eTime - .1f) / .07f);
                arm2a.transform.localPosition = Vector3.Lerp(arm2Pos[0], new Vector3(arm2Pos[0].x - .025f, arm2Pos[0].y, arm2Pos[0].z + .5f), (eTime - .1f) / .07f);
                arm2b.transform.localPosition = Vector3.Lerp(arm2Pos[1], new Vector3(arm2Pos[1].x - .06f, arm2Pos[1].y + .025f, arm2Pos[1].z + 1.3f), (eTime - .1f) / .07f);
                arm2c.transform.localPosition = Vector3.Lerp(arm2Pos[2], new Vector3(arm2Pos[2].x - .11f, arm2Pos[2].y + .05f, arm2Pos[2].z + 2.3f), (eTime - .1f) / .07f);
                arm2d.transform.localPosition = Vector3.Lerp(arm2Pos[3], new Vector3(arm2Pos[3].x - .15f, arm2Pos[3].y + .1f, arm2Pos[3].z + 3.1f), (eTime - .1f) / .07f);
                arm2e.transform.localPosition = Vector3.Lerp(arm2Pos[4], new Vector3(arm2Pos[4].x - .2f, arm2Pos[4].y + .2f, arm2Pos[4].z + 4f), (eTime - .1f) / .07f);
                leftHand.transform.localPosition = Vector3.Lerp(leftHandPos, new Vector3(leftHandPos.x - .25f, leftHandPos.y + .4f, leftHandPos.z + 5f), (eTime - .1f) / .07f);
            }
            else if (eTime >= .17f && eTime < .30f)
            {
                arm1a.transform.localPosition = new Vector3(arm1Pos[0].x - .05f, arm1Pos[0].y, arm1Pos[0].z + 1f);
                arm1b.transform.localPosition = new Vector3(arm1Pos[1].x - .1f, arm1Pos[1].y + .025f, arm1Pos[1].z + 2f);
                arm1c.transform.localPosition = new Vector3(arm1Pos[2].x - .15f, arm1Pos[2].y + .05f, arm1Pos[2].z + 3f);
                arm1d.transform.localPosition = new Vector3(arm1Pos[3].x - .2f, arm1Pos[3].y + .1f, arm1Pos[3].z + 4f);
                arm1e.transform.localPosition = new Vector3(arm1Pos[4].x - .25f, arm1Pos[4].y + .2f, arm1Pos[4].z + 5f);
                arm2a.transform.localPosition = new Vector3(arm2Pos[0].x - .025f, arm2Pos[0].y, arm2Pos[0].z + .5f);
                arm2b.transform.localPosition = new Vector3(arm2Pos[1].x - .06f, arm2Pos[1].y + .025f, arm2Pos[1].z + 1.3f);
                arm2c.transform.localPosition = new Vector3(arm2Pos[2].x - .11f, arm2Pos[2].y + .05f, arm2Pos[2].z + 2.3f);
                arm2d.transform.localPosition = new Vector3(arm2Pos[3].x - .15f, arm2Pos[3].y + .1f, arm2Pos[3].z + 3.1f);
                arm2e.transform.localPosition = new Vector3(arm2Pos[4].x - .2f, arm2Pos[4].y + .2f, arm2Pos[4].z + 4f);
                leftHand.transform.localPosition = new Vector3(leftHandPos.x - .25f, leftHandPos.y + .4f, leftHandPos.z + 5f);
            }
            else if (eTime >= .30f)
            {
                arm1a.transform.localPosition = Vector3.Lerp(new Vector3(arm1Pos[0].x - .05f, arm1Pos[0].y, arm1Pos[0].z + 1f), arm1Pos[0], (eTime - .30f) / .3f);
                arm1b.transform.localPosition = Vector3.Lerp(new Vector3(arm1Pos[1].x - .1f, arm1Pos[1].y + .025f, arm1Pos[1].z + 2f), arm1Pos[1], (eTime - .30f) / .3f);
                arm1c.transform.localPosition = Vector3.Lerp(new Vector3(arm1Pos[2].x - .15f, arm1Pos[2].y + .05f, arm1Pos[2].z + 3f), arm1Pos[2], (eTime - .30f) / .3f);
                arm1d.transform.localPosition = Vector3.Lerp(new Vector3(arm1Pos[3].x - .2f, arm1Pos[3].y + .1f, arm1Pos[3].z + 4f), arm1Pos[3], (eTime - .30f) / .3f);
                arm1e.transform.localPosition = Vector3.Lerp(new Vector3(arm1Pos[4].x - .25f, arm1Pos[4].y + .2f, arm1Pos[4].z + 5f), arm1Pos[4], (eTime - .30f) / .3f);
                arm2a.transform.localPosition = Vector3.Lerp(new Vector3(arm2Pos[0].x - .025f, arm2Pos[0].y, arm2Pos[0].z + .5f), arm2Pos[0], (eTime - .30f) / .3f);
                arm2b.transform.localPosition = Vector3.Lerp(new Vector3(arm2Pos[1].x - .06f, arm2Pos[1].y + .025f, arm2Pos[1].z + 1.3f), arm2Pos[1], (eTime - .30f) / .3f);
                arm2c.transform.localPosition = Vector3.Lerp(new Vector3(arm2Pos[2].x - .11f, arm2Pos[2].y + .05f, arm2Pos[2].z + 2.3f), arm2Pos[2], (eTime - .30f) / .3f);
                arm2d.transform.localPosition = Vector3.Lerp(new Vector3(arm2Pos[3].x - .15f, arm2Pos[3].y + .1f, arm2Pos[3].z + 3.1f), arm2Pos[3], (eTime - .30f) / .3f);
                arm2e.transform.localPosition = Vector3.Lerp(new Vector3(arm2Pos[4].x - .2f, arm2Pos[4].y + .2f, arm2Pos[4].z + 4f), arm2Pos[4], (eTime - .30f) / .3f);
                leftHand.transform.localPosition = Vector3.Lerp(new Vector3(leftHandPos.x - .25f, leftHandPos.y + .4f, leftHandPos.z + 5f), leftHandPos, (eTime - .30f) / .3f);
            }
            if (eTime >= .60f)
            {
                xPressed = false;
                busy = false;
                eTime = 0;
            }
        }
    }

    void secondaryAttack()
    {
        if (weaponType == "estoc")
        {
            //does a sweeping parry motion to block stabs and some attacks
        }
    }

    void tertiaryAttack()
    {
        if (weaponType == "estoc")
        {
            //does a hanging guard that blocks overhead attacks
        }
    }
}
