using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIntSync : MonoBehaviour {

    public int currentPlayerTurn = 1;

    public static TurnIntSync instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void OnPhotonSerializeView()
    {

    }
}
