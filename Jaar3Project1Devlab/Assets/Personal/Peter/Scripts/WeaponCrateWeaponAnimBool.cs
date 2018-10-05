using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrateWeaponAnimBool : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetThatBool()
    {
        GetComponent<Animator>().SetBool("MoveUp", true);
    }
}
