using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeTurnManager : Photon.PunBehaviour {


    public PhotonPlayer currentPlayer;

    public List<Color> playerColors = new List<Color>();
    public Image playerAvatar;
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

      

    }

    private void Start()
    {
        if(currentPlayer == null)
        {
            currentPlayer = PhotonNetwork.masterClient;
        }

        playerAvatar.color = new Color(playerColors[PhotonNetwork.player.ID - 1].r, playerColors[PhotonNetwork.player.ID - 1].g, playerColors[PhotonNetwork.player.ID - 1].b, 1); //Temporary client identification
        
    }

    // Update is called once per frame
    void Update () {
       if(currentPlayer == PhotonNetwork.player)
        {
            if (Input.GetKeyDown("n"))
            {
                CallNextTurn();
            }
            if (Input.GetKeyDown("c"))
            {
                PhotonNetwork.Instantiate("Koob", new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)), Quaternion.identity, 0);
            }
        }
	}

    public void CallNextTurn()
    {
        photonView.RPC("NextTurn", PhotonTargets.All);
    }

    [PunRPC]
    void NextTurn()
    {
        currentPlayer = PhotonNetwork.player.GetNextFor(currentPlayer);

    }




}
