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
        switch (cameraState)
        {
            case CameraStates.Topview:
                TopViewCamera();
                break;
            case CameraStates.ThirdPerson:
                if (gunToRotate != transform.root.GetComponent<Soldier>().equippedWeapon)
                {
                    gunToRotate = transform.root.GetComponent<Soldier>().equippedWeapon;
                }
                SoldierCamera();
                break;
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
        // if(gunToRotate != null)
        // {
        //     if (!gunToRotate.isTank)
        //     {
        //         gunToRotate.transform.localRotation = Quaternion.Euler(0, -xRotInput, 0);
        //     }
        // }
    }
}
