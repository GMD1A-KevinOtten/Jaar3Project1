using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	
	public bool cameraState;

	private float xRotInput;
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

	}

	public void SoldierCamera()
	{
		xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
		print (xRotInput);
		xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
		Camera.main.transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
	}
}
