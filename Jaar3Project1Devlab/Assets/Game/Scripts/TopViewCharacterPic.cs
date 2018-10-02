using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewCharacterPic : MonoBehaviour {

	public Image characterPic;
	public Image currentCharacterDot;

	void Update () 
	{
		if(TeamManager.instance.mainCamera.cameraState == CameraMovement.CameraStates.Topview)
		{
			characterPic.enabled = true;
			transform.LookAt(Camera.main.transform);
			if(transform.root.GetComponent<Soldier>().myTeam == TeamManager.instance.teamIndex && GetComponentInChildren<Image>().color != Color.green)
			{
				characterPic.color = Color.green;
				if(TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].allSoldiers[TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].soldierIndex] == transform.root.GetComponent<Soldier>())
				{
					print("ThisSoldier");
					currentCharacterDot.color = Color.green;
					currentCharacterDot.enabled = true;
				}
			}
			else if(transform.root.GetComponent<Soldier>().myTeam != TeamManager.instance.teamIndex && GetComponentInChildren<Image>().color != Color.red)
			{
				characterPic.color = Color.red;
				currentCharacterDot.enabled = false;
			}
		}
		else if(characterPic.enabled)
		{
			characterPic.enabled = false;
			currentCharacterDot.enabled = false;
		}
		


		if(transform.root.GetComponent<Soldier>().isDead)
		{
			gameObject.SetActive(false);
		}
	}
}
