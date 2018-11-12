using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeedback : MonoBehaviour {

    public Image healthBar;
    public Text currentClip;
    public Text totalClip;


    public void UpdateAmmo(int current, int total)
    {
        currentClip.text = current.ToString();
        totalClip.text = total.ToString();
    }

    public void UpdateHealth(float health, float totalHealth)
    {
        float percent = health / totalHealth;
        healthBar.fillAmount = percent;
    }

    public void ToggleFeedback(bool toggle)
    {
        if (toggle)
        {
            healthBar.transform.parent.gameObject.SetActive(true);
            currentClip.gameObject.SetActive(true);
        }
        else
        {
            healthBar.transform.parent.gameObject.SetActive(false);
            currentClip.gameObject.SetActive(false);
        }
    }
}
