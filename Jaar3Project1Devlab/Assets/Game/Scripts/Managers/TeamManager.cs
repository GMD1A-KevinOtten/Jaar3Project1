using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeamManager : MonoBehaviour {

    public static TeamManager instance;
    private bool runningIenumerator;

    [Header("Camera Properties")]
    public CameraMovement mainCamera;
    public Transform cameraPositionSky;
    public float movementSpeed;

    [Header("Team Properties")]
    public bool gameOver;
    public int teamCount;
    public int teamIndex;
    public int lastTeamIndex;
    public List<Team> allTeams = new List<Team>();

    [Header("Turn Properties")]
    public float maxTurnTime = 60;
    [HideInInspector]
    public float turnTime;
    public float combatTurnTime;
    public bool combatTimer;
    public TextMeshProUGUI timeText;
    public Image turnTimerCircle;

    public Color circleStartColor;
    public Color circleEndColor;

    public TextMeshProUGUI betweenTurnsText;
    public float textDisplayTime = 1.5F;
    private bool textSet;
    private bool inTopView;

    public Soldier activeSoldier;
    public delegate void EndTurn();
    public EndTurn endTurn;


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


        UIManager.instance.InstantiateStatusButtons(allTeams, teamIndex);
        UIManager.instance.ShowMessageOnUI("Press TAB to hide the status panel", 5);
        UIManager.instance.ShowMessageOnUI("Press Enter to start the battle!", 5);

        StartCoroutine(UIUpdateDelay());

        UIManager.instance.ToggleMouse();
    }

    private IEnumerator UIUpdateDelay()
    {
        yield return new WaitForSeconds(0.1f);
        UIManager.instance.UpdateWorldSpaceStatuses(allTeams);

        if (inTopView)
        {
            UIManager.instance.HideCrosshair();

            UIManager.instance.ToggleWindow(UIManager.instance.weaponIconWindow, false);
            UIManager.instance.ToggleWindow(UIManager.instance.soldierStatusWindow, true);
            UIManager.instance.ToggleMouse();
        }

    }

    void Update()
    {
        if(!gameOver)
        {
            if(mainCamera.cameraState == CameraMovement.CameraStates.Topview)
            {

                if(turnTime < maxTurnTime)
                {
                    ResetTimer();
                }
                if(Cursor.lockState == CursorLockMode.Locked || Cursor.visible == false)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (!textSet)
                {
                    StartCoroutine(SetBetweenTurnsText());
                }

                if (Input.GetButtonDown("Enter"))
                {
                    ToSoldier();
                }

                if (Input.GetButtonDown("Tab"))
                {
                    if (UIManager.instance.soldierStatusWindow.gameObject.activeInHierarchy)
                    {
                        UIManager.instance.ToggleWindow(UIManager.instance.soldierStatusWindow, false);
                    }
                    else
                    {
                        UIManager.instance.ToggleWindow(UIManager.instance.soldierStatusWindow, true);
                    }
                }
            }
            if(mainCamera.cameraState == CameraMovement.CameraStates.ThirdPerson || mainCamera.cameraState == CameraMovement.CameraStates.CombatVieuw)
            {
                if(Input.GetButtonDown("Enter") && activeSoldier.canSwitch == true)
                {
                    EndTheTurn();
                }
                if(combatTimer)
                {
                    CombatTimer();
                }
                else
                {
                    TurnTimer();
                }
            }
            else if(mainCamera.cameraState == CameraMovement.CameraStates.Idle)
            {
                if (combatTimer)
                {
                    CombatTimer();
                }
            }
            if(activeSoldier == null && mainCamera.cameraState == CameraMovement.CameraStates.CombatVieuw)
            {
                ToTopView(true);
            }
        }
    }

    public void EndTheTurn()
    {
        if (combatTimer)
        {
            activeSoldier.CombatToggle();
        }
        lastTeamIndex = teamIndex;
        if(!gameOver)
        {
        NextTeam();
        }
        if (endTurn != null)
        {
            endTurn();
        }
    }

    public void GameOverCheck()
    {
        int teamsAlive = 0;
        Team livingTeam = null;
        foreach (Team team in allTeams)
        {
            if(team.teamAlive == true)
            {
                teamsAlive += 1;
                livingTeam = team;
            }
        }
        if(teamsAlive < 2)
        {
            print("infinity test 2");
            gameOver = true;
            StartCoroutine(GameOverWait(teamsAlive, livingTeam));
        }
    }

    public IEnumerator GameOverWait(int teamsAlive ,Team livingTeam)
	{
		yield return new WaitForSeconds(3);
        EndTheTurn();
        if(teamsAlive == 0)
        {
            mainCamera.cantMoveCamera = true;
            GameManager.Instance.GameOverEvent(0);
        }
        else if(teamsAlive == 1)
        {
            mainCamera.cantMoveCamera = true;
            GameManager.Instance.GameOverEvent(livingTeam.allSoldiers[0].myTeam += 1);

        }
	}

    IEnumerator SetBetweenTurnsText()
    {
        textSet = true;

        int tindx = teamIndex + 1;
        int sindx = allTeams[teamIndex].soldierIndex + 1;

        betweenTurnsText.text = "Player: " + tindx + "\n" + "Soldier: " + sindx;
        betweenTurnsText.color = allTeams[teamIndex].thisTeamColor;

        yield return new WaitForSeconds(textDisplayTime);
        betweenTurnsText.text = "";
        

    }

    public void CombatTimer()
    {
        if(turnTime > 0)
        {
            turnTime -= Time.deltaTime;
        }
        if(turnTime <= 0)
        {
            lastTeamIndex = teamIndex;
            activeSoldier.CombatToggle();
            NextTeam();
            if(endTurn != null)
            {
                endTurn();
            }
        }

        turnTimerCircle.fillAmount = turnTime / combatTurnTime;
        float t = turnTime / combatTurnTime;
        Color color = Color.Lerp(circleEndColor, circleStartColor, t);
        turnTimerCircle.color = color;
        timeText.text = "" + turnTime.ToString("F0");
    }

    void ResetTimer()
    {
        turnTime = maxTurnTime;
        turnTimerCircle.fillAmount = 1;

        timeText.text = "" + maxTurnTime;
        turnTimerCircle.color = circleStartColor;
    }

    void TurnTimer()
    {
        if(turnTime > 0)
        {
            turnTime -= Time.deltaTime;
        }
        if(turnTime <= 0)
        {
            lastTeamIndex = teamIndex;
            if(endTurn != null)
            {
                endTurn();
            }
            NextTeam();
        }

        turnTimerCircle.fillAmount = turnTime / maxTurnTime;
        float t = turnTime / maxTurnTime;
        Color color = Color.Lerp(circleEndColor, circleStartColor, t);
        turnTimerCircle.color = color;
        timeText.text = "" + turnTime.ToString("F0");
    }

    /// <summary>
    /// Sets the active team to the next in the list.
    /// <para>Call NextSoldier() in the active team to set the next soldier in that team </para>
    /// </summary>
    public void NextTeam()
    {
        if(!gameOver)
        {
            ResetTimer();

            if(activeSoldier != null)
            {
                activeSoldier.DisableMovementAnimation();
            }

            Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
            soldier.soldierMovement.canMoveAndRotate = false;
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
            ToTopView(true);
        }
    }

    /// <summary>
    /// Call this to move the MainCamera to the current soldier of the current team.
    /// </summary>
    public void ToSoldier()
    {
        UIManager.instance.ToggleMouse();
        betweenTurnsText.text = "";
        textSet = false;

        //pakt de positie waar de camera heen moet gaan
        int soldierIndex = allTeams[teamIndex].soldierIndex;
        Transform playerCamPos = allTeams[teamIndex].allSoldiers[soldierIndex].thirdPersonCamPos;
        Soldier soldier = allTeams[teamIndex].allSoldiers[soldierIndex].GetComponent<Soldier>();
        mainCamera.transform.parent.SetParent(playerCamPos);

        UIManager.instance.ToggleWindow(UIManager.instance.soldierStatusWindow, false);
       // UIManager.instance.ToggleWorldSpaceStatuses(false);
        UIManager.instance.InstantiateWeaponIcons(allTeams[teamIndex].allSoldiers[soldierIndex].availableWeapons);
        UIManager.instance.ToggleWindow(UIManager.instance.weaponIconWindow, true);
        UIManager.instance.UpdateWeaponIcons(soldier.equippedWeapon);
        

        StartCoroutine(MoveCam(playerCamPos.position, playerCamPos.rotation, CameraMovement.CameraStates.ThirdPerson));
    }

    /// <summary>
    /// Call this to move the MainCamera to the TopView.
    /// </summary>
    public void ToTopView(bool toggleCursor)
    {
        UIManager.instance.HideCrosshair();
        UIManager.instance.UpdateWorldSpaceStatuses(allTeams);
        //UIManager.instance.ToggleWorldSpaceStatuses(true);
        UIManager.instance.ToggleWindow(UIManager.instance.weaponIconWindow, false);
        UIManager.instance.ToggleWindow(UIManager.instance.soldierStatusWindow, true);
        UIManager.instance.DeleteSoldierButtons();
        UIManager.instance.InstantiateStatusButtons(allTeams, teamIndex);

        if (activeSoldier != null)
        {
            mainCamera.transform.parent.SetParent(null);
            activeSoldier.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            activeSoldier = null;
        }

        StartCoroutine(MoveCam(cameraPositionSky.position,cameraPositionSky.rotation,CameraMovement.CameraStates.Topview));
        if (toggleCursor)
        {
            UIManager.instance.ToggleMouse();
        }

    }

    public void ToCombatVieuw()
    {
        int soldierIndex = allTeams[teamIndex].soldierIndex;
        Transform combatCamPos = allTeams[teamIndex].allSoldiers[soldierIndex].combatCameraPosition;
        mainCamera.transform.parent.SetParent(combatCamPos);
        mainCamera.transform.parent.localPosition = Vector3.zero;
        StartCoroutine(MoveCam(combatCamPos.position, combatCamPos.rotation, CameraMovement.CameraStates.CombatVieuw));
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
            Quaternion cameraRot = Quaternion.Euler(mainCamera.transform.eulerAngles.x,mainCamera.transform.eulerAngles.y,mainCamera.transform.eulerAngles.z);
            Quaternion cameraToRot = Quaternion.Euler(mainCamera.transform.eulerAngles.x,mainCamera.transform.eulerAngles.y,mainCamera.transform.eulerAngles.z);

            while (mainCamera.transform.parent.position != moveTo || cameraRot != cameraToRot)
            {
                mainCamera.transform.parent.position = Vector3.MoveTowards(mainCamera.transform.parent.position, moveTo, movementSpeed * Time.deltaTime);
                mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, rotateTo, movementSpeed / 10 * Time.deltaTime);
                if(camState == CameraMovement.CameraStates.ThirdPerson)
                {
                    Quaternion rot = Quaternion.Euler(mainCamera.transform.parent.eulerAngles.x,rotateTo.eulerAngles.y,rotateTo.eulerAngles.z);
                    mainCamera.transform.parent.rotation = Quaternion.Lerp(mainCamera.transform.parent.rotation, rot, movementSpeed / 10 * Time.deltaTime);
                }
                if(camState == CameraMovement.CameraStates.Topview)
                {
                    mainCamera.transform.parent.rotation = Quaternion.Lerp(mainCamera.transform.parent.rotation, Quaternion.Euler(Camera.main.transform.parent.eulerAngles.x,cameraPositionSky.eulerAngles.y,cameraPositionSky.eulerAngles.z),movementSpeed / 10 * Time.deltaTime);
                }
                yield return null;
            }

            if(camState == CameraMovement.CameraStates.ThirdPerson)
            {
                Soldier soldier = allTeams[teamIndex].allSoldiers[allTeams[teamIndex].soldierIndex];
                soldier.soldierMovement.canMoveAndRotate = true;
                soldier.isActive = true;
                activeSoldier = soldier;
                activeSoldier.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                mainCamera.xRotInput = mainCamera.baseXRotInput;
                if (activeSoldier.damageTurns != 0)
                {
                    activeSoldier.TakeDamageOverTime();
                }
            }
            
            mainCamera.cameraState = camState;
            
            runningIenumerator = false;
        }
        yield return null;
    }
}
