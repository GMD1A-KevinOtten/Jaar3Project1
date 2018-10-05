using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Image crosshairImage;
    public GameObject sniperScope;

    public List<UI_SoldierStatus> ui_soldierStatuses = new List<UI_SoldierStatus>();
    public List<Image> weaponIcons = new List<Image>();

    [Header("Windows")]
    public RectTransform soldierStatusWindow;
    public RectTransform weaponIconWindow;

    [Header("Prefabs")]
    public GameObject soldierStatusPrefab;
    public GameObject weaponIconPrefab;

    [Header("Parents")]
    public RectTransform soldierStatusParent;
    public RectTransform weaponIconParent;

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

    public List<UI_SoldierStatus> InstantiateSoldierStatus(List<Team> teams)
    {
        List<UI_SoldierStatus> toReturn = new List<UI_SoldierStatus>();

        foreach(Team t in teams)
        {
            for (int i = 0; i < t.allSoldiers.Count; i++)
            {
                GameObject newObject = Instantiate(soldierStatusPrefab, soldierStatusParent.transform.position, soldierStatusPrefab.transform.rotation);
                newObject.transform.SetParent(soldierStatusParent, false);

                toReturn.Add(newObject.GetComponent<UI_SoldierStatus>());
            }
        }

        return toReturn;
    }

    public void InstantiateWeaponIcons(List<GameObject> availableWeapons)
    {
        foreach(Image i in weaponIcons)
        {
            Destroy(i);
        }

        weaponIcons.Clear();

        for (int i = 0; i < availableWeapons.Count; i++)
        {
            GameObject newObject = Instantiate(weaponIconPrefab, weaponIconParent.transform.position, weaponIconPrefab.transform.rotation);
            newObject.transform.SetParent(weaponIconParent, false);
            Image weaponIcon = newObject.GetComponentInChildren<Image>();
            //weaponIcon.sprite = availableWeapons[i].icon;
            weaponIcons.Add(weaponIcon);
        }
    }

    public void UpdateSoldierStatuses()
    {
        foreach(UI_SoldierStatus status in ui_soldierStatuses)
        {
            status.UpdateStatus();
        }
    }

    public void ShowCrosshairOnScreen(Sprite toShow, Vector3 position)
    {
        if(showCroshair)
        {   
            crosshairImage.sprite = toShow;
            crosshairImage.enabled = true;
            crosshairImage.transform.position = Camera.main.WorldToScreenPoint(position);
        }
    }

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
