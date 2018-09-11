using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	
	public float moveSpeed;
	public float rotSpeed;

	private float yRotInput;
	public float clampValue;

	public bool canMove;

	void FixedUpdate ()
	{
		if(canMove == true)
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
		float xRotInput = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
		transform.Rotate(0, xRotInput, 0);

		yRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
		print (yRotInput);
		yRotInput = Mathf.Clamp(yRotInput, -clampValue, clampValue);
		Camera.main.transform.localRotation = Quaternion.Euler(yRotInput, 0, 0);
	}

}
