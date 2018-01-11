using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invPanel : MonoBehaviour {

	Color selectedColor = Color.red;
	Color defaultColor = Color.black;
	//rn the color just selects the border color.

	// Use this for initialization
	void Start () {

	}

	public void selectPanel()
	{
		GetComponent<Image> ().color = selectedColor;
	}

	public void resetPanel(){
		GetComponent<Image> ().color = defaultColor;
	}

	// Update is called once per frame
	void Update () {

	}
}
