using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : Photon.PunBehaviour {

    public static TeamManager instance;
    private bool runningIenumerator;

    [Header("Camera proporties")]
    public CameraMovement mainCamera;
    public Transform cameraPositionSky;
    public float movementSpeed;

    [Header("Team Proporties")]
    public int teamIndex;
    public List<Team> allTeams = new List<Team>();

    [Header("Networking")]
    public PhotonPlayer currentPlayer;

    public List<Color> playerColors = new List<Color>();
    public Image playerAvatar; //Temporary

    private void Awake()
    {
        //Makes a Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCamera = GameObject.FindObjectOfType<CameraMovement>();
    }

    private void Start()
    {
        if(currentPlayer == null)
        {
            currentPlayer = PhotonNetwork.masterClient;
        }

        if(playerAvatar != null)
        {
            playerAvatar.color = new Color(playerColors[PhotonNetwork.player.ID - 1].r, playerColors[PhotonNetwork.player.ID - 1].g, playerColors[PhotonNetwork.player.ID - 1].b, 1); //Temporary client identification
        }
    }

    void Update()
    {
        if(mainCamera.cameraState == CameraMovement.CameraStates.Topview)
        {
            if(NWManager.instance.playingMultiplayer)
            {
                if(Input.GetButtonDown("Enter") && currentPlayer == PhotonNetwork.player)
                {
                    Debug.Log("TeamIndex = " + teamIndex);

                    photonView.RPC("ToSoldier", PhotonTargets.All);
                }
            }
            else
            {
                if(Input.GetButtonDown("Enter"))
                {
                    ToSoldier();
                }
            }
        }
        if(mainCamera.cameraState == CameraMovement.CameraStates.ThirdPerson)
        {
            if(NWManager.instance.playingMultiplayer)
            {
                if (Input.GetKeyDown("n") && currentPlayer == PhotonNetwork.player)
                {
                    CallNextTurn();
                    mainCamera.GetComponent<PhotonView>().TransferOwnership(currentPlayer);
                    photonView.RPC("NextTeam", PhotonTargets.All);
                }
            }
            else if(Input.GetKeyDown("n"))
            {
                NextTeam();
            }
        }   
    }

    /// <summary>
    /// Sets the active team to the next in the list.
    /// <para>Call NextSoldier() in the active team to set the next soldier in that team </para>
    /// </summary>
    [PunRPC]
    public void NextTeam()
    {
        if(NWManager.instance.playingMultiplayer)
        {
            photonView.RPC("ToTopView", PhotonTargets.All);
        }
        else
        {
            ToTopView();
        } 
    }

    /// <summary>
    /// Call this to move the MainCamera to the current soldier of the current team.
    /// </summary>
    [PunRPC]
    public void ToSoldier()
    {
        if(NWManager.instance.playingMultiplayer)
        {
            if(currentPlayer == PhotonNetwork.player)
            {
                mainCamera.GetComponent<PhotonView>().TransferOwnership(currentPlayer);
            }
        }
        
        //pakt de positie waar de camera heen moet gaan
        int soldierIndex = allTeams[teamIndex].soldierIndex;
        Transform playerCamPos = allTeams[teamIndex].allSoldiers[soldierIndex].thirdPersonCamPos;
        mainCamera.transform.parent.SetParent(playerCamPos);
        StartCoroutine(MoveCam(playerCamPos.position, playerCamPos.rotation, CameraMovement.CameraStates.ThirdPerson));

         if(NWManager.instance.playingMultiplayer)
        {
            if (currentPlayer == PhotonNetwork.player)
            {
                allTeams[teamIndex].allSoldiers[soldierIndex].GetComponent<Movement>().canMove = true;
                allTeams[teamIndex].allSoldiers[soldierIndex].GetComponent<PhotonView>().RequestOwnership();

            }
        }
    }

    /// <summary>
    /// Call this to move the MainCamera to the TopView.
    /// </summary>
    [PunRPC]
    public void ToTopView()
    {
        mainCamera.transform.parent.SetParent(null);
       
       if(NWManager.instance.playingMultiplayer)
       {
           photonView.RPC("MoveCam", PhotonTargets.All, cameraPositionSky.position, CameraMovement.CameraStates.Topview);
       }
       else
       {
           StartCoroutine(MoveCam(cameraPositionSky.position,new Quaternion(0,0,0,0),CameraMovement.CameraStates.Topview));
       }
    }

    /// <summary>
    /// Moves the main camera to the overloaded position. Set camState to the state the camera should be in when it arrives.
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="camState"></param>
    /// <returns></returns>
    [PunRPC]
    public IEnumerator MoveCam(Vector3 moveTo,Quaternion rotateTo, CameraMovement.CameraStates camState)
    {
        if(!runningIenumerator)
        {
            runningIenumerator = true;
            mainCamera.cameraState = CameraMovement.CameraStates.Idle;
            while (mainCamera.transform.position != moveTo)
            {
                mainCamera.transform.parent.position = Vector3.MoveTowards(mainCamera.transform.parent.position, moveTo, movementSpeed * Time.deltaTime);
                mainCamera.transform.parent.rotation = Quaternion.Lerp(mainCamera.transform.parent.rotation, rotateTo, movementSpeed * Time.deltaTime);
                //mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, new Quaternion(cameraPositionSky.rotation.x, 0 ,0, 0), movementSpeed * Time.deltaTime);
                yield return null;
            }

            mainCamera.cameraState = camState;
            if(camState == CameraMovement.CameraStates.ThirdPerson)
            {
                Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
                soldier.soldierMovement.canMove = true;
                soldier.isActive = true;
            }

            if(camState == CameraMovement.CameraStates.Topview)
            {
                Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
                soldier.soldierMovement.canMove = false;
                soldier.isActive = false;

                if (teamIndex + 1 < allTeams.Count  )
                {
                    teamIndex += 1;
                }
                else
                {
                    teamIndex = 0;
                    allTeams[teamIndex].NextSoldier();
                }
                if(allTeams[teamIndex].teamAlive == false)
                {
                    NextTeam();
                }   
            }
            runningIenumerator = false;
        }

        yield return null;
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
