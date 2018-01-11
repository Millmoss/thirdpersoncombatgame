using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

/*

	TEMP COMMANDS: WASD for MOVE; U for SELECT.

*/
public class UIScript : MonoBehaviour
{

	public ControllerInput inputScript;
	public Text statusText;
	public GameObject player;
	private PlayerInventory playerInv;
	public GameObject inventoryMenu;
	public GameObject inventoryPanel;
	public GameObject equipMenu;
	public GameObject handItems;
	public GameObject[] equipPanels;
	public GameObject[,] invePanels;
	public GameObject[] handPanels;
	public Sprite defaultIcon;


	private PlayerStatus playerInfo;
	private Canvas thisCanvas;

	private PlayerAction playerActionScript;

	private int[] curInvPos;
	private int curEquipPos;
	private int edgeBorder = 10;
	bool paused = false;
    private float inputDelay;

	void Start()
	{
		invePanels = new GameObject[PlayerInventory.inventorySize, PlayerInventory.inventorySize];
		playerInv = player.GetComponent<PlayerInventory> ();
		thisCanvas = GetComponent<Canvas> ();
		instantiateInventory ();
		curInvPos = new int[2]{0,0};
		curEquipPos = -1;
		statusText = gameObject.GetComponent<Text>();
		playerInfo = player.GetComponent<PlayerStatus>();
		updatePanels (0,0);
		playerActionScript = player.GetComponent<PlayerAction> ();
        inputDelay = 1;
	}

	//TODO: Make this function check for edge cases that don't work and stuff. Most likely create a new function to handle those though.
	void updatePanels(int newX, int newY){
		if (curEquipPos == -1) {
			invePanels [curInvPos [0], curInvPos [1]].GetComponent<invPanel> ().resetPanel ();
			curInvPos [0] = newX;
			curInvPos [1] = newY;
			invePanels [curInvPos [0], curInvPos [1]].GetComponent<invPanel> ().selectPanel ();
		} else {
			//We're going to use newY for the positions becuase armor is vertical
			//TODO: Toss some inheritance onto these panels so equiping them affects stats and stuff.
			equipPanels[curEquipPos].GetComponent<invPanel>().resetPanel();
			curEquipPos = newY;
			equipPanels[curEquipPos].GetComponent<invPanel>().selectPanel();

		}
	}

	void instantiateInventory(){
		RectTransform inventoryRect = inventoryMenu.GetComponent<RectTransform> ();
		inventoryRect.position = new Vector3(thisCanvas.pixelRect.width - inventoryMenu.GetComponent<RectTransform>().rect.width/2 - edgeBorder,
			thisCanvas.pixelRect.height - inventoryMenu.GetComponent<RectTransform>().rect.height/2 - edgeBorder,
			0);

		RectTransform invPanel = inventoryPanel.GetComponent<RectTransform> ();
		for (int i = 0; i < PlayerInventory.inventorySize; i++) {
			for (int j = 0; j < PlayerInventory.inventorySize; j++) {
				GameObject temp = Instantiate (inventoryPanel, new Vector3 (i*invPanel.rect.width + inventoryRect.position.x,
					j*invPanel.rect.height + inventoryRect.position.y - edgeBorder,
					0), Quaternion.identity);
				temp.transform.SetParent ( GetComponent<Canvas> ().transform);
				invePanels [i, j] = temp;
			}
		}

	}

	public void addItem(Item ITEM){
		int[] pos = playerInv.addItem(ITEM);
		invePanels [pos [0], pos [1]].transform.GetChild (0).GetComponent<Image> ().sprite = ITEM.icon;
	}

	public void getItem(){
		GameObject gottenItem = playerInv.getItem (curInvPos [0], curInvPos [1]);
		if (gottenItem == null)
			return;
		playerActionScript.setHeldItem (gottenItem);
		//TODO: Remove item from inventory / reset icon
		invePanels[curInvPos[0],curInvPos[1]].transform.GetChild(0).GetComponent<Image>().sprite = defaultIcon;
	
	}

	//NOTE : I moved the pause script from ClunkMove to here. This is going to need to be refactored as hell.
	void Update()
	{
        inputDelay += Time.deltaTime;
		if (inputScript.getInventoryPressed() || Input.GetKeyDown(KeyCode.P) == true) 
		{
			if (paused == false) {
				paused = true;
				inventoryMenu.SetActive (true);
				equipMenu.SetActive (true);
				handItems.SetActive (true);
				for (int i = 0; i < PlayerInventory.inventorySize; i++)
					for(int j=0; j < PlayerInventory.inventorySize;j++)
						invePanels [i,j].SetActive (true);
			} else {
				paused = false;
				inventoryMenu.SetActive (false);
				equipMenu.SetActive (false);
				handItems.SetActive (false);
				for (int i = 0; i < PlayerInventory.inventorySize; i++)
					for(int j=0; j < PlayerInventory.inventorySize;j++)
						invePanels [i,j].SetActive (false);
			}
		}

		//Moving target selection controls. 
		//TODO:  Disable player movement upon opening the menu. Also to rebind the KEyboard buttons to the controller.
	
	
		if (paused) {
			if (curEquipPos == -1) {
				//Inventory
				if (inputScript.getXTiltNav() == 1 && inputDelay >= .1f || Input.GetKeyDown (KeyCode.L))
                {
					updatePanels (curInvPos [0] + 1, curInvPos [1]);
                    inputDelay = 0;
				}
				if (inputScript.getXTiltNav() == -1 && inputDelay >= .1f || Input.GetKeyDown (KeyCode.J))
                {
					updatePanels (curInvPos [0] - 1, curInvPos [1]);
                    inputDelay = 0;
                }
				if (inputScript.getZTiltNav() == 1 && inputDelay >= .1f || Input.GetKeyDown (KeyCode.I))
                {
					updatePanels (curInvPos [0], curInvPos [1] + 1);
                    inputDelay = 0;
                }
				if (inputScript.getZTiltNav() == -1 && inputDelay >= .1f || Input.GetKeyDown (KeyCode.K))
                {
					updatePanels (curInvPos [0], curInvPos [1] - 1);
                    inputDelay = 0;
                } 
				if (inputScript.getInteractPressed() || Input.GetKeyDown (KeyCode.U)) {
					//Check if its a consumable. If it is just give it to the player
					if (playerInv.returnType(curInvPos[0],curInvPos[1])=="Consumable") {
						getItem ();
					}
					/*
					curEquipPos = 0;
					updatePanels (0, curEquipPos);
					*/
					//That stuff is for later.
				}
			}
			else {
				//Equips
				if (Input.GetKeyDown (KeyCode.S)) {
					updatePanels (0, curEquipPos+1);
				}
				if (Input.GetKeyDown (KeyCode.W)) {
					updatePanels (0, curEquipPos - 1);
				}
				if (Input.GetKeyDown (KeyCode.U)) {
					curEquipPos = -1;
					updatePanels (curInvPos[0],curInvPos[1]);
				}
			}

		}
		string health = playerInfo.getHealthString();
		string hunger = playerInfo.getHungerString();
		if (hunger == "d")
			hunger = hunger; //modify to end game
		statusText.text = health + "\n" + hunger;
	}
}
