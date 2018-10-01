using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewCharacterPic : MonoBehaviour {


	void Update () 
	{
		transform.LookAt(Camera.main.transform);
		if(transform.root.GetComponent<Soldier>().myTeam == TeamManager.instance.teamIndex && GetComponentInChildren<Image>().color != Color.green)
		{
			GetComponentInChildren<Image>().color = Color.green;
		}
		else if(transform.root.GetComponent<Soldier>().myTeam != TeamManager.instance.teamIndex && GetComponentInChildren<Image>().color != Color.red)
		{
			GetComponentInChildren<Image>().color = Color.red;
		}
	}
}
