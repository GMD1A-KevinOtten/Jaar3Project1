using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	
	public float moveSpeed;
	public float horRotSpeed;
	public bool canMoveAndRotate;
    public bool canMove;
    private Soldier soldier;

    private void Awake()
    {
        soldier = GetComponent<Soldier>();
        canMove = true;
    }

    void FixedUpdate ()
	{
		if(canMoveAndRotate == true) 
        {
            if(canMove == true)
            {
                SoldierMovement();
            }
            SoldierRotation();
		}
	}


	public void SoldierMovement()
	{
		float xInput = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float zInput = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		Vector3 move = new Vector3(xInput , 0 ,zInput);
        soldier.SetMoveAnimation(move);
        transform.Translate(move);
	}

	public void SoldierRotation()
	{
		float yRotInput = Input.GetAxis("Mouse X") * Time.deltaTime * horRotSpeed;
		transform.Rotate(0, yRotInput, 0);
	}
    public void ChangeCanMove()
    {
        if (!canMoveAndRotate)
        {
            canMoveAndRotate = true;
        }
        else
        {
            canMoveAndRotate = false;
        }
    }

}
