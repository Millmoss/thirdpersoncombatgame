using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

	public string itemName;
	protected string type;
	protected int height;
    protected int width;
	protected int weight;
	public Sprite icon;
	private GameObject obj;

	// Use this for initialization
	void Start () {
		type = "Item";
		itemName = "itemTemp";
		height = 1;
        width = 1;
		obj = gameObject;
	}

	public GameObject getObject(){
		return obj;
	}

	public void setObject(){
		obj = gameObject;

	}

	public string getItemName()
	{
		return itemName;
	}

	virtual public string checkType()
	{
		return "None";
	}

}
