using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public Image crosshairImage;
    public GameObject sniperScope;

    public UI_SoldierStatus SoldierStatusOnUI;
    public List<Image> weaponIcons = new List<Image>();

    [Header("Windows")]
    public RectTransform soldierStatusWindow;
    public RectTransform weaponIconWindow;

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

    [HideInInspector]
    public bool showCroshair = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Shows a window in the UI. Typically a panel is the best overload.
    /// </summary>
    /// <param name="windowToShow"></param>
    public void ShowWindow(RectTransform windowToShow)
    {
        windowToShow.gameObject.SetActive(true);
        activeWindows.Add(windowToShow);
    }

    /// <summary>
    /// Hides a window on the UI. Typically a panel is the best overload.
    /// </summary>
    /// <param name="windowToHide"></param>
    public void HideWindow(RectTransform windowToHide)
    {
        if (activeWindows.Contains(windowToHide))
        {
            windowToHide.gameObject.SetActive(false);
            activeWindows.Remove(windowToHide);
        }
        else
        {
            Debug.LogWarning("The window you are trying to hide is not currently active. Windowname: " + windowToHide.name);
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
    /// Lets the weapon Icons appear on the UI. Use this when changing the camera to Soldier View
    /// </summary>
    /// <param name="availableWeapons"></param>
    public void InstantiateWeaponIcons(List<GameObject> availableWeapons)
    {
        //Clears the existing weapon icons so that the new ones can be instantiated
        foreach(Image i in weaponIcons)
        {
            Destroy(i.gameObject);
        }

        weaponIcons.Clear();

        //New weapon Icons are instantiated
        for (int i = 0; i < availableWeapons.Count; i++)
        {
            GameObject newObject = Instantiate(weaponIconPrefab, weaponIconParent.transform.position, weaponIconPrefab.transform.rotation);
            newObject.transform.SetParent(weaponIconParent, false);
            Image weaponIcon = newObject.GetComponentInChildren<Image>();
            //weaponIcon.sprite = availableWeapons[i].icon;
            weaponIcons.Add(weaponIcon);
        }
    }

    public void InstantiateStatusButtons(List<Team> teams)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            int delegateIndex = 0;

            GameObject newObject = Instantiate(teamButtonPrefab, teamButtonsParent.position, teamButtonPrefab.transform.rotation);
            newObject.transform.SetParent(teamButtonsParent, false);

            Button b = newObject.GetComponent<Button>();

            delegateIndex = i;
            b.onClick.AddListener(delegate { ShowSoldierButtons(teams[delegateIndex].allSoldiers); }); 
        }
    }

    public void ShowSoldierButtons(List<Soldier> soldiersToShow)
    {
        if (soldiersInTeamButtons.Count > 0)
        {
            foreach(Button b in soldiersInTeamButtons)
            {
                Destroy(b.gameObject);
            }
        }

        for (int i = 0; i < soldiersToShow.Count; i++)
        {
            int delegateIndex = 0;

            GameObject newObject = Instantiate(soldierButtonPrefab, soldierButtonsParent.position, soldierButtonsParent.rotation);
            newObject.transform.SetParent(soldierButtonsParent, false);

            Button b = newObject.GetComponent<Button>();

            delegateIndex = i;
            b.onClick.AddListener(delegate { SetActiveSoldierStatus(soldiersToShow[delegateIndex]); });
            soldiersInTeamButtons.Add(b);
        }
    }

    public void SetActiveSoldierStatus(Soldier toShow)
    {
        SoldierStatusOnUI.UpdateStatus(toShow);
    }

    /// <summary>
    /// Updates the existing Soldier statuses in the UI.
    /// </summary>
    private void UpdateSoldierStatuses(Soldier updateTo)
    {
        SoldierStatusOnUI.UpdateStatus(updateTo);
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
}
