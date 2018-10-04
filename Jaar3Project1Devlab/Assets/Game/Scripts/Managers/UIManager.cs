using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Image crosshairImage;
    public GameObject sniperScope;
    public UI_SoldierStatus[] ui_soldierStatuses;

    [Header("Prefabs")]
    public GameObject soldierStatusPrefab;

    [Header("Parents")]
    public RectTransform soldierStatusParent;

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

    public UI_SoldierStatus[] InstantiateSoldierStatus(List<Team> teams)
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

        return toReturn.ToArray();
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
