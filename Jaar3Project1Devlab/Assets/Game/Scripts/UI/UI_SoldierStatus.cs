using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoldierStatus : MonoBehaviour {

    private Soldier soldier;

    public Slider healthBar;
    public Image deathMarker;
    public bool alive;

    private void Awake()
    {
        soldier.transform.root.GetComponent<Soldier>();
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        if (soldier.health <= 0 || soldier.isDead)
        {
            deathMarker.gameObject.SetActive(true);
            alive = false;
        }

        float percent = soldier.health / 100;
        healthBar.value = percent;
    }
}
