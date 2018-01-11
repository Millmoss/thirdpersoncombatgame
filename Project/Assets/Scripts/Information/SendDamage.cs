using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDamage : MonoBehaviour
{
    public int damage;
    public GameObject receiveObj;
    public GameObject parentObj;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.name == "HitBoxHead" || collision.gameObject.name == "HitBoxBody") && parentObj.GetComponent<GenericStatus>().getDangerous())
            receiveObj.SendMessage("receiveDamage", damage);
    }
}
