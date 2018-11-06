using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoldierStatus : MonoBehaviour {

    public Image soldierIcon;
    public Image healthBar;
    public Image deathMarker;
    public Text currentClipSize;
    public Image weaponImage;
    public bool alive = true;
    internal bool minimal { get; private set; }

    public void UpdateStatus(Soldier mySoldier, Color teamColor)
    {
        if (!soldierIcon.gameObject.activeInHierarchy)
            soldierIcon.gameObject.SetActive(true);

        if (mySoldier.health <= 0 || mySoldier.isDead)
        {
            deathMarker.gameObject.SetActive(true);
            alive = false;
        }
        else
        {
            deathMarker.gameObject.SetActive(false);
            alive = true;
        }

        soldierIcon.color = teamColor;
        currentClipSize.text = mySoldier.equippedWeapon.currentClip.ToString();
        weaponImage.sprite = mySoldier.equippedWeapon.weaponSprite;

        float percent = (float)mySoldier.health / mySoldier.maxHealth;
        print(percent.ToString());
        healthBar.fillAmount = percent;
    }

    public void ToggleMinimalism(bool toggle)
    {
        if (toggle)
        {
            currentClipSize.transform.parent.gameObject.SetActive(false);
            weaponImage.gameObject.SetActive(false);
            minimal = true;
        }
        else
        {
            currentClipSize.transform.parent.gameObject.SetActive(true);
            weaponImage.gameObject.SetActive(true);
            minimal = false;
        }

    }
}
