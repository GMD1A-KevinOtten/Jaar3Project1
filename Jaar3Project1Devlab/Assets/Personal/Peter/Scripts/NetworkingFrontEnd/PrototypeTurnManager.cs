using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeTurnManager : Photon.PunBehaviour {

    public int currentPlayerTurn = 1;


    public List<PrototypeSoldier> clientTeamSoldiers = new List<PrototypeSoldier>();

    public int lastMovedSoldierIndex;
    //public GameObject currentSoldier;

    public static PrototypeTurnManager instance;
	// Use this for initialization
	void Awake () {

        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        PrototypeSoldier[] allSoldiers = FindObjectsOfType<PrototypeSoldier>();

        foreach (PrototypeSoldier s in allSoldiers)
        {
            if(s.myTeam == PhotonNetwork.player.ID)
            {
                clientTeamSoldiers.Add(s);
            }
        }

        foreach (PrototypeSoldier q in clientTeamSoldiers)
        {
            q.photonView.RequestOwnership();
        }

    }

    private void Start()
    {
      
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(TurnIntSync.instance.currentPlayerTurn);
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
        //clientTeamSoldiers[0].gameObject.GetComponent<Movement>().canMove = false;
        photonView.RPC("NextTurn", PhotonTargets.All);
        photonView.RPC("PreTurnLimbo", PhotonTargets.All);

        if(TurnIntSync.instance.currentPlayerTurn == PhotonNetwork.player.ID)
        {
            //photonView.RPC("DisableMove", PhotonTargets.Others);
            //clientTeamSoldiers[0].gameObject.GetComponent<PhotonView>().RPC("ChangeCanMove", PhotonTargets.All);
        }
    
    }

    [PunRPC]
    public void NextTurn()
    {
        Debug.Log("NextTurn()");

        if (TurnIntSync.instance.currentPlayerTurn < 4)
        {
            TurnIntSync.instance.currentPlayerTurn += 1;
        }
        else
        {
            TurnIntSync.instance.currentPlayerTurn = 1;
        }

        
    }

    [PunRPC]
    public void PreTurnLimbo()
    {

        //Camera.main.transform.parent = null;
        //Camera.main.transform.position = new Vector3(0, 4, -6);

        photonView.RPC("StartWait", PhotonTargets.All);

        if (currentPlayerTurn == PhotonNetwork.player.ID)
        {
            
            
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
        //Camera.main.transform.SetParent(clientTeamSoldiers[0].gameObject.transform); 
        //Camera.main.transform.localPosition = new Vector3(0, 2, -8);
        //Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }

    [PunRPC]
    public void DisableMove()
    {
        clientTeamSoldiers[lastMovedSoldierIndex].GetComponent<Movement>().canMove = false;
    }
}
