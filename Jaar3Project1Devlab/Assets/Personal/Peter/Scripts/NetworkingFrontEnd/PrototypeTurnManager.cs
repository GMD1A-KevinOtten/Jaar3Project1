using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeTurnManager : Photon.PunBehaviour {

    public int currentPlayerTurn;


    public List<PrototypeSoldier> clientTeamSoldiers = new List<PrototypeSoldier>();

    public int lastMovedSoldierIndex;
	// Use this for initialization
	void Awake () {
        PrototypeSoldier[] allSoldiers = FindObjectsOfType<PrototypeSoldier>();

        foreach (PrototypeSoldier s in allSoldiers)
        {
            if(s.myTeam == PhotonNetwork.player.ID)
            {
                clientTeamSoldiers.Add(s);
            }
        }

    }

    private void Start()
    {
        photonView.RPC("EndTurn", PhotonTargets.All);
    }

    // Update is called once per frame
    void Update () {
		
	}

 

    public void ButtonCall()
    {
        Debug.Log("ButtonCall");
        photonView.RPC("EndTurn", PhotonTargets.All);
    }

    [PunRPC]
    public void EndTurn()
    {
        Debug.Log("EndTurn()");
        photonView.RPC("NextTurn", PhotonTargets.All);
        photonView.RPC("PreTurnLimbo", PhotonTargets.All);
    }

    [PunRPC]
    public void NextTurn()
    {
        Debug.Log("NextTurn()");

        if (currentPlayerTurn < 4)
        {
            currentPlayerTurn += 1;
        }
        else
        {
            currentPlayerTurn = 0;
        }
    }

    [PunRPC]
    public void PreTurnLimbo()
    {
        Camera.main.transform.parent = null;
        Camera.main.transform.position = new Vector3(0, 4, -6);

        photonView.RPC("StartWait", PhotonTargets.All);

        if (currentPlayerTurn == PhotonNetwork.player.ID)
        {
            clientTeamSoldiers[lastMovedSoldierIndex].GetComponent<Movement>().canMove = true;

            PrototypeSoldier[] soldats = FindObjectsOfType<PrototypeSoldier>();

            foreach (PrototypeSoldier s in soldats)
            {
                if (s != clientTeamSoldiers[lastMovedSoldierIndex])
                {
                    s.GetComponent<Movement>().canMove = false;
                }
            }
        }
    }

    [PunRPC]
    void StartWait()
    {
        StartCoroutine(WaitFewSeconds());
    }

    IEnumerator WaitFewSeconds()
    {
        yield return new WaitForSeconds(2);
        Camera.main.transform.SetParent(clientTeamSoldiers[lastMovedSoldierIndex].transform); 
        Camera.main.transform.localPosition = new Vector3(0, 2, -6);
        Camera.main.transform.localRotation = new Quaternion(20, 0, 0, 0);
    }
}
