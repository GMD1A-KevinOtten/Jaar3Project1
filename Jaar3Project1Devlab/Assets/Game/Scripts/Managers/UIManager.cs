using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Image crosshairImage;
    public Image sniperScope;

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
        crosshairImage.sprite = toShow;
        crosshairImage.enabled = true;
        crosshairImage.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    public void HideCrosshair()
    {
        crosshairImage.enabled = false;
    }

    public void ToggleScope()
    {
        if(sniperScope.enabled)
        {
            sniperScope.enabled = false;
        }
        else
        {
            sniperScope.enabled = true;
        }
    }
}
