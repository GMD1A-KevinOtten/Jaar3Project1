using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon {

    [Header("Sniper Specific")]

    private float baseFovValue;
    public float fovValue;

    void Start() 
    {
        baseFovValue = Camera.main.GetComponent<Camera>().fieldOfView;
    }

	void Update() 
    {
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(currentClip == 0)
            {
                StartCoroutine(waitTillTrigger());
            }
            if(gameObject.transform.root.GetComponent<Soldier>().isActive == true)
            {
                if(Input.GetButtonDown("Fire2"))
                {
                    ActivateScope();
                }
            }
        }
    }

    public IEnumerator waitTillTrigger()
    {
        yield return new WaitForSeconds(3);
        TeamManager.instance.NextTeam();
    }

    public void ActivateScope()
    {
        Camera.main.GetComponent<Camera>().fieldOfView = fovValue; 
        //Scope UI element
        //Play sound
    }
}
