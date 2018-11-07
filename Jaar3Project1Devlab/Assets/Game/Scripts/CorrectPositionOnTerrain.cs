using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectPositionOnTerrain : MonoBehaviour {

    private RaycastHit hit;

    public void Awake()
    {
        if(Physics.Raycast(transform.position + new Vector3(0, 5, 0), -Vector3.up, out hit, 10))
        {
            if (hit.transform.GetComponent<Terrain>() != null)
            {
                print(transform.gameObject.name + ": " + hit.point);
                transform.position = hit.point;
                transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            }
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), -Vector3.up * 10, Color.black);
    }

}
