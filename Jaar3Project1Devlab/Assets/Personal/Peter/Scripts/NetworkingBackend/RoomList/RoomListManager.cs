using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListManager : Photon.PunBehaviour {

    public List<RoomInfo> rooms = new List<RoomInfo>();
    public byte maxPlayersInRoom = 4;

    public Button roomButtonPrefab;
    public GameObject roomScrollViewContent;

    public InputField playerNameInput;

    public RoomListManager instance;

    private void Awake()
    {
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
       
        
    }

    public void ChangeName()
    {
        if (PhotonNetwork.player.NickName != null)
        {
            playerNameInput.text = PhotonNetwork.player.NickName;
            playerNameInput.enabled = false;
        }
    }

    public void CreateRoom(InputField roomName)
    {
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() { MaxPlayers = maxPlayersInRoom, IsVisible = true, IsOpen = true }, null);
        StartCoroutine(NWManager.instance.WaitBeforeLoad(1));
        
    }

    public void RefreshList()
    {
        Debug.Log("Refreshing");
        rooms.Clear();

        if(roomScrollViewContent != null)
        {
            foreach (Transform t in roomScrollViewContent.transform)
            {
                if (t.name.Contains("Clone"))
                {
                    Destroy(t.gameObject);
                }
            }
        }
       

        RoomInfo[] roums = PhotonNetwork.GetRoomList();

        foreach (RoomInfo r in roums)
        {
            Debug.Log(r);
            if (!rooms.Contains(r))
            {
                rooms.Add(r);
            }
        }

        if(rooms.Count > 0)
        {
            foreach (RoomInfo room in rooms)
            {
                if(room != null && roomButtonPrefab != null && roomScrollViewContent != null)
                {
                    Button b = Instantiate(roomButtonPrefab, roomScrollViewContent.transform);
                    if(b != null)
                    {
                        b.GetComponentInChildren<Text>().text = room.Name;
                    }
                }
              
            }
        }
      

        Debug.Log("Refreshed");
    }

    public override void OnReceivedRoomListUpdate()
    {
        RefreshList();
    }

  

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            playerNameInput = GameObject.Find("PlayerNameInputField").GetComponent<InputField>();
            roomScrollViewContent = GameObject.Find("RoomListContent");

            ChangeName();

            RefreshList();
        }
    }


}
