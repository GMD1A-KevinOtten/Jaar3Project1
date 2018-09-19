using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public static TeamManager instance;

    [Header("Camera proporties")]
    public CameraMovement mainCamera;
    public Transform cameraPositionSky;
    public float movementSpeed;

    [Header("Team Proporties")]
    public int teamIndex;
    public List<Team> allTeams = new List<Team>();

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

    void Update()
    {
        if(mainCamera.cameraState == CameraMovement.CameraStates.Topview)
        {
            if(Input.GetButtonDown("Enter"))
            {
                ToSoldier();
            }
        }
    }

    /// <summary>
    /// Sets the active team to the next in the list.
    /// <para>Call NextSoldier() in the active team to set the next soldier in that team </para>
    /// </summary>
    public void NextTeam()
    {
        ToTopView();
        if (teamIndex + 1 < allTeams.Count)
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

    /// <summary>
    /// Call this to move the MainCamera to the current soldier of the current team.
    /// </summary>
    public void ToSoldier()
    {
        int soldierIndex = allTeams[teamIndex].soldierIndex;
        Transform playerCamPos = allTeams[teamIndex].allSoldiers[soldierIndex].thirdPersonCamPos;
        mainCamera.transform.SetParent(playerCamPos);
        StartCoroutine(MoveCam(playerCamPos.position , CameraMovement.CameraStates.ThirdPerson));
    }

    /// <summary>
    /// Call this to move the MainCamera to the TopView.
    /// </summary>
    public void ToTopView()
    {
        mainCamera.transform.SetParent(null);
        MoveCam(cameraPositionSky.position,CameraMovement.CameraStates.Topview);
    }

    /// <summary>
    /// Moves the main camera to the overloaded position. Set camState to the state the camera should be in when it arrives.
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="camState"></param>
    /// <returns></returns>
    public IEnumerator MoveCam(Vector3 moveTo, CameraMovement.CameraStates camState)
    {
        mainCamera.cameraState = CameraMovement.CameraStates.Idle;
        while (mainCamera.transform.position != moveTo)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, moveTo, movementSpeed * Time.deltaTime);
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
        }

        yield return null;
    }
}
