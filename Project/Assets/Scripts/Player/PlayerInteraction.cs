using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerStatus playerInfo;
    //public GoblinAI goblin;
    //public StoneManAI stone;
    private float iFrames;
    private float iInterval;

    void Start ()
    {
        iFrames = .25f;
        iInterval = 0;
	}
	
	void Update ()
    {
        if (playerInfo.getHealth() < 1)
            print("YOU ARE DED");
        if (playerInfo.getHealth() <= 0)
            SceneManager.LoadScene("DedScene");
        iInterval += Time.deltaTime;
	}

    public void receiveDamage(int damage)
    {
        if (iInterval >= iFrames)
        {
            playerInfo.changeHealth(damage);
            iInterval = 0;
        }
    }

    /*void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "TipBinding" && c.GetComponentInParent<GoblinAI>().getDangerous() && iInterval >= iFrames)
        {
            playerInfo.changeHealth(-2);
            iInterval = 0;
        }
        if (c.gameObject.name == "SwordBoxA" && stone.getDangerous() && iInterval >= iFrames)
        {
            playerInfo.changeHealth(-3);
            iInterval = 0;
        }
        if (c.name == "Ladder")
            SceneManager.LoadScene("WinScene");
    }*/
}
