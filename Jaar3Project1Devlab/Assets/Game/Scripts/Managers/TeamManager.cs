using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public static TeamManager instance;

    [Header("Camera proporties")]
    public CameraMovement mainCamera;
    public float movementSpeed;

    [Header("Team Proporties")]
    public int teamIndex;
    public List<Team> allTeams = new List<Team>();

    private bool moveCam;
    private Vector3 tempPosition;

    private void Update()
    {
        if (moveCam)
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, tempPosition, movementSpeed);
    }

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
    }

    /// <summary>
    /// Sets the active team to the next in the list.
    /// <para>Call NextSoldier() in the active team to set the next soldier in that team </para>
    /// </summary>
    public void NextTeam()
    {
        if (teamIndex + 1 < allTeams.Count)
        {
            teamIndex += 1;
        }
        else
        {
            teamIndex = 0;
        }
    }

    public void MoveCameraToPosition(Vector3 moveTo)
    {
        moveCam = true;
        tempPosition = moveTo;
    }
}
