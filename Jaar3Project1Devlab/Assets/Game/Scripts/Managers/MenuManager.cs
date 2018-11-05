using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public void Start()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void LoadScene(int i)
    {
        GameManager.Instance.ChangeScene(i);
    }
}
