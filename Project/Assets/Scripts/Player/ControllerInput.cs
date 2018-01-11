using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ControllerInput : MonoBehaviour
{
    private int controlScheme = 0;
    private enum schemeName { player, inventory, pause };
    //could possible hold an array of strings so that button rebinding is easy
    //should be a separate script for keyboard and mouse maybe? later though if keyboard and mouse is added

    public float getXTiltMove()
    {
        float tempx = CrossPlatformInputManager.GetAxis("Horizontal");
        float tempz = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
        if (Mathf.Abs(tempv.x) >= .15f)
            return tempv.x;
        return 0;
    }

    public float getZTiltMove()
    {
        float tempx = CrossPlatformInputManager.GetAxis("Horizontal");
        float tempz = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 tempv = Vector3.ClampMagnitude(new Vector3(tempx, 0, tempz), 1);
        if (Mathf.Abs(tempv.z) >= .15f)
            return tempv.z;
        return 0;
    }

    public float getXTiltLook()
    {
        float temp = CrossPlatformInputManager.GetAxis("Horizontal2");
        if (Mathf.Abs(temp) >= .07f)
            return temp;
        return 0;
    }

    public float getZTiltLook()
    {
        float temp = CrossPlatformInputManager.GetAxis("Vertical2");
        if (Mathf.Abs(temp) >= .07f)
            return temp;
        return 0;
    }

    public int getXTiltNav()
    {
        float temp = CrossPlatformInputManager.GetAxis("Horizontal");
        if (temp >= .15f)
            return 1;
        if (temp <= -.15f)
            return -1;
        return 0;
    }

    public int getZTiltNav()
    {
        float temp = CrossPlatformInputManager.GetAxis("Vertical");
        if (temp >= .15f)
            return 1;
        if (temp <= -.15f)
            return -1;
        return 0;
    }

    public bool getInteractPressed()
    {
        return Input.GetKeyDown("joystick 1 button 0"); //a, general interaction
    }

    public bool getThrowPressed()
    {
        return Input.GetKeyDown("joystick 1 button 2"); //x, throw
    }

    public bool getConsumePressed()
    {
        return Input.GetKeyDown("joystick 1 button 1"); //b, consume
    }

    public bool getPrimaryPressed()
    {
        return Input.GetKeyDown("joystick 1 button 5"); //rb, primary attack
    }

    public bool getSecondaryPressed()
    {
        return Input.GetAxis("RT") == 1; //rt, secondary attack
    }

    public bool getParryPressed()
    {
        return Input.GetKeyDown("joystick 1 button 4"); //lb, parry
    }

    public bool getDodgePressed()
    {
        return Input.GetAxis("LT") == 1; //lt, dodge/jump
    }

    public bool getPausePressed()
    {
        bool pressed = Input.GetKeyDown("joystick 1 button 7"); //start button
        if (pressed)
        {
            if (controlScheme == 0)
                controlScheme = 2;
            else if (controlScheme == 2)
                controlScheme = 0;
        }
        return pressed;
    }

    public bool getInventoryPressed()
    {
        bool pressed = Input.GetKeyDown("joystick 1 button 6"); //back button
        if (pressed)
        {
            if (controlScheme == 0)
                controlScheme = 1;
            else if (controlScheme == 1)
                controlScheme = 0;
        }
        return pressed;
    }

    public int getControlScheme()
    {
        return controlScheme;
    }
}
