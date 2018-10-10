using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public Camera faceToCam;

    private void Start() 
    {
        if(faceToCam == null)
        {
            faceToCam = Camera.main;
        }
    }

    private void Update()
    {
        transform.LookAt(faceToCam.transform);
    }
}
