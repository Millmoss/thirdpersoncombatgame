using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	protected int damage;
    
	void Start ()
    {
		type = "Weapon";
		itemName = "itemWeapon";
        damage = 0;
	}
	
	void Update ()
    {
		
	}

    public int getDamage()
    {
        return damage;
    }
}