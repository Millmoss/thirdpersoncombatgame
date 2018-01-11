using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurStatus
{
    private int health;
    private int damage;

    public MinotaurStatus()
    {
        health = 30;
        damage = 5;
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