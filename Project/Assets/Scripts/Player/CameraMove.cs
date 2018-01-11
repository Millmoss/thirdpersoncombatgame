using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMove : MonoBehaviour
{
    public ControllerInput inputScript;
    public Camera cam;
    public Transform camPoint;
    private Vector3 playerChange;
    private float xTiltLook;
    private float zTiltLook;
    private float rotateSpeed;

    void Start()
    {
        transform.position = new Vector3(camPoint.transform.position.x, camPoint.transform.position.y + 0.188f, camPoint.transform.position.z - 1.2f);
        cam.transform.Rotate(Vector3.right, 38);
        playerChange = Vector3.zero;
        rotateSpeed = 200;
    }

    void FixedUpdate()
    {
        //camera control input
        xTiltLook = inputScript.getXTiltLook();
        zTiltLook = 0 - inputScript.getZTiltLook();

        //camera movement
        playerChange = camPoint.transform.position - playerChange;
        //playerChange.y = 0;
        transform.position += playerChange;

        //camera y rotation //DO NOT REORDER X AND Y!!!
        transform.RotateAround(camPoint.transform.position, Vector3.up, xTiltLook * Time.deltaTime * rotateSpeed);

        //camera x rotation //IF ROTATION GETS WEIRD REORDER X AND Y!!!
        float xRotation = cam.transform.rotation.eulerAngles.x;
        float futureRotation = zTiltLook * Time.deltaTime * rotateSpeed;
        cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * 0.7f);
        if ((xRotation + futureRotation) > 45 && (xRotation + futureRotation) < 180)
        {
            cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * -0.7f);
        }
        else if ((xRotation + futureRotation) < 324 && (xRotation + futureRotation) > 180)
        {
            cam.transform.RotateAround(camPoint.position, camPoint.right, futureRotation * -0.7f);
        }

        //update player position
        playerChange = camPoint.position;
    }

    void LateUpdate()
    {
        //movement stabilization
        playerChange = camPoint.position - playerChange;
        //playerChange.y = 0;
        transform.position += playerChange;
        playerChange = camPoint.position;
    }
}