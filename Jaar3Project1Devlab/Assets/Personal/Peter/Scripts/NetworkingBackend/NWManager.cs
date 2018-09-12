using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NWManager : Photon.PunBehaviour {

    public string gameVersion;
    public byte maximumPlayersInRoom = 4;
    public int playerID;


    public List<PhotonPlayer> connectedPlayers = new List<PhotonPlayer>();


    public static NWManager instance;

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

        

        PhotonNetwork.autoJoinLobby = false;

        PhotonNetwork.automaticallySyncScene = true;

    }

    // Update is called once per frame
    void Update () {
	}

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(gameVersion);

    }

    public void ConnectToRoom(InputField roomName)
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public  void CreateRoom(InputField roomName)
    {
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() {MaxPlayers = maximumPlayersInRoom }, null);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
       // photonView.RPC("RemoveConnectedClient", PhotonTargets.All);
        SceneManager.LoadScene(0);
    }

    public override void OnJoinedRoom()
    {
       

        Debug.Log("Player Joined Room");
        //photonView.RPC("AddConnectedClient", PhotonTargets.All);
        SceneManager.LoadScene(1);
    }

   

    public void LockRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }
    }


    //[PunRPC]
    //void AddConnectedClient()
    //{
       
       
    //     playerID = PhotonNetwork.playerList.Length;

        
    //}

    //[PunRPC]
    //void RemoveConnectedClient()
    //{
       
    //    connectedPlayers.Remove(PhotonNetwork.player);

    //}

    //[PunRPC]
    public  bool CheckClientTeam(int team, PhotonPlayer play) //Doesn't quite work yet
    {
       PhotonPlayer clientPlayer = PhotonNetwork.player;
        int playerTeam = PhotonNetwork.player.ID;

        if(play == clientPlayer && team == playerTeam)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
