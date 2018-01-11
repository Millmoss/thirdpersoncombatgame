using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public Vector3 wallGoal;
    private Vector3 wallStart;
    public GameObject panel;
    public ControllerInput inputScript;
    public Transform playerPos;
    private bool open;
    private bool moving;
    private float time;

	void Start ()
    {
        wallStart = transform.localPosition;
        open = false;
        moving = false;
        time = 0;
	}
	
	void Update ()
    {
        if (inputScript.getInteractPressed() && Vector3.Distance(panel.transform.position, playerPos.position) < 1)
        {
            if (!moving)
            {
                moving = true;
                open = !open;
            }
        }
        if (moving)
        {
            if (open)
            {
                transform.localPosition = Vector3.Lerp(wallStart, wallGoal, time / .5f);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(wallGoal, wallStart, time / .5f);
            }
            time += Time.deltaTime;
            if (time >= .5f)
            {
                moving = false;
                time = 0;
            }
        }
    }
}