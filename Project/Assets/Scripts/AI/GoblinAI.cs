using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    public GameObject path;
    public Rigidbody goblinBody;
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
    private GoblinStatus goblinInfo;
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
    private FSM goblinFSM;
    private enum fsmStateRef { patrol, search, backtrace, stand, approach, circle, run, wait };
    public int stateDebug;
    private bool reverseAttack;

    void Start ()
    {
        genericInfo = GetComponent<GenericStatus>();
        reverseAttack = false;
        goblinInfo = new GoblinStatus(1);
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
        //goblin can patrol, search, backtrace, stand, approach, circle, run, and wait
        //correlates to   0,      1,         2,     3,        4,      5, 6,       7
        bool[][] ins = new bool[8][];
        ins[0] = new bool[8];
        Array.Clear(ins[0], 0, 7);
        ins[0][3] = true;
        ins[1] = new bool[8];
        Array.Clear(ins[1], 0, 7);
        ins[1][2] = true;
        ins[1][3] = true;
        ins[1][7] = true;
        ins[2] = new bool[8];
        Array.Clear(ins[2], 0, 7);
        ins[2][0] = true;
        ins[2][3] = true;
        ins[3] = new bool[8];
        Array.Clear(ins[3], 0, 7);
        ins[3][1] = true;
        ins[3][4] = true;
        ins[3][5] = true;
        ins[3][6] = true;
        ins[4] = new bool[8];
        Array.Clear(ins[4], 0, 7);
        ins[4][1] = true;
        ins[4][5] = true;
        ins[4][6] = true;
        ins[5] = new bool[8];
        Array.Clear(ins[5], 0, 7);
        ins[5][1] = true;
        ins[5][4] = true;
        ins[5][6] = true;
        ins[6] = new bool[8];
        Array.Clear(ins[6], 0, 7);
        ins[6][1] = true;
        ins[6][4] = true;
        ins[6][5] = true;
        ins[7] = new bool[8];
        Array.Clear(ins[7], 0, 7);
        ins[7][0] = true;
        ins[7][3] = true;
        goblinFSM = new FSM(0, ins);
        #endregion FSM Instantiation
    }

    void FixedUpdate ()
    {
        stateDebug = goblinFSM.getState();

        if (goblinInfo.getHealth() < 1)
        {
            Destroy(transform.parent.gameObject);
        }

        determineVisible();
        determineSituation(); //changes state if need be

        switch (goblinFSM.getState())
        {
            case (int)fsmStateRef.patrol:
                patrol();
                break;
            case (int)fsmStateRef.search:
                search();
                break;
            case (int)fsmStateRef.backtrace:
                backtrace();
                break;
            case (int)fsmStateRef.stand:
                stand();
                break;
            case (int)fsmStateRef.approach:
                approach();
                break;
            case (int)fsmStateRef.circle:
                circle();
                break;
            case (int)fsmStateRef.run:
                run();
                break;
            case (int)fsmStateRef.wait:
                wait();
                break;
        }
        aiAttack();
        weapon.transform.rotation = goblinBody.rotation;
        weapon.transform.Rotate(new Vector3(-11, -7, 0));

        /*if (!playerVisible)
        {
            lerpRotate = .05f;
            aiNormalProcess();
        }
        else if (playerVisible)
        {
            Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - goblinBody.transform.position).normalized;
            
            //rotate towards player
            Quaternion tempCurrent = goblinBody.rotation;
            Quaternion tempGoal = Quaternion.LookRotation(goalDirection); //make this work better
            tempGoal = Quaternion.Euler(new Vector3(0, tempGoal.eulerAngles.y, 0));
            goblinBody.rotation = Quaternion.Slerp(tempCurrent, tempGoal, lerpRotate);

            aiThinking();
            dangerProcess();
            aiAttack();
        }*/
        iFrames += Time.deltaTime;
    }

    void determineVisible()
    {
        RaycastHit hit;
        playerVisible = !Physics.Raycast(goblinBody.transform.position, (player.transform.position - goblinBody.transform.position).normalized,
            out hit, Vector3.Distance(goblinBody.transform.position, player.transform.position), collisionLayer); //check if player is visible
        /*if (playerVisible == false && lastVisible == true) //player was recently visible, but is now no longer visible, so ai must find a path to the player's last assumed position
        {
            followingPath = true;
            chasing = false;
            running = false;
            attacking = false;
            thinkingInterval = 0;
            patrolPathGoal++;
            if (patrolPathGoal >= pathSize)
                patrolPathGoal = 1;
        }

        if (playerVisible && followingPath) //player was recently invisible, but upon following a path to the player's last assumed position, the player was seen
        {
            followingPath = false;
            for (int c = 0; c < pathfindPath.Length; c++)
            {
                Destroy(searchPathPoints[c]);
            }
            stepsChased = 0;
            stepChasing = 0;
            searchPathGoal = 1;
        }*/
    }

    void determineSituation()
    {
        int state = goblinFSM.getState();
        if (playerVisible == false)
        {
            lerpRotate = .05f;
            if (state == (int)fsmStateRef.patrol)
            {
                return;
            }
            else if (state == (int)fsmStateRef.backtrace)
            {
                return;
            }
            else if (state == (int)fsmStateRef.wait)
            {
                return;
            }
            else
            {
                goblinFSM.changeState((int)fsmStateRef.search);
            }
        }
        else
        {
            if (goblinFSM.changeState((int)fsmStateRef.stand) == ((int)fsmStateRef.stand))
            {
                if (state == (int)fsmStateRef.search)
                {
                    cleanSearchPath();
                }
                thinkingInterval = 0;
            }
            float playerDist = Vector3.Distance(player.transform.position, goblinBody.transform.position);
            if (playerDist > 2.5f && thinkingInterval >= .5f)
            {
                goblinFSM.changeState((int)fsmStateRef.approach);
                thinkingInterval = 0;
                lerpRotate = .1f;
            }
            else if (playerDist <= 2.5f && playerDist > 2f && thinkingInterval >= .1f)
            {
                goblinFSM.changeState((int)fsmStateRef.circle);
                thinkingInterval = 0;
            }
            else if (playerDist <= 2f && thinkingInterval >= .5f)
            {
                goblinFSM.changeState((int)fsmStateRef.run);
                thinkingInterval = 0;
                lerpRotate = .15f;
            }
            else
            {
                thinkingInterval += Time.deltaTime;
            }
            if (attackTime >= .8f && playerDist <= 2.25)
            {
                if (!reverseAttack)
                    attackTime = 0f;
            }
        }
    }

    void patrol()
    {
        Vector3 goalDirection = (pathPoints[patrolPathGoal] - transform.position).normalized;
        Quaternion tempCurrent = goblinBody.rotation;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        goblinBody.rotation = Quaternion.Slerp(tempCurrent, tempGoal, lerpRotate);
        goblinBody.velocity = goblinBody.rotation * Vector3.forward * speed;
        Collider[] near = Physics.OverlapSphere(transform.position, .8f);
        for (int i = 0; i < near.Length; i++)
        {
            if (near[i].gameObject.tag == "PatrolPathPoint")
            {
//                print(patrolPathGoal);
                if (near[i].gameObject.name == (patrolPathGoal).ToString())
                {
                    patrolPathGoal++;
                    if (patrolPathGoal >= pathSize)
                        patrolPathGoal = 1;
                }
            }
        }
    }

    void search()
    {
        if (searchPath == null) //get path to last known position
        {
            searchPath = pathfindAccess.FindPath(goblinBody.position, player.transform.position);
            searchPathPoints = new GameObject[searchPath.Length];
            for (int i = searchPath.Length; i > 0; i--)
            {
                if (searchPath.Length == 0)
                    break;
                searchPathPoint.name = (i).ToString();
                searchPathPoint.transform.position = searchPath[i - 1];
                searchPathPoints[i - 1] = Instantiate(searchPathPoint);
            }
        }

        if (searchPath.Length != 0)
        {
            Vector3 goalDirection = (searchPathPoints[searchPathGoal - 1].transform.position - transform.position).normalized;
            print("searchpath" + (searchPathGoal - 1));
            Quaternion tempCurrent = goblinBody.rotation;
            Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
            goblinBody.rotation = Quaternion.Slerp(tempCurrent, tempGoal, lerpRotate);
            goblinBody.velocity = goblinBody.rotation * Vector3.forward * speed;
            Collider[] near = Physics.OverlapSphere(transform.position, .8f);
            for (int i = 0; i < near.Length; i++)
            {
                if (near[i].gameObject.tag == "SearchPathPoint")
                    if (near[i].gameObject.name == ((searchPathGoal).ToString() + "(Clone)"))
                    {
                        searchPathGoal++;
                        if (searchPathGoal >= searchPath.Length) //has reached the player's last known position
                        {
                            cleanSearchPath();
                            i = near.Length;
                        }
                    }
            }
        }
        else
        {
            goblinFSM.changeState((int)fsmStateRef.wait);
            cleanSearchPath();
            thinkingInterval = 0;
        }
    }

    void cleanSearchPath()
    {
        for (int c = 0; c < searchPath.Length; c++)
            Destroy(searchPathPoints[c]);
        searchPathGoal = 1;
        searchPath = null;
    }

    void backtrace()
    {
        goblinFSM.changeState((int)fsmStateRef.patrol);
        if (patrolPathGoal >= pathSize)
            patrolPathGoal = 1;
    }

    void stand()
    {
        goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, Vector3.zero, .15f);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - goblinBody.transform.position).normalized;
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection);
        goblinBody.rotation = Quaternion.Slerp(goblinBody.rotation, tempGoal, lerpRotate);
    }

    void approach()
    {
        float playerDist = Vector3.Distance(player.transform.position, goblinBody.transform.position);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - goblinBody.transform.position).normalized;
        goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, goalDirection * 6, .15f);
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection); //make this work better
        tempGoal = Quaternion.Euler(new Vector3(0, tempGoal.eulerAngles.y, 0));
        goblinBody.rotation = Quaternion.Slerp(goblinBody.rotation, tempGoal, lerpRotate);
    }

    void circle()
    {
        float playerDist = Vector3.Distance(player.transform.position, goblinBody.transform.position);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - goblinBody.transform.position).normalized;
        goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, Vector3.zero, .15f);
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection); //make this work better
        tempGoal = Quaternion.Euler(new Vector3(0, tempGoal.eulerAngles.y, 0));
        goblinBody.rotation = Quaternion.Slerp(goblinBody.rotation, tempGoal, lerpRotate);
    }

    void run()
    {
        float playerDist = Vector3.Distance(player.transform.position, goblinBody.transform.position);
        Vector3 goalDirection = ((player.transform.position - new Vector3(0, 0.3088f, 0)) - goblinBody.transform.position).normalized;
        goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, -goalDirection * 3, .15f);
        if (reverseAttack)
            goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, Vector3.zero, .5f);
        Quaternion tempGoal = Quaternion.LookRotation(goalDirection); //make this work better
        tempGoal = Quaternion.Euler(new Vector3(0, tempGoal.eulerAngles.y, 0));
        if (!reverseAttack)
            goblinBody.rotation = Quaternion.Slerp(goblinBody.rotation, tempGoal, lerpRotate);
        else
            goblinBody.rotation = Quaternion.Slerp(goblinBody.rotation, tempGoal, lerpRotate/3);
    }

    void wait()
    {
        goblinBody.velocity = Vector3.Lerp(goblinBody.velocity, Vector3.zero, .15f);
        if (playerVisible)
        {
            if (pathfindAccess.FindPath(goblinBody.position, player.transform.position).Length == 0)
                thinkingInterval = 0;
            else
                goblinFSM.changeState((int)fsmStateRef.search);
        }
        if (thinkingInterval >= 5)
            goblinFSM.changeState((int)fsmStateRef.patrol);
    }

    void aiAttack()
    {
        if (!reverseAttack)
        {
            if (attackTime < .8f)
            {
                if (attackTime < .15f)
                {
                    genericInfo.setDangerous(true);
                    weapon.transform.localPosition = Vector3.Lerp(weaponPos, new Vector3(weaponPos.x - .3f, weaponPos.y + .4f, weaponPos.z + 1.5f), attackTime / .15f);
                }
                else if (attackTime >= .25f)
                {
                    genericInfo.setDangerous(false);
                    weapon.transform.localPosition = Vector3.Lerp(new Vector3(weaponPos.x - .3f, weaponPos.y + .4f, weaponPos.z + 1.5f), weaponPos, (attackTime - .25f) / .25f);
                }
                attackTime += Time.deltaTime;
            }
        }
        else
        {
            goblinFSM.changeState((int)fsmStateRef.run);
            thinkingInterval = 0;
            if (attackTime >= 0 && attackTime < .5f)
            {
                weapon.transform.localPosition = Vector3.Lerp(reverseStart, Vector3.Lerp(reverseStart, weaponPos, .25f), attackTime / .5f);
            }
            else if (attackTime >= .5f && attackTime < .6f)
            {
                weapon.transform.localPosition = Vector3.Lerp(reverseStart, weaponPos, (attackTime - .45f) / .2f);
            }
            else
            {
                reverseAttack = false;
                attackTime = .8f;
            }
            if (reverseAttack)
                attackTime += Time.deltaTime;
        }
    }

    public bool getDangerous()
    {
        return genericInfo.getDangerous();
    }

    public void parried(Collider collision)
    {
        if (!reverseAttack && attackTime < .8f && attack.getParrying())
        {
            attackTime = 0;
            reverseAttack = true;
            reverseStart = weapon.transform.localPosition;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "SwordBoxB" && attack.getDangerous() && iFrames > .2f)
        {
            goblinInfo.changeHealth(-2);
            print(goblinInfo.getHealth());
            iFrames = 0;
        }
        if (collision.gameObject.name == "Knife" && /*collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1 &&*/ iFrames > .2f)
        {
            goblinInfo.changeHealth(-1);
            print(goblinInfo.getHealth());
            iFrames = 0;
        }
    }
}