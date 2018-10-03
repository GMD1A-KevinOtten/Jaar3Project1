using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Image crosshairImage;
    public GameObject sniperScope;

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
