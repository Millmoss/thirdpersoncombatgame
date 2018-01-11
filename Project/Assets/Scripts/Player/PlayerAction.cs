using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public ControllerInput inputScript;
    public PlayerStatus playerInfo;
    public Camera cam;
	public UIScript invenScript;
    private GameObject nearItem;
	private GameObject heldItem;
    private bool held;
    private Rigidbody itemBody;

    void Start ()
    {
		
	}

	public void setHeldItem(GameObject ITEM){
		heldItem = ITEM;
		itemBody = heldItem.GetComponent<Rigidbody>();
		held = true;
		print (ITEM.GetComponent<Item> ());
	}

    void Update()
    {
        if (inputScript.getControlScheme() != 1)
        {
            Collider[] near = Physics.OverlapSphere(transform.position, .8f);
            bool itemNear = false;
            if (held == false)
            {
                for (int i = 0; i < near.Length; i++)
                {
                    if (near[i].tag == "Item")
                    {
                        nearItem = near[i].gameObject;
                        itemNear = true;
                    }
                }
            }
            if (itemNear == false)
                nearItem = null;
            if (inputScript.getInteractPressed())
            {
                if (held == true)
                {
                    held = false;
                    print("press");
                }
                if (nearItem != null)
                {
                    invenScript.addItem(nearItem.GetComponent<Item>());
                    //heldItem = nearItem;
                    //itemBody = heldItem.GetComponent<Rigidbody>();
                    //held = true;
                    Destroy(nearItem);
                }
            }
        }

        if (held == true)
        {
            heldItem.transform.position = transform.position + new Vector3(0, 1, 0);
            itemBody.velocity = Vector3.zero;
			//doesnt seem to work anymore???? will check this later
        }
        if (held == true)
        {
            if (inputScript.getThrowPressed())
            {
                heldItem.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
                if (heldItem.name == "Knife")
                    heldItem.transform.Rotate(Vector3.forward, 90);
                itemBody.angularVelocity = Vector3.zero;
                itemBody.velocity = cam.transform.forward * 10;
                held = false;
            }
            if (inputScript.getConsumePressed())
            {
				print (heldItem.GetComponent<Item>().getItemName());
				if (heldItem.GetComponent<Item>().checkType() == "Consumable")
                {
					playerInfo.changeEnergy(heldItem.GetComponent<Consumable>().getFeedValue());
					playerInfo.changeHealth(heldItem.GetComponent<Consumable>().getHealValue());
					Destroy(heldItem);
                    heldItem = null;
                    held = false;
                }
            }
        }
    }
}