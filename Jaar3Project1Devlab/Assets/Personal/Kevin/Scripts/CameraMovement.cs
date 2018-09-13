using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	
	private float xRotInput;

	public float camMovSpeed;
	public float camRotSpeed;

	public bool cameraState;
	public float clampValue;
	public float vertRotSpeed;

	void Start()
	{
		xRotInput = 20;
	}

	void FixedUpdate () 
	{
		if(cameraState == true)
		{
			TopVieuwCamera();
		}

		else
		{
			SoldierCamera();
		}
	}

	public void TopVieuwCamera()
	{

		float horMovement = Input.GetAxis("Horizontal") * Time.deltaTime * camMovSpeed;
		float vertMovement = Input.GetAxis("Vertical") * Time.deltaTime * camMovSpeed;

		Vector3 toMove = new Vector3(horMovement, 0, vertMovement);
		transform.Translate(toMove, Space.World);

		if(Input.GetButton("Q"))
		{
			transform.Rotate(0,-camRotSpeed * Time.deltaTime,0, Space.World);
		}

		if(Input.GetButton("E"))
		{
			transform.Rotate(0,camRotSpeed * Time.deltaTime,0, Space.World);
		}
	}

	public void SoldierCamera()
	{
		xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
		xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
		Camera.main.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
	}
}
