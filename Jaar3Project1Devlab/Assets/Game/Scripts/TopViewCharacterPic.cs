using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopViewCharacterPic : MonoBehaviour {

	public Image characterPic;
	public Image currentCharacterDot;


    private void Start()
    {

       
    }

	//This is old and only use for refrence incase needed
    void Update () 
	{

        if (characterPic.color != TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].thisTeamColor)
        {
            characterPic.color = TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].thisTeamColor; //Ja dit moet in de update omdat de int pas later dan start wordt geassigned
        }
        if(currentCharacterDot.color != TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].thisTeamColor)
        {
            currentCharacterDot.color = TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].thisTeamColor;
        }


        if (TeamManager.instance.mainCamera.cameraState == CameraMovement.CameraStates.Topview)
		{
			characterPic.enabled = true;
			transform.LookAt(Camera.main.transform);
			if(transform.root.GetComponent<Soldier>().myTeam == TeamManager.instance.teamIndex)
			{
				if(TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].allSoldiers[TeamManager.instance.allTeams[transform.root.GetComponent<Soldier>().myTeam].soldierIndex] == transform.root.GetComponent<Soldier>())
				{
					currentCharacterDot.enabled = true;
				}
			}
			else if(transform.root.GetComponent<Soldier>().myTeam != TeamManager.instance.teamIndex)
			{
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
