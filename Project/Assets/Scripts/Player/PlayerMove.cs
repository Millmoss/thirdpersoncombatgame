using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour
{
    public ControllerInput inputScript;
    private float xTiltMove;
    private float zTiltMove;
    private float xTiltLook;
    private float zTiltLook;
    private float xMoveMod;
    private float zMoveMod;
    private float moveSpeed; //set to 4
    private float xSpeedUp; //set to 3
    private float zSpeedUp; //set to 3
    public Rigidbody playerBody;
    private bool controlMethod; //mark true for using gamepad, false for using keyboard and mouse
    private float rotateSpeed; //set to 1
    public Camera playerCam;
    public Transform camPoint;
    private PlayerAttack pAttack;
    private bool paused;
    private bool inv;

    void Start()
    {
        xTiltMove = 0f;
        zTiltMove = 0f;
        xTiltLook = 0f;
        zTiltLook = 0f;
        xMoveMod = 0f;
        zMoveMod = 0f;
        moveSpeed = 4;
        xSpeedUp = 3;
        zSpeedUp = 3;
        playerBody = GetComponent<Rigidbody>();
        controlMethod = true;
        rotateSpeed = 200;
        pAttack = GetComponent<PlayerAttack>();
        paused = false;
        inv = false;
    }

	void Update(){
		//pause
		if (Input.GetKeyDown ("joystick 1 button 7") == true) {
			if (paused)
				paused = false;
			else
				paused = true;
		}

        if (Input.GetKeyDown("joystick 1 button 6") == true)
        {
            inv = !inv;
        }
    }

    void FixedUpdate()
    {
        //movement input
		if (!paused && !inv)
        {
			xTiltMove = inputScript.getXTiltMove ();
			zTiltMove = inputScript.getZTiltMove ();
		}
        else
        {
			xTiltMove = 0;
			zTiltMove = 0;
		}
        //movement input editting
        if (xTiltMove > 0 && xTiltMove < .5f)
            xTiltMove = .334f;
        else if (xTiltMove > 0)
            xTiltMove = 1;
        if (zTiltMove > 0 && zTiltMove < .5f)
            zTiltMove = .334f;
        else if (zTiltMove > 0)
            zTiltMove = 1;
        if (xTiltMove < 0 && xTiltMove > -.5f)
            xTiltMove = -.334f;
        else if (xTiltMove < 0)
            xTiltMove = -1;
        if (zTiltMove < 0 && zTiltMove > -.5f)
            zTiltMove = -.334f;
        else if (zTiltMove < 0)
            zTiltMove = -1;
        if (zTiltMove < 0)
            zTiltMove = zTiltMove / 1.5f;
        xTiltMove = xTiltMove / 1.25f;

        //look input
        xTiltLook = inputScript.getXTiltLook();
        zTiltLook = inputScript.getZTiltLook();

        //rotation method call
        rotate();

        //movement code
        if (xMoveMod < xTiltMove)
        {
            float tempMod = 1;
            if (xTiltMove == 0 || xMoveMod < 0)
                tempMod = 5f;
            xMoveMod += xSpeedUp * Time.deltaTime * tempMod;
            if (tempMod == 5f && xMoveMod > 0)
                xMoveMod = 0;
            if (xMoveMod > xTiltMove)
                xMoveMod = xTiltMove;
        }
        if (xMoveMod > xTiltMove)
        {
            float tempMod = 1;
            if (xTiltMove == 0 || xMoveMod > 0)
                tempMod = 5f;
            xMoveMod -= xSpeedUp * Time.deltaTime * tempMod;
            if (tempMod == 5f && xMoveMod < 0)
                xMoveMod = 0;
            if (xMoveMod < xTiltMove)
                xMoveMod = xTiltMove;
        }
        Mathf.Clamp(xMoveMod, -xTiltMove, xTiltMove);
        if (zMoveMod < zTiltMove)
        {
            float tempMod = 1;
            if (zTiltMove == 0 || zMoveMod < 0)
                tempMod = 7f;
            zMoveMod += zSpeedUp * Time.deltaTime * tempMod;
            if (tempMod == 5f && zMoveMod > 0)
                zMoveMod = 0;
            if (zMoveMod > zTiltMove)
                zMoveMod = zTiltMove;
        }
        if (zMoveMod > zTiltMove)
        {
            float tempMod = 1;
            if (zTiltMove == 0 || zMoveMod > 0)
                tempMod = 7f;
            zMoveMod -= zSpeedUp * Time.deltaTime * tempMod;
            if (tempMod == 7f && zMoveMod < 0)
                zMoveMod = 0;
            if (zMoveMod < zTiltMove)
                zMoveMod = zTiltMove;
        }
        Mathf.Clamp(zMoveMod, -zTiltMove, zTiltMove);
        Vector3 xyzTemp = moveSpeed * xMoveMod * transform.right + moveSpeed * zMoveMod * transform.forward + new Vector3(0, -6, 0);
        xyzTemp = Vector3.ClampMagnitude(xyzTemp, 7);
        if (pAttack.getJumping()) //player is jumping, so has a different velocity
        {
            xyzTemp = pAttack.getJumpVelocity();
        }
        playerBody.velocity = xyzTemp;
        camPoint.position = transform.position - new Vector3(0, .5f, 0) + playerBody.velocity * Time.deltaTime;
        camPoint.Translate(new Vector3(0, .75f, 0));
    }

    void LateUpdate()
    {
        camPoint.position = transform.position - new Vector3(0, .5f, 0);
        camPoint.Translate(new Vector3(0, .75f, 0));
    }

    void rotate()
    {
        camPoint.Rotate(Vector3.up, xTiltLook * Time.deltaTime * rotateSpeed);
        float interval = .1f;
        if (Mathf.Abs(xTiltLook) > .5)
            interval = .35f;
        float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, playerCam.transform.rotation.eulerAngles.y, interval);
        Vector3 angleVect = new Vector3(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(angleVect);
        #region deprecated code
        /*//segment 1//
        Vector3 movement = new Vector3(xTiltLook, 0.0f, zTiltLook);
        float angleDif = transform.rotation.eulerAngles.y - Quaternion.LookRotation(movement).eulerAngles.y;
        float interval = .15f;

        if (Mathf.Abs(xTiltLook) > 0f || Mathf.Abs(zTiltLook) > 0f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), interval);
        else
            rotated = true;*/
        /*//segment 2//
        if (xTiltLook != 0 && rotateTime <= .4f)
            rotateTime += Time.deltaTime;
        else if (xTiltLook == 0)
            rotateTime = 0;
        transform.Rotate(Vector3.up, xTiltLook * rotateSpeed * rotateTime);*/
        /*float rotateMod = 1;
        if (xLookBuffer != 0 && xTiltLook == 0)
            endInput = true;
        xLookBuffer = xTiltLook;
        if (xTiltLook > .85f)
            xLookBuffer = 1;
        else if (xTiltLook < -.85f)
            xLookBuffer = -1;
        if (xTiltLook != 0 && rotateTime <= rotateDelay)
        {
            rotateTime += Time.deltaTime;
            rotateMod = rotateTime / rotateDelay;
        }
        else if (xTiltLook != 0 && rotateTime > rotateDelay)
        {
            rotateTime += Time.deltaTime;
            rotateMod = 1;
        }
        else
        {
            rotateTime = 0;
            if (endInput == true)
            {
                finishTimer = Mathf.Abs(Mathf.Abs(playerCam.transform.rotation.eulerAngles.y) - Mathf.Abs(transform.rotation.eulerAngles.y)) / 40;
                finishTime = finishTimer * 2;
                endInput = false;
                stillRotating = true;
            }
            if (finishTimer > 0)
            {
                finishTimer -= Time.deltaTime;
            }
            else
            {
                stillRotating = false;
                Vector3 goalRotation = new Vector3(transform.rotation.eulerAngles.x, playerCam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Euler(goalRotation);
                finishTimer = 0;
            }
        }
        transform.Rotate(Vector3.up, xLookBuffer * Time.deltaTime * rotateSpeed * rotateMod);
        if (stillRotating)
        {
            Vector3 goalRotation = new Vector3(transform.rotation.eulerAngles.x, playerCam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalRotation), (finishTime - finishTimer) / finishTime);
        }
        */
        #endregion
    }
}