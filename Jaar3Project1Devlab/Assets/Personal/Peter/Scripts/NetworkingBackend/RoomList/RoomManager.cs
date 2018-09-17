using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : Photon.PunBehaviour
{
    public GameObject playersScrollViewContent;
    public Button playerButtonPrefab;

    private void Start()
    {
        photonView.RPC("RefreshPlayers", PhotonTargets.All);
    }



    public void LockRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
        }
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
             button.GetComponentInChildren<Text>().text = p.NickName;
        }
    }


}
