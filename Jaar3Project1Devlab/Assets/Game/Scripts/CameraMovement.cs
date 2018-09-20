using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public enum CameraStates
    {
        ThirdPerson,
        Topview,
        Idle,
    }

	public float camMovSpeed;
	public float camRotSpeed;

    public CameraStates cameraState;
	public float clampValue;
	public float vertRotSpeed;
    public Weapon gunToRotate;

    private float xRotInput;

    void Start()
	{
		xRotInput = 20;
	}

	void FixedUpdate () 
	{
        switch (cameraState)
        {
            case CameraStates.Topview:
                TopViewCamera();
                break;
            case CameraStates.ThirdPerson:
                if(transform.root != null)
                    {
                        if(gunToRotate != null)
                        {
                            if (gunToRotate != transform.root.GetComponent<Soldier>().equippedWeapon)
                            {
                                gunToRotate = transform.root.GetComponent<Soldier>().equippedWeapon;
                            }
                        }
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
        if(NWManager.instance != null)
        {
            if (NWManager.instance.playingMultiplayer) //Are we playing multiplayer?
            {
                if(TeamManager.instance.currentPlayer == PhotonNetwork.player)
                {
                    float hornetMovement = Input.GetAxis("Horizontal") * Time.deltaTime * camMovSpeed;
                    float vertnetMovement = Input.GetAxis("Vertical") * Time.deltaTime * camMovSpeed;

                    Vector3 tonetMove = new Vector3(hornetMovement, 0, vertnetMovement);
                    transform.Translate(tonetMove, Space.World);

                    if (Input.GetButton("Q"))
                    {
                        transform.Rotate(0, -camRotSpeed * Time.deltaTime, 0, Space.World);
                    }

                    if (Input.GetButton("E"))
                    {
                        transform.Rotate(0, camRotSpeed * Time.deltaTime, 0, Space.World);
                    }
                }
            }
        }
        else //Not playing multiplayer
        {
            float horMovement = Input.GetAxis("Horizontal") * Time.deltaTime * camMovSpeed;
            float vertMovement = Input.GetAxis("Vertical") * Time.deltaTime * camMovSpeed;

            Vector3 toMove = new Vector3(horMovement, 0, vertMovement);
            transform.Translate(toMove, Space.World);

            if (Input.GetButton("Q"))
            {
                transform.Rotate(0, -camRotSpeed * Time.deltaTime, 0, Space.World);
            }

            if (Input.GetButton("E"))
            {
                transform.Rotate(0, camRotSpeed * Time.deltaTime, 0, Space.World);
            }
        }
		
	}

    /// <summary>
    /// Call this function when the camera is in third person mode
    /// </summary>
	public void SoldierCamera()
	{
        if(NWManager.instance != null)
        {
            if (NWManager.instance.playingMultiplayer)
            {
                if(TeamManager.instance.currentPlayer == PhotonNetwork.player)
                {
                    xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
                    xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
                    transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
                    gunToRotate.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
                }
            }
        }
        else
        {
            xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
            xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
            transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
            if(gunToRotate != null)
            {
                gunToRotate.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
            }
        }
		
    }
}
