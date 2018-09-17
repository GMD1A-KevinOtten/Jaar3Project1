using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListButton : Photon.PunBehaviour {
    public Text roomNameText;

    private void Awake()
    {
        roomNameText = GetComponentInChildren<Text>();
    }
   public void OnClick()
    {
        PhotonNetwork.JoinRoom(roomNameText.text);
        StartCoroutine(NWManager.instance.WaitBeforeLoad(1));

    }
}
