using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour {

	private Animator animator;
	public bool activateIK;
	private Transform leftHandObj;
	private Transform rightHandObj;

	void Start() 
	{
		animator = transform.root.GetComponent<Animator>();
		
	}

	void OnAnimatorIK(int layerIndex)
    {
		if(GetComponent<Soldier>())
		{
			if(GetComponent<Soldier>().equippedWeapon.iKPositionLeft)
			{
				leftHandObj = GetComponent<Soldier>().equippedWeapon.iKPositionLeft;
			}
			else
			{
				leftHandObj = null;
			}

			if(GetComponent<Soldier>().equippedWeapon.iKPositionRight)
			{
				rightHandObj = GetComponent<Soldier>().equippedWeapon.iKPositionRight;
			}
			else
			{
				rightHandObj = null;
			}
		}
		print("I");
		if(animator)
		{
			print("Am");
			if(activateIK)
			{
				print("Extremely");
				if(leftHandObj != null) 
				{
					print("Gay");
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand,leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand,leftHandObj.rotation);
                }       

				// if(rightHandObj != null) 
				// {
				// 	print("Gay");
                //     animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                //     // animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);
                //     animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandObj.position);
				// 	// Quaternion handRot = Quaternion.Euler(0,0,rightHandObj.rotation.z);
                //     // animator.SetIKRotation(AvatarIKGoal.RightHand,handRot);
                // }         
			}
			else 
			{          
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0); 

				// animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                // animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
            }
		}
	}
}
