using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public Image crosshairImage;
    public GameObject sniperScope;

    public UI_SoldierStatus SoldierStatusOnUI;
    public List<Transform> weaponIcons = new List<Transform>();

    [Header("Windows")]
    public RectTransform soldierStatusWindow;
    public RectTransform weaponIconWindow;
    public UI_Message messageWindow;

    [Header("Prefabs")]
    public GameObject teamButtonPrefab;
    public GameObject soldierButtonPrefab;
    public GameObject weaponIconPrefab;

    [Header("Parents")]
    public RectTransform teamButtonsParent;
    public RectTransform soldierButtonsParent;
    public RectTransform weaponIconParent;

    private List<RectTransform> activeWindows = new List<RectTransform>();
    private List<Button> soldiersInTeamButtons = new List<Button>();
    public List<Button> teamButtons = new List<Button>();
    public List<UI_SoldierStatus> worldSpaceStatuses = new List<UI_SoldierStatus>();

    [HideInInspector]
    internal bool showCroshair = true;

    [Header("EndGame Menu")]
    public GameObject endGamePanel;
    public TextMeshProUGUI winnerText;

    public GameObject pausePanel;
    public bool settingsOpen;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingsOpen)
            {
                OpenSettings(true);
                settingsOpen = true;
            }
            else if (settingsOpen)
            {
                OpenSettings(false);
                settingsOpen = false;
            }
        }
    }

    /// <summary>
    /// Toggles a window in the UI. 
    /// </summary>
    /// <param name="windowToShow"></param>
    public void ToggleWindow(RectTransform windowToShow, bool toggle)
    {
        if (toggle)
        {
            windowToShow.gameObject.SetActive(true);
            activeWindows.Add(windowToShow);
        }
        else
        {
            windowToShow.gameObject.SetActive(false);
            activeWindows.Remove(windowToShow);
        }
    }

    /// <summary>
    /// Hides all windows currently active on the UI.
    /// <para>Use the e.g. for opening a menu</para>
    /// </summary>
    public void HideAllWindows()
    {
        if (activeWindows.Count > 0)
        {
            foreach (RectTransform window in activeWindows)
            {
                window.gameObject.SetActive(false);
            }

            activeWindows.Clear();
        }
        else
        {
            Debug.LogWarning("You are trying to hide all windows, but there are no windows active");
        }
    }

    /// <summary>
    /// Lets the weapon Icons appear on the UI. Use this when changing the camera to Soldier View, or when the available weapons change.
    /// </summary>
    /// <param name="availableWeapons"></param>
    public void InstantiateWeaponIcons(List<GameObject> availableWeapons)
    {
        //Clears the existing weapon icons so that the new ones can be instantiated
        foreach(Transform t in weaponIcons)
        {
            Destroy(t.gameObject);
        }

        weaponIcons.Clear();

        //New weapon Icons are instantiated
        for (int i = 0; i < availableWeapons.Count; i++)
        {
            GameObject newObject = Instantiate(weaponIconPrefab, weaponIconParent.transform.position, weaponIconPrefab.transform.rotation);
            newObject.transform.SetParent(weaponIconParent, false);
            Image weaponIcon = newObject.transform.GetChild(0).GetComponent<Image>();
            weaponIcon.sprite = availableWeapons[i].GetComponent<Weapon>().weaponSprite;
            weaponIcons.Add(newObject.transform);
        }
    }

    public void UpdateWeaponIcons(Weapon equippedWeapon)
    {
        for (int i = 0; i < weaponIcons.Count; i++)
        {
            if (weaponIcons[i].GetChild(0).GetComponent<Image>().sprite.name == equippedWeapon.weaponSprite.name)
            {
                weaponIcons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                weaponIcons[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
            }
        }
    }

    /// <summary>
    /// Shows a buttons for every team.
    /// </summary>
    /// <param name="teams"></param>
    public void InstantiateStatusButtons(List<Team> teams, int nextTeam)
    {
        if (teamButtons.Count > 0)
        {
            foreach(Button b in teamButtons)
            {
                Destroy(b.gameObject);
            }

            teamButtons.Clear();
        }


        for (int i = 0; i < teams.Count; i++)
        {
            int delegateIndex = 0;

            GameObject newObject = Instantiate(teamButtonPrefab, teamButtonsParent.position, teamButtonPrefab.transform.rotation);
            newObject.transform.SetParent(teamButtonsParent, false);

            Button b = newObject.GetComponent<Button>();
            Text text = b.transform.GetChild(0).GetComponent<Text>();
            text.text = "Team " + (i + 1);

            delegateIndex = i;

            if (i == nextTeam)
            {
                b.onClick.AddListener(delegate { ShowSoldierButtons(teams[delegateIndex].allSoldiers, teams[delegateIndex].thisTeamColor, teams[nextTeam].soldierIndex); });
            }
            else
            {
                b.onClick.AddListener(delegate { ShowSoldierButtons(teams[delegateIndex].allSoldiers, teams[delegateIndex].thisTeamColor, -1); });
            }

            b.onClick.AddListener(delegate { EffectsManager.instance.PlayButtonSound(); });

            teamButtons.Add(b);
        }

        UpdateTeamButtons(teamButtons, nextTeam);
    }


    /// <summary>
    /// Updates the team buttons. Shows the next team that will have a turn by making the button brighter.
    /// </summary>
    /// <param name="teamButtons"></param>
    /// <param name="teamIndex"></param>
    public void UpdateTeamButtons(List<Button> teamButtons, int teamIndex)
    {
        for (int i = 0; i < teamButtons.Count; i++)
        {
            if (i == teamIndex)
            {
                teamButtons[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                teamButtons[i].gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
            }
        }
    }

    /// <summary>
    /// Shows buttons for every soldier in the overload. 
    /// </summary>
    /// <param name="soldiersToShow"></param>
    /// <param name="teamColor"></param>
    public void ShowSoldierButtons(List<Soldier> soldiersToShow, Color teamColor, int soldierIndex)
    {
        if (soldiersInTeamButtons.Count > 0)
        {
            foreach(Button b in soldiersInTeamButtons)
            {
                Destroy(b.gameObject);
            }

            soldiersInTeamButtons.Clear();
        }

        for (int i = 0; i < soldiersToShow.Count; i++)
        {
            int delegateIndex = 0;

            GameObject newObject = Instantiate(soldierButtonPrefab, soldierButtonsParent.position, soldierButtonsParent.rotation);
            newObject.transform.SetParent(soldierButtonsParent, false);

            Button b = newObject.GetComponent<Button>();
            Text text = b.transform.GetChild(0).GetComponent<Text>();
            text.text = soldiersToShow[i].soldierName;

            delegateIndex = i;
            b.onClick.AddListener(delegate { SetActiveSoldierStatus(soldiersToShow[delegateIndex], teamColor); });
            b.onClick.AddListener(delegate { EffectsManager.instance.PlayButtonSound(); });
            soldiersInTeamButtons.Add(b);
        }

        UpdateSoldierButtons(soldiersInTeamButtons, soldierIndex);
    }

    public void DeleteSoldierButtons()
    {
        if (soldiersInTeamButtons.Count > 0)
        {
            foreach (Button b in soldiersInTeamButtons)
            {
                Destroy(b.gameObject);
            }

            soldiersInTeamButtons.Clear();
        }
    }

    /// <summary>
    /// Updates the soldier buttons. Shows the next Soldier that will have a turn by making the button brighter.
    /// </summary>
    /// <param name="soldierButtons"></param>
    /// <param name="nextSoldierIndex"></param>
    public void UpdateSoldierButtons(List<Button> soldierButtons, int nextSoldierIndex)
    {
        if (nextSoldierIndex >= 0)
        {
            for (int i = 0; i < soldierButtons.Count; i++)
            {
                if (i == nextSoldierIndex)
                {
                    soldierButtons[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    soldierButtons[i].gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                }
            }
        }
        else
        {
            foreach(Button soldierButton in soldierButtons)
            {
                soldierButton.gameObject.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
            }
        }

    }
    
    /// <summary>
    /// Updates the Soldier Icon in the solder status window
    /// </summary>
    /// <param name="toShow"></param>
    /// <param name="teamColor"></param>
    public void SetActiveSoldierStatus(Soldier toShow, Color teamColor)
    {
        SoldierStatusOnUI.UpdateStatus(toShow, teamColor);
    }

    /// <summary>
    /// Updates all the soldier statuses in the world space canvases on each soldier. This function will toggle the minimalism function on the status.
    /// </summary>
    /// <param name="teams"></param>
    public void UpdateWorldSpaceStatuses(List<Team> teams)
    {
        foreach(Team t in teams)
        {
            foreach(Soldier s in t.allSoldiers)
            {
                UI_SoldierStatus status = s.GetComponentInChildren<UI_SoldierStatus>(true);

                if (!status.minimal)
                {
                    status.ToggleMinimalism(true);
                }

                status.UpdateStatus(s, t.thisTeamColor);

                if (!worldSpaceStatuses.Contains(status))
                {
                    worldSpaceStatuses.Add(status);
                }
            }
        }
    }

    /// <summary>
    /// Toggles the world space statuses of every soldier.
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleWorldSpaceStatuses(bool toggle)
    {
        foreach(UI_SoldierStatus status in worldSpaceStatuses)
        {
            status.gameObject.SetActive(toggle);
        }
    }

    /// <summary>
    /// Shows the crosshair of the active player on screen. 
    /// </summary>
    /// <param name="toShow"></param>
    /// <param name="position"></param>
    public void ShowCrosshairOnScreen(Sprite toShow, Vector3 position)
    {
        if(showCroshair)
        {   
            crosshairImage.sprite = toShow;
            crosshairImage.enabled = true;
            crosshairImage.transform.position = Camera.main.WorldToScreenPoint(position);
        }
    }

    /// <summary>
    /// Hides the crosshair that was shown by ShowCrosshairOnScreen().
    /// <para>Use this e.g. for going to Top View</para>
    /// </summary>
    public void HideCrosshair()
    {
        crosshairImage.enabled = false;
    }

    /// <summary>
    /// Call this function to show a message on the UI. Useful for tips or quick tutorials.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="duration"></param>
    public void ShowMessageOnUI(string message, float duration)
    {
        messageWindow.ShowMessage(message, duration);
    }

    public void ToggleScope()
    {
        if(sniperScope.activeSelf == true)
        {
            sniperScope.SetActive(false);
        }
        else
        {
            sniperScope.SetActive(true);
        }
    }

    //Menu
    public void LoadScene(int sceneIndex)
    {
        GameManager.Instance.ChangeScene(sceneIndex);
    }

    public void GameOver(int victoriousTeam)
    {
        endGamePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winnerText.text = "Team " + victoriousTeam + " won!";
        Time.timeScale = 0;

        EffectsManager.instance.PlayAudio2D(EffectsManager.instance.FindAudioClip("Victory Music"));
    }

    public void OpenSettings(bool open)
    {
        if (open)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            if(Camera.main.GetComponent<CameraMovement>().cameraState != CameraMovement.CameraStates.Topview)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
         
        }
       
    }

    public void QuitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ToggleMouse()
    {
        if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
