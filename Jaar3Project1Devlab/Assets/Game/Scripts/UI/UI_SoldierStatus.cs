using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoldierStatus : MonoBehaviour {

    public Image soldierIcon;
    public Slider healthBar;
    public Image deathMarker;
    public bool alive = true;

    public void UpdateStatus(Soldier mySoldier)
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

        float percent = (float)mySoldier.health / mySoldier.maxHealth;
        healthBar.value = percent;
    }
}
