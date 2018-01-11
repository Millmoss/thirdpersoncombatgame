using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ControllerInput inputScript;
    public PlayerStatus playerInfo;
    public string weaponType;
    public GameObject weapon;
    private bool dangerous;
    private bool jumping;
    private bool sheathed;
    private bool busy;
    private bool rbPressed;
    private bool rtPressed;
    private bool lbPressed;
    private bool ltPressed;
    private float eTime;
    private float jTime;
    private Vector3 startPos;
    private Quaternion startRot; 
    public Vector3 swordRotation;
    private Vector3 jumpVelocity;
    private Vector3 jumpInfo;
    private Rigidbody playerBody;
    private GenericStatus genericInfo;

    void Start ()
    {
        weapon.transform.localPosition = new Vector3(0.2f, 0f, .8f);
        swordRotation = weapon.transform.localRotation.eulerAngles;
        jTime = .05f;
        eTime = .05f;
        jumping = false;
        sheathed = false;
        busy = false;
        rbPressed = false;
        rtPressed = false;
        lbPressed = false;
        ltPressed = false;
        dangerous = false;
        playerBody = GetComponent<Rigidbody>();
        playerInfo = GetComponent<PlayerStatus>();
        genericInfo = GetComponent<GenericStatus>();
    }
	
	void Update ()
    {
        genericInfo.setDangerous(dangerous);
        if (!sheathed)
        {
            if (inputScript.getPrimaryPressed()) //rb, primary attack
            {
                if (busy == false)
                {
                    rbPressed = true;
                    busy = true;
                }
            }
            else if (inputScript.getSecondaryPressed()) //rt, secondary attack
            {
                if (busy == false)
                {
                    rtPressed = true;
                    busy = true;
                }
            }
            else if (inputScript.getParryPressed()) //lb, parry
            {
                if (busy == false)
                {
                    lbPressed = true;
                }
            }
            else if (inputScript.getDodgePressed()) //lt, dodge/jump
            {
                ltPressed = true;
            }
            if (rbPressed == true)
                primaryAttack();
            else if (rtPressed == true)
                secondaryAttack();
            else if (lbPressed == true)
                parry();
            else
                idle();
            if (ltPressed == true)
                dodge();
        }
        else
        {
            safeIdle();
        }
    }

    void safeIdle()
    {
        idle();
    }

    void idle ()
    {
        weapon.transform.localPosition = new Vector3(0.2f, 0f, .8f);
        weapon.transform.localRotation = Quaternion.Euler(swordRotation);
    }

    void primaryAttack ()
    {
        if (weaponType == "estoc")
        {
            if (lbPressed)
            {
                eTime = 0;
                lbPressed = false;
            }
            if (eTime == 0)
            {
                startPos = weapon.transform.localPosition;
                startRot = weapon.transform.localRotation;
            }
            if (eTime >=  .1f && eTime < .17f)
            {
                weapon.transform.localPosition = Vector3.Lerp(startPos, new Vector3(0.134f, .1f, 3.3f), (eTime - .1f) / .14f);
                weapon.transform.localRotation = Quaternion.Lerp(startRot, Quaternion.Euler(swordRotation), (eTime - .1f) / .07f);
                dangerous = true;
            }
            else if (eTime >= .17f && eTime < .30f)
            {
                weapon.transform.localPosition = new Vector3(.1667f, .05f, 2.25f);
            }
            else if (eTime >= .30f)
            {
                weapon.transform.localPosition = Vector3.Lerp(new Vector3(.1667f, .05f, 2.25f), new Vector3(.2333f, -.05f, -.45f), (eTime - .3f) / .6f);
                dangerous = false;
            }
            eTime += Time.deltaTime;
            if (eTime >= .60f)
            {
                playerInfo.changeEnergy(-.02f);
                rbPressed = false;
                busy = false;
                eTime = 0;
            }
        }
    }

    void secondaryAttack ()
    {
        rtPressed = false;
        busy = false;
    }

    void parry () //timing based parry that can knock attacks out of the way, this applies to estoc, maybe to others but not all
    {
        Quaternion parryRotation = Quaternion.Euler(new Vector3(-43, -53, 22));
        Vector3 parryPosition = new Vector3(-.103f, .95f, 1.095f);
        if (weaponType == "estoc") //make work properly
        {
            eTime += Time.deltaTime;
            if (eTime < .05f)
            {
                weapon.transform.localPosition = Vector3.Lerp(new Vector3(0.2f, 0f, .8f), new Vector3(0.2f, .05f, .85f), eTime / .025f);
            }
            else if (eTime >= .05f && eTime < .2f)
            {
                weapon.transform.localPosition = Vector3.Lerp(new Vector3(.2f, .05f, .85f), parryPosition, Mathf.Log(.15f, (eTime - .05f)));
                weapon.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(swordRotation), parryRotation, Mathf.Log(.15f, (eTime - .05f)));
            }
            else if (eTime >= .30f)
            {
                weapon.transform.localPosition = Vector3.Lerp(parryPosition, new Vector3(.2f, 0f, .8f), (eTime - .3f) / .2f);
                weapon.transform.localRotation = Quaternion.Lerp(parryRotation, Quaternion.Euler(swordRotation), (eTime - .3f) / .2f);
            }
            if (eTime >= .5f)
            {
                playerInfo.changeEnergy(-.01f);
                lbPressed = false;
                eTime = 0;
            }
        }
    }

    void dodge ()
    {
        Vector3 zeroVector = new Vector3(0, -4, 0);
        if (jTime == 0.05f)
        {
            jumpInfo = 10 * inputScript.getXTiltMove() * transform.right + 10 * inputScript.getZTiltMove() * transform.forward;
            if (jumpInfo.x == 0 && jumpInfo.z == 0)
            {
                jumpInfo = 10 * -1 * transform.forward;
            }
        }
        jTime += Time.deltaTime;
        if (jTime < .1f)
        {
            jumping = true;
            jumpVelocity = Vector3.Lerp(playerBody.velocity, zeroVector, (jTime - .05f) / .05f);
        }
        else if (jTime >= .1f && jTime < .2f)
        {
            jumpVelocity = Vector3.Lerp(Vector3.zero, jumpInfo + new Vector3(0, 1.5f, 0), (jTime - .1f) / .1f);
        }
        else if (jTime >= .2f && jTime < .40f)
        {
            jumpVelocity = Vector3.Lerp(jumpInfo + new Vector3(0, 1.5f, 0), zeroVector, (jTime - .2f) / .2f);
        }
        else if (jTime >= .40f && jTime < .45f)
        {
            jumpVelocity = Vector3.Lerp(zeroVector, jumpInfo + new Vector3(0, 0.3f, 0), (jTime - .375f) / .1f);
        }
        else if (jTime >= .45f)
        {
            jumpVelocity = Vector3.Lerp(jumpInfo + new Vector3(0, 0.3f, 0), zeroVector, (jTime - .425f) / .1f);
        }
        if (jTime >= .5f)
        {
            jumping = false;
            ltPressed = false;
            jTime = .05f;
            if (jumpInfo.x != 0 || jumpInfo.z != 0)
                playerInfo.changeEnergy(-.04f);
        }
    }

    public bool getDangerous()
    {
        return dangerous;
    }

    public bool getJumping()
    {
        return jumping;
    }

    public bool getParrying()
    {
        if (weaponType == "estoc" && lbPressed)
            return true;
        return false;
    }

    public Vector3 getJumpVelocity()
    {
        return jumpVelocity;
    }
}