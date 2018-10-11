using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrint : MonoBehaviour {

	private void OnCollisionEnter(Collision other) 
	{
		if(other.transform.GetComponent<Bullet>())
		{
			print("Wowzers");
		}
	}
}
