using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public RaycastHit hit;
    public Image image;
    public Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
        Physics.Raycast(transform.position, transform.forward, out hit, 20);

        image.transform.position = cam.WorldToScreenPoint(hit.point);
	}
}
