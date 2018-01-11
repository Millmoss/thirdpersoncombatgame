using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAI : MonoBehaviour
{
    public GameObject path;
    public Rigidbody minotaurBody;
    public GameObject player;
    public Pathfind pathfindAccess;
    public GameObject searchPathPoint;
    private GenericStatus genericInfo;
    private Vector3[] pathPoints;
    private GameObject[] searchPathPoints;
    private int pathSize;
    public float speed;
    public float acceleration; //use this to dictate slow down and speedup, clamp values to current values
    private float lerpRotate;
    private bool playerVisible;
    private int patrolPathGoal;
    private LayerMask collisionLayer;
    private MinotaurStatus minotaurInfo;
    private int searchPathGoal;
    private bool mustReturn;
    private Vector3[] searchPath;
    private float thinkingInterval;
    private float attackTime;
    private float iFrames;
    public GameObject weapon;
    private Vector3 weaponPos;
    private Quaternion weaponRot;
    private Vector3 reverseStart;
    public PlayerAttack attack;
    private FSM minotaurFSM;
    private enum fsmStateRef { wait, growl, stalk, charge, halt, punch };
    public int stateDebug;
    private bool reverseAttack;

    void Start()
    {
        genericInfo = GetComponent<GenericStatus>();
        reverseAttack = false;
        minotaurInfo = new MinotaurStatus();
        Transform[] temp = path.GetComponentsInChildren<Transform>();
        pathSize = temp.Length;
        pathPoints = new Vector3[pathSize];
        for (int i = 0; i < pathSize; i++)
        {
            pathPoints[i] = temp[i].position;
        }
        lerpRotate = .05f;
        patrolPathGoal = 1;
        searchPathGoal = 1;
        collisionLayer = 1 << 9;
        mustReturn = false; //use to return to path using A*
        thinkingInterval = 0;
        weaponPos = new Vector3(.45f, 0.07f, .83f);
        weaponRot = weapon.transform.rotation;
        attackTime = .8f;
        iFrames = 0;
        #region FSM Instantiation
        //minotaur can    wait, growl, stalk, charge, halt, punch
        //correlates to   0     1      2      3       4     5
        bool[][] ins = new bool[6][];
        ins[0] = new bool[6];
        Array.Clear(ins[0], 0, 5);
        ins[0][1] = true;
        ins[1] = new bool[6];
        Array.Clear(ins[1], 0, 5);
        ins[1][2] = true;
        ins[1][3] = true;
        ins[1][5] = true;
        ins[2] = new bool[6];
        Array.Clear(ins[2], 0, 5);
        ins[2][1] = true;
        ins[2][3] = true;
        ins[2][4] = true;
        ins[2][5] = true;
        ins[3] = new bool[6];
        Array.Clear(ins[3], 0, 5);
        ins[3][4] = true;
        ins[4] = new bool[6];
        Array.Clear(ins[4], 0, 5);
        ins[4][2] = true;
        ins[4][5] = true;
        ins[5] = new bool[6];
        Array.Clear(ins[5], 0, 5);
        ins[5][1] = true;
        ins[5][2] = true;
        ins[5][3] = true;
        minotaurFSM = new FSM(0, ins);
        #endregion FSM Instantiation
    }

    void FixedUpdate()
    {
        stateDebug = minotaurFSM.getState();

        if (minotaurInfo.getHealth() < 1)
        {
            Destroy(transform.parent.gameObject);
        }

        determineVisible();

        switch (minotaurFSM.getState())
        {
            case (int)fsmStateRef.wait:
                wait();
                break;
            case (int)fsmStateRef.growl:
                growl();
                break;
            case (int)fsmStateRef.stalk:
                stalk();
                break;
            case (int)fsmStateRef.charge:
                charge();
                break;
            case (int)fsmStateRef.halt:
                halt();
                break;
            case (int)fsmStateRef.punch:
                punch();
                break;
        }
        weapon.transform.rotation = minotaurBody.rotation;
        weapon.transform.Rotate(new Vector3(-11, -7, 0));

        /*if (!playerVisible)
        {
            lerpRotate = .05f;
            aiNormalProcess();
        }
        else if (playerVisible)
        {
            Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
            
            //rotate towards player
            Quaternion tempCurrent = minotaurBody.rotation;
            Quaternion tempGoal = Quaternion.LookRotation(goalDirection); //make this work better
            tempGoal = Quaternion.Euler(new Vector3(0, tempGoal.eulerAngles.y, 0));
            minotaurBody.rotation = Quaternion.Slerp(tempCurrent, tempGoal, lerpRotate);

            aiThinking();
            dangerProcess();
            aiAttack();
        }*/
        iFrames += Time.deltaTime;
    }

    void determineVisible()
    {
        RaycastHit hit;
        playerVisible = !Physics.Raycast(minotaurBody.transform.position, (player.transform.position - minotaurBody.transform.position).normalized,
            out hit, Vector3.Distance(minotaurBody.transform.position, player.transform.position), collisionLayer); //check if player is visible
    }

    void cleanSearchPath()
    {
        for (int c = 0; c < searchPath.Length; c++)
            Destroy(searchPathPoints[c]);
        searchPathGoal = 1;
        searchPath = null;
    }

    void wait()
    {
        minotaurBody.velocity = Vector3.Lerp(minotaurBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        minotaurBody.rotation = Quaternion.Slerp(minotaurBody.rotation, tempGoal, lerpRotate);
        if (playerVisible == true)
            minotaurFSM.changeState((int)fsmStateRef.growl);
    }

    void growl()
    {
        minotaurBody.velocity = Vector3.Lerp(minotaurBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        minotaurBody.rotation = Quaternion.Slerp(minotaurBody.rotation, tempGoal, lerpRotate);
        if (thinkingInterval == 0)
            print("The minotaur growls.");
        thinkingInterval += Time.deltaTime;
        if (thinkingInterval >= .3f)
        {
            minotaurFSM.changeState((int)fsmStateRef.stalk);
            thinkingInterval = 0;
        }
    }

    void stalk()
    {
        RaycastHit hit;
        LayerMask cLayer = 1 << 8;
        bool chargeable = (!Physics.Raycast(minotaurBody.transform.position, (player.transform.position - minotaurBody.transform.position).normalized,
            out hit, Vector3.Distance(minotaurBody.transform.position, player.transform.position), cLayer) && !Physics.Raycast(player.transform.position, (player.transform.position - minotaurBody.transform.position).normalized,
            out hit, Vector3.Distance(minotaurBody.transform.position, player.transform.position)/4, cLayer)); //check if player is visible
        thinkingInterval += Time.deltaTime;
        if (thinkingInterval >= .25f && chargeable)
        {
            minotaurFSM.changeState((int)fsmStateRef.stalk);
            thinkingInterval = 0;
        }
    }

    void charge()
    {
        minotaurBody.velocity = Vector3.Lerp(minotaurBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        minotaurBody.rotation = Quaternion.Slerp(minotaurBody.rotation, tempGoal, lerpRotate);
    }

    void halt()
    {
        minotaurBody.velocity = Vector3.Lerp(minotaurBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        minotaurBody.rotation = Quaternion.Slerp(minotaurBody.rotation, tempGoal, lerpRotate);
    }

    void punch()
    {
        minotaurBody.velocity = Vector3.Lerp(minotaurBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - minotaurBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        minotaurBody.rotation = Quaternion.Slerp(minotaurBody.rotation, tempGoal, lerpRotate);
    }

    public bool getDangerous()
    {
        return genericInfo.getDangerous();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "SwordBoxB" && attack.getDangerous() && iFrames > .2f)
        {
            minotaurInfo.changeHealth(-2);
            print(minotaurInfo.getHealth());
            iFrames = 0;
        }
        if (collision.gameObject.name == "Knife" && /*collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1 &&*/ iFrames > .2f)
        {
            minotaurInfo.changeHealth(-1);
            print(minotaurInfo.getHealth());
            iFrames = 0;
        }
    }
}