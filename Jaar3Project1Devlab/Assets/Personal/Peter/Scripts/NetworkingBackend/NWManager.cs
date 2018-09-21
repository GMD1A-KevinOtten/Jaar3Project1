using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NWManager : Photon.PunBehaviour {

    public string gameVersion;

    public bool playingMultiplayer;


    public List<PhotonPlayer> connectedPlayers = new List<PhotonPlayer>();


    public static NWManager instance;

    public bool loading = false;
	// Use this for initialization
	void Start () {
        Connect();

	}

    void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        

        PhotonNetwork.autoJoinLobby = true;

        PhotonNetwork.automaticallySyncScene = false;

    }

    // Update is called once per frame
    void Update () {
	}

    void Connect()
    {
        if (playingMultiplayer)
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }

    }



    public  IEnumerator WaitBeforeLoad(int sceneIndex)
    {
        if (!loading)
        {
            loading = true;
            yield return new WaitForSeconds(2);
            PhotonNetwork.LoadLevel(sceneIndex);
            loading = false;
        }
  
    }

    public void ChangeUserName(InputField nameInput)
    {
        PhotonNetwork.player.NickName = null;
        PhotonNetwork.player.NickName = nameInput.text;
    }

}
