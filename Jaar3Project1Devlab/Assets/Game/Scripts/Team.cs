using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team {

    public int soldierIndex;
    public List<Soldier> allSoldiers = new List<Soldier>();

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
    }
}
