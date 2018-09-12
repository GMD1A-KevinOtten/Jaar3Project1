using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.PunBehaviour {
	
	public float moveSpeed;
	public float rotSpeed;

	private float xRotInput;
	public float clampValue;

	public bool canMove;

	void Start()
	{
		xRotInput = 20;
        //Camera.main.transform.SetParent(transform); Keep this around pl0x
        //Camera.main.transform.localPosition = new Vector3(0, 2, -6);
        //Camera.main.transform.rotation = new Quaternion(20, 0, 0, 0);
	}

	void FixedUpdate ()
	{
		if(canMove == true) //&& PhotonNetwork.player.IsLocal && GetComponent<PrototypeSoldier>().myTeam == PhotonNetwork.player.ID
        {

			SoldierMovement();
			SoldierRotation();
		}
	}


	public void SoldierMovement()
	{
		float xInput = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float zInput = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		Vector3 move = new Vector3(xInput , 0 ,zInput);
		transform.Translate(move);
	}

	public void SoldierRotation()
	{
		float yRotInput = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
		transform.Rotate(0, yRotInput, 0);

		xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
		print (xRotInput);
		xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
		Camera.main.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
	}

}
