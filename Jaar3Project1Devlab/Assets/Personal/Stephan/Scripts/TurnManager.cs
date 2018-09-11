using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public TurnManager instance;

    public int teamIndex;
    public List<Team> allTeams = new List<Team>();

    private void Awake()
    {
        //Makes a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextTeam()
    {
        if (teamIndex + 1 < allTeams.Count)
        {
            teamIndex += 1;
        }
        else
        {
            teamIndex = 0;
        }
    }
}
