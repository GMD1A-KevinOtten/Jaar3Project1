﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour {

    public static TeamManager instance;
    private bool runningIenumerator;

    [Header("Camera proporties")]
    public CameraMovement mainCamera;
    public Transform cameraPositionSky;
    public float movementSpeed;

    [Header("Team Proporties")]
    public int teamIndex;
    public int lastTeamIndex;
    public List<Team> allTeams = new List<Team>();

    [Header("Turn Properties")]
    public float maxTurnTime = 60;
    private float turnTime;
    private float tempFloat;
    public Text timeText;
    public Image turnTimerCircle;
    private bool countingDown;

    public Color circleStartColor;
    public Color circleEndColor;

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

        mainCamera = FindObjectOfType<CameraMovement>();
        Camera.main.transform.parent.position = cameraPositionSky.position;
        Camera.main.transform.parent.rotation = Quaternion.Euler(Camera.main.transform.parent.eulerAngles.x,cameraPositionSky.eulerAngles.y,cameraPositionSky.eulerAngles.z);
        Camera.main.transform.rotation = Quaternion.Euler(cameraPositionSky.transform.eulerAngles.x,mainCamera.transform.eulerAngles.y,mainCamera.transform.eulerAngles.z);
    }

    private void Start()
    {
        turnTime = maxTurnTime;
        timeText.text = "" + maxTurnTime;
        turnTimerCircle.fillAmount = 1;
        turnTimerCircle.color = circleStartColor;

        tempFloat = maxTurnTime;
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
        if(mainCamera.cameraState == CameraMovement.CameraStates.ThirdPerson)
        {
            if(Input.GetKeyDown("n"))
            {
                lastTeamIndex = teamIndex;
                NextTeam();
            }
        }
    }

    void InvokeRepeat()
    {
        if (!countingDown)
        {
            InvokeRepeating("TurnTimer", 1, 1);
        }
    }

    void ResetTimer()
    {
        turnTime = maxTurnTime;
        tempFloat = maxTurnTime;
        turnTimerCircle.fillAmount = 1;

        timeText.text = "" + maxTurnTime;
        turnTimerCircle.color = circleStartColor;
    }

    void ResetCountingDown()
    {
        countingDown = false;
    }

    void TurnTimer()
    {
        countingDown = true;
        if(turnTime > 0)
        {
            turnTime--;
        }
        if(turnTime <= 0)
        {
            CancelInvoke();
            turnTime = maxTurnTime;
            
            lastTeamIndex = teamIndex;
            NextTeam();
        }
        int seconds = Mathf.FloorToInt(turnTime);
    }

    void TurnTimerCircleHotseat()
    {
        turnTimerCircle.fillAmount = turnTime / maxTurnTime;
        float t = turnTime / maxTurnTime;
        Color color = Color.Lerp(circleEndColor, circleStartColor, t);
        turnTimerCircle.color = color;
        timeText.text = "" + turnTime;
    }

    /// <summary>
    /// Sets the active team to the next in the list.
    /// <para>Call NextSoldier() in the active team to set the next soldier in that team </para>
    /// </summary>
    public void NextTeam()
    {
        Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
        soldier.soldierMovement.canMove = false;
        soldier.isActive = false;

        if (teamIndex + 1 < allTeams.Count)
        {
            teamIndex += 1;
            if(allTeams[teamIndex].teamAlive == false)
            {
                NextTeam();
            }
            else
            {
                allTeams[lastTeamIndex].NextSoldier();
                if(allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex].isDead == true)
                {
                    allTeams[teamIndex].NextSoldier();
                }
            }
        }
        else
        {
            teamIndex = 0;
            if(allTeams[teamIndex].teamAlive == false)
            {
                NextTeam();
            }
            else
            {
                allTeams[lastTeamIndex].NextSoldier();
                if(allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex].isDead == true)
                {
                    allTeams[teamIndex].NextSoldier();
                }
            }
        }
        ToTopView();
        ResetCountingDown();
    }

    /// <summary>
    /// Call this to move the MainCamera to the current soldier of the current team.
    /// </summary>
    public void ToSoldier()
    {
        //pakt de positie waar de camera heen moet gaan
        int soldierIndex = allTeams[teamIndex].soldierIndex;
        Transform playerCamPos = allTeams[teamIndex].allSoldiers[soldierIndex].thirdPersonCamPos;
        mainCamera.transform.parent.SetParent(playerCamPos);
        StartCoroutine(MoveCam(playerCamPos.position, playerCamPos.rotation, CameraMovement.CameraStates.ThirdPerson));
    }

    /// <summary>
    /// Call this to move the MainCamera to the TopView.
    /// </summary>
    public void ToTopView()
    {
        mainCamera.transform.parent.SetParent(null);
        StartCoroutine(MoveCam(cameraPositionSky.position,cameraPositionSky.rotation,CameraMovement.CameraStates.Topview));
    }

    /// <summary>
    /// Moves the main camera to the overloaded position. Set camState to the state the camera should be in when it arrives.
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="camState"></param>
    /// <returns></returns>
    public IEnumerator MoveCam(Vector3 moveTo,Quaternion rotateTo, CameraMovement.CameraStates camState)
    {
        if(!runningIenumerator)
        {
            runningIenumerator = true;
            mainCamera.cameraState = CameraMovement.CameraStates.Idle;

            while (mainCamera.transform.parent.position != moveTo || mainCamera.transform.rotation != rotateTo)
            {
                mainCamera.transform.parent.position = Vector3.MoveTowards(mainCamera.transform.parent.position, moveTo, movementSpeed * 0.01f);
                mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, rotateTo, movementSpeed / 5 * 0.01f);

                if(camState == CameraMovement.CameraStates.ThirdPerson)
                {
                    Quaternion rot = Quaternion.Euler(mainCamera.transform.parent.eulerAngles.x,rotateTo.eulerAngles.y,rotateTo.eulerAngles.z);
                    mainCamera.transform.parent.rotation = Quaternion.Lerp(mainCamera.transform.parent.rotation, rot, movementSpeed / 3 * 0.01f);
                }
                if(camState == CameraMovement.CameraStates.Topview)
                {
                    mainCamera.transform.parent.rotation = Quaternion.Lerp(mainCamera.transform.parent.rotation, Quaternion.Euler(Camera.main.transform.parent.eulerAngles.x,cameraPositionSky.eulerAngles.y,cameraPositionSky.eulerAngles.z),movementSpeed / 3 * 0.01f);
                }
                yield return null;
            }

            if(camState == CameraMovement.CameraStates.ThirdPerson)
            {
                Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
                soldier.soldierMovement.canMove = true;
                soldier.isActive = true;
                mainCamera.xRotInput = mainCamera.baseXRotInput;
            }

            mainCamera.cameraState = camState;
            
            runningIenumerator = false;
        }
        yield return null;
    }
}
