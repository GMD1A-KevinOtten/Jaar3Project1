using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSoldier : Photon.PunBehaviour {
    public int myTeam;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        //if (NWManager.instance.CheckClientTeam(myTeam, PhotonNetwork.player))
        //{
        //    //photonView.RPC("DestroySoldier", PhotonTargets.All);
        //}
    }

    [PunRPC]
    void DestroySoldier()
    {
        Destroy(gameObject);
    }
}
