using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : Photon.PunBehaviour
{
    public GameObject playersScrollViewContent;
    public Button playerButtonPrefab;

    public GameObject startGameButton;

    private void Start()
    {
        photonView.RPC("RefreshPlayers", PhotonTargets.All);

        if (!PhotonNetwork.isMasterClient)
        {
            startGameButton.SetActive(false);
        }
    }



    public void LockRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }
    }

    [PunRPC]
    private void StartGame()
    {
            if(PhotonNetwork.room.PlayerCount > 1)
            {
                StartCoroutine(NWManager.instance.WaitBeforeLoad(2));
            }
    }

    public void StartGameButtonRPC()
    {
        photonView.RPC("StartGame", PhotonTargets.All);
    }

    public void LeaveRoom()
    {
        StartCoroutine(NWManager.instance.WaitBeforeLoad(0));
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        photonView.RPC("RefreshPlayers", PhotonTargets.All);
    }



    [PunRPC]
    public void RefreshPlayers()
    {
        foreach (Transform t in playersScrollViewContent.transform)
        {
            if (t.name.Contains("Clone"))
            {
                Destroy(t.gameObject);
            }
        }

        PhotonPlayer[] connectedPlayers = PhotonNetwork.playerList;

        foreach (PhotonPlayer p in connectedPlayers)
        {
             Button button = Instantiate(playerButtonPrefab, playersScrollViewContent.transform);
            if(p.NickName != "")
            {
                button.GetComponentInChildren<Text>().text = p.NickName;
            }
        }
    }


}
