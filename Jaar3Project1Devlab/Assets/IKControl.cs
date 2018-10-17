using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControl : MonoBehaviour {

	private Animator animator;
	public bool activateIK;

	void Start() 
	{
		animator = GetComponent<Animator>();
	}

	void OnAnimatorIK()
    {
		if(animator)
		{
			if(activateIK)
			{
				
			}
		}
	}
}
