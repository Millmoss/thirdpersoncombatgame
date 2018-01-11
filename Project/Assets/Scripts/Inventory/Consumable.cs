using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {

	protected int feedValue;
	protected int healValue;
	// Use this for initialization
	void Start () {
		type = "Consumable";
		itemName = "itemConsume";
		feedValue = 1;
		healValue = 1;
	}

	override public string checkType(){
		return "Consumable";
	}

	// Update is called once per frame
	void Update () {
		
	}

	public int getFeedValue(){
		return feedValue;
	}

	public int getHealValue(){
		return healValue;
	}
}
