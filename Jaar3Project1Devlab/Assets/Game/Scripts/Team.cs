using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team {

    public bool teamAlive = true;
    public int soldierIndex;
    public int teamNumber;
    public List<Soldier> allSoldiers = new List<Soldier>();

    void Start() 
    {

    }

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
                Debug.Log("But GWy");
                break;
            }
        }
        Debug.Log(soldiersAlive);
        teamAlive = soldiersAlive;
    }
}
