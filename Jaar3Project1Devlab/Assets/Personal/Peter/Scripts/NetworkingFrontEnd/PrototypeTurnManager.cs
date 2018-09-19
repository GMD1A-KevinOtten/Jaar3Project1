using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeTurnManager : Photon.PunBehaviour {



    public static PrototypeTurnManager instance;
    // Use this for initialization
    void Awake() {

        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }



    }

    private void Start()
    {


    }

    // Update is called once per frame
    void Update() {
        //if(currentPlayer == PhotonNetwork.player)
        // {
        //     if (Input.GetKeyDown("n"))
        //     {
        //         CallNextTurn();
        //     }
        //     if (Input.GetKeyDown("c"))
        //     {
        //         PhotonNetwork.Instantiate("Koob", new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5)), Quaternion.identity, 0);
        //     }
        // }
    }

}





