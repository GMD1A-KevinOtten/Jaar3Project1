using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeScript : Photon.PunBehaviour {

    //SCRIPT FOR MULTIPLAYER PROTOTYPE ONLY
    //NOT ACTUALLY GOING TO BE USED IN THE GAME
    //THIS IS JUST A PROOF OF CONCEPT TO SHOW WORKING MULTIPLAYER


   

    public GameObject playerPrefab;
	// Use this for initialization
	void Awake () {

       GameObject g = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void FindNWManager()
    {
        NWManager.instance.LockRoom();
    }


}
