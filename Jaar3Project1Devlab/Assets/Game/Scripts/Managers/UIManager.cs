using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Image crosshair;
    

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

        HideCrosshair();
    }

    public void ShowCrosshairOnScreen(Sprite toShow, Vector3 position)
    {
        crosshair.sprite = toShow;
        crosshair.enabled = true;
        crosshair.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    public void HideCrosshair()
    {
        crosshair.enabled = false;
    }
}
