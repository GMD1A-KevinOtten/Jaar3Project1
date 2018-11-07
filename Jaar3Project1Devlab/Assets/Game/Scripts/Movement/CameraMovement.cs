using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public enum CameraStates
    {
        ThirdPerson,
        Topview,
        CombatVieuw,
        Idle,
    }

	public float camMovSpeed;
	public float camRotSpeed;
    public bool cantMoveCamera;

    public CameraStates cameraState;
	public float clampValue;
	public float vertRotSpeed;
    public Weapon gunToRotate;

    [HideInInspector]
    public float xRotInput;
    public float baseXRotInput = 30;

    void Start() 
    {
        xRotInput = baseXRotInput;
    }
	void FixedUpdate () 
	{
        if(!cantMoveCamera)
        {
            if(cameraState == CameraStates.Topview)
            {
                TopViewCamera();
            }
            else if(cameraState == CameraStates.ThirdPerson || cameraState == CameraStates.CombatVieuw)
            {
                if(transform.root.GetComponent<Soldier>() != null)
                {
                    if (gunToRotate != transform.root.GetComponent<Soldier>().equippedWeapon)
                    {
                        gunToRotate = transform.root.GetComponent<Soldier>().equippedWeapon;
                    }
                }
              
                SoldierCamera();
            }
        }
	}

    /// <summary>
    /// Call this function when the camera is hovering above the map.
    /// </summary>
	public void TopViewCamera()
	{
        float horMovement = Input.GetAxis("Horizontal") * Time.deltaTime * camMovSpeed;
        float vertMovement = Input.GetAxis("Vertical") * Time.deltaTime * camMovSpeed;

        Vector3 toMove = new Vector3(horMovement, 0, vertMovement);
        transform.parent.Translate(toMove, Space.Self);

        if (Input.GetButton("Q"))
        {
            transform.parent.Rotate(0, -camRotSpeed * Time.deltaTime, 0, Space.World);
        }

        if (Input.GetButton("E"))
        {
            transform.parent.Rotate(0, camRotSpeed * Time.deltaTime, 0, Space.World);
        }
	}

    /// <summary>
    /// Call this function when the camera is in third person mode
    /// </summary>
	public void SoldierCamera()
	{
        xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
        xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
        transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
        if(cameraState == CameraStates.CombatVieuw)
        {
            if(gunToRotate != null)
            {
                if (!gunToRotate.isTank)
                {
                    gunToRotate.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
                }
            }
        }
    }
}
