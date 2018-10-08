using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meme : MonoBehaviour {
    public AudioSource sauce;
	// Use this for initialization
	void Start () {
        sauce.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "Kevin Otten")
        {
            sauce.enabled = true;
        }
	}
}
