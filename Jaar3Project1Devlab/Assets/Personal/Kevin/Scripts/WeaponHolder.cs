using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour {

	public GameObject equipedWeapon;

	void Start () 
	{
		Instantiate(equipedWeapon,transform.position,Quaternion.identity);
	}
}
