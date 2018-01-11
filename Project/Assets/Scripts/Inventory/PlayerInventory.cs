using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

	public static int inventorySize = 5;
	public GameObject[] items;
	private Dictionary<string, GameObject> itemDict;

	public string[,] inventory;
	// Use this for initialization
	void Start () {
		inventory = new string[inventorySize, inventorySize];
		for (int i = 0; i < inventorySize; i++)
			for (int j = 0; j < inventorySize; j++)
				inventory [i, j] = "itemTemp";
		//itemTemp is the default name of an ITEM object.
		itemDict = new Dictionary<string, GameObject> ();
		for (int i = 0; i < items.Length; i++) {
			items [i].GetComponent<Item> ().setObject ();
			itemDict.Add (items [i].GetComponent<Item> ().name, items [i]);
		}
	}

	public GameObject getItem(int X, int Y)
	{
		string temp = inventory [X, Y];
		if (temp == "itemTemp")
			return null;
		inventory [X, Y] = "itemTemp";
		GameObject objecto = Instantiate(itemDict[temp]);
		print (objecto.GetComponent<Item> ().getItemName());
		objecto.name = objecto.GetComponent<Item>().getItemName();
		return objecto;
	}

	public string returnType(int X, int Y)
	{
		if (inventory [X, Y] == "itemTemp")
			return null;
		return itemDict [inventory [X, Y]].GetComponent<Item> ().checkType ();
	}

	public void setItem(int X, int Y, string ITEM){
		inventory [X, Y] = ITEM;
	}

	//TODO: fix when full inventory
	public int[] addItem(Item ITEM){
		bool breakplz = false;
		int itemX = 0;
		int itemY = 0;
		for (int j = 0; j < inventorySize; j++) {
			for (int i = 0; i < inventorySize; i++) {
				if (inventory [i, j].Equals("itemTemp")) {
					itemX = i;
					inventory [i, j] = ITEM.name;
					breakplz = true;
					break;
				}
			}
			if (breakplz) {
				itemY = j;
				break;
			}
		}
		print (inventory [itemX, itemY]+" inv");
		print (ITEM);
		return new int[2]{ itemX, itemY };



	}

	// Update is called once per frame
	void Update () {
		
	}
		
}
