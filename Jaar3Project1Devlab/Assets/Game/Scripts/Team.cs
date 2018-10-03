using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team {

    public bool teamAlive = true;
    public int soldierIndex;
    public List<Soldier> allSoldiers = new List<Soldier>();
    public Color thisTeamColor;

    public void NextSoldier()
    {
        if (soldierIndex + 1 < allSoldiers.Count)
        {
            soldierIndex += 1;
        }
        else
        {
            soldierIndex = 0;
        }
        if(teamAlive == true && allSoldiers[soldierIndex].isDead == true)
        {
                NextSoldier();
        }
    }

    public void CheckTeam()
    {
        bool soldiersAlive = false;
        foreach (Soldier soldier in allSoldiers)
        {
            if(soldier.isDead != true)
            {
                soldiersAlive = true;
                break;
            }
        }
        teamAlive = soldiersAlive;
    }

    public bool SoldierCheck(Soldier checkThis)
    {
        bool isHere = false;

        foreach (Soldier sold in allSoldiers)
        {
            if(sold == checkThis)
            {
                isHere = true;
                CheckTeam();
                break;
            }
        }
        return isHere;
    }
}
