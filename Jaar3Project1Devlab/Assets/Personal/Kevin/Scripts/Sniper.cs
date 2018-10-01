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

	public override void  Update() 
    {
        base.Update();
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(gameObject.transform.root.GetComponent<Soldier>().isActive == true)
            {
                if(Input.GetButtonDown("Fire2"))
                {
                    ActivateScope();
                }
            }
        }
    }

    

    public void ActivateScope()
    {
        //Camera.main.GetComponent<Camera>().fieldOfView = fovValue; Iedere keer als ik ALT+TAB deet hij dit pl0x fix -Peter
        //Scope UI element
        //Play sound
    }
}
