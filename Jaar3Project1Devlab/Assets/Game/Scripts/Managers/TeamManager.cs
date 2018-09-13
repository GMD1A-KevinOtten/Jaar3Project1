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

    /// <summary>
    /// Moves the main camera to the overloaded position. Set camState to the state the camera should be in when it arrives.
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="camState"></param>
    /// <returns></returns>
    public IEnumerator MoveCamTest(Vector3 moveTo, CameraMovement.CameraStates camState)
    {
        mainCamera.cameraState = CameraMovement.CameraStates.Idle;
        while (mainCamera.transform.position != moveTo)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, moveTo, movementSpeed * Time.deltaTime);
            yield return null;
        }

        mainCamera.cameraState = camState;

        yield return null;
    }
}
