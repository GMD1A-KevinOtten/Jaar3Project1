using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	
	public float moveSpeed;
	public float rotSpeed;

	private float xRotInput;
	public float clampValue;

	public bool canMove;

	void Start()
	{
		xRotInput = 20;
	}

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
		float yRotInput = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
		transform.Rotate(0, yRotInput, 0);

		xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
		print (xRotInput);
		xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
		Camera.main.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
	}

}
