using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon {

    [Header("Sniper Specific")]

    public Camera cameraScope;

    public override void Start() 
    {
        base.Start();
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
                    ToggelScope();
                }
            }
        }
    }

    

    public void ToggelScope()
    {
        UIManager.instance.ToggleScope();
        if(cameraScope.GetComponent<Camera>().depth < Camera.main.depth)
        {
            cameraScope.GetComponent<Camera>().depth = 1;
            UIManager.instance.showCroshair = false;
            UIManager.instance.HideCrosshair();
        }
        else
        {
            cameraScope.GetComponent<Camera>().depth = -1;
            UIManager.instance.showCroshair = true;
        }
        
        //Scope UI element
        //Play sound
    }
}
