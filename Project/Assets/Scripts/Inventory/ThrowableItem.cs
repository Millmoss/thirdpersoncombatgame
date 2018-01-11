using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : Consumable{

	protected int damage;
	protected int weight;

	// Use this for initialization
	void Start () {
		type = "ThrowableItem";
		itemName = "itemThrow";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
