using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item {

	protected int protectValue;
	//needs to store hitbox data? i have no idea how... apparently store collider?


	// Use this for initialization
	void Start () {
		type = "Armor";
		itemName = "itemArmor";
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
