using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSoldiers : MonoBehaviour {

	public List<GameObject> soldierPrefabs = new List<GameObject>();
	public List<GameObject> instantiationCords = new List<GameObject>();
	public List<GameObject> starterWeapons =  new List<GameObject>();

	void Start () 
	{
		InstantiateSoldierPrefabs(TeamManager.instance.teamCount);
	}

	public void InstantiateSoldierPrefabs(int amountOfTeams)
	{
		for (int team = 0; team < amountOfTeams; team++)
		{
			TeamManager.instance.allTeams.Add(new Team());
			for (int e = 0; e < 4; e++)
			{
				GameObject newSoldier = Instantiate(soldierPrefabs[team], instantiationCords[team].transform.GetChild(e).transform.position, Quaternion.identity);
				Soldier newSoldierInfo = newSoldier.GetComponent<Soldier>();
				TeamManager.instance.allTeams[team].allSoldiers.Add(newSoldierInfo);

				if(TeamManager.instance.allTeams[team].thisTeamColor != newSoldierInfo.teamColor)
				{
					TeamManager.instance.allTeams[team].thisTeamColor = newSoldierInfo.teamColor;
				}

				//all soldiers get a pistol
				foreach (GameObject weapon in starterWeapons)
				{
					if(weapon.GetComponent<Makarov>())
					{
						newSoldierInfo.StarterWeaponPrefabs.Add(weapon);
					}
				}
				switch (e)
				{
					// soldier 0 adn 1 get the special weapons (sniper and shotgun) the other 2 get the smg(Tommygun)
					case 0:
						foreach (GameObject weapon in starterWeapons)
						{
							if(weapon.GetComponent<Sniper>())
							{
								newSoldierInfo.StarterWeaponPrefabs.Add(weapon);
							}
						}
						break;
					case 1:
						foreach (GameObject weapon in starterWeapons)
						{
							if(weapon.GetComponent<Blunderbus>())
							{
								newSoldierInfo.StarterWeaponPrefabs.Add(weapon);
							}
						}
						break;
					default:
						foreach (GameObject weapon in starterWeapons)
						{
							if(weapon.GetComponent<TommyGun>())
							{
								newSoldierInfo.StarterWeaponPrefabs.Add(weapon);
							}
						}
						break;
				}
			}
		}
	}
}
