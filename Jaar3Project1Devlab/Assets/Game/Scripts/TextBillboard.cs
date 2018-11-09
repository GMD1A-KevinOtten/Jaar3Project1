using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBillboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().transform.rotation = Camera.main.transform.rotation;
        }
    }

    //public IEnumerator Die()
    //{
    //    yield return new WaitForSeconds(2);
    //    Destroy(gameObject);
    //}

}
