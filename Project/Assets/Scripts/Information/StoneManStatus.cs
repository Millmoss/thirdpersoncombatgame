using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManStatus
{
    private int health;
    private int damage;

    public StoneManStatus()
    {
        health = 24;
        //for (int i = 0; i < 5; i++)
        //{
            //health += Mathf.FloorToInt(Random.Range(1, 4));
        //}
        damage = 6;
    }

    public int getHealth()
    {
        return health;
    }

    public void changeHealth(int change)
    {
        health += change;
    }

    public int getDamage()
    {
        return damage;
    }
}
