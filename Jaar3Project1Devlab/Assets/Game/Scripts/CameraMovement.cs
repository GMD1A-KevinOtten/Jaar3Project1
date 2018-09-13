using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public float camMovSpeed;
	public float camRotSpeed;

	public bool topview;
	public float clampValue;
	public float vertRotSpeed;

    private float xRotInput;

    void Start()
	{
		xRotInput = 20;
	}

	void FixedUpdate () 
	{
		if(topview == true)
		{
			TopViewCamera();
		}

		else
		{
			SoldierCamera();
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

    /// <summary>
    /// Call this function when the camera is in third person mode
    /// </summary>
	public void SoldierCamera()
	{
		xRotInput -= Input.GetAxis("Mouse Y") * Time.deltaTime * vertRotSpeed;
		xRotInput = Mathf.Clamp(xRotInput, -clampValue, clampValue);
		transform.localRotation = Quaternion.Euler(xRotInput, 0, 0);
	}
}
