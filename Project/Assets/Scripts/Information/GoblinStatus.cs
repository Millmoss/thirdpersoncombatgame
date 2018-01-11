using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStatus
{
    private int health;
    private int damage;

	public GoblinStatus(int difficulty)
    {
        if (difficulty == 1)
        {
            health = 0;
            for (int i = 0; i < 5; i++)
            {
                health += 2;
            }
            damage = 4;
        }
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