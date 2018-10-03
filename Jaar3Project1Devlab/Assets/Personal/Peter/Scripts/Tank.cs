using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : InteractableObject {
    public float sphereRadius = 5;
    public Transform soldierInsidePos;

    public Camera barrelCam;
    public GameObject turret;
    public GameObject barrel;
    public float barrelRotationSpeed;

    public float screenBoundary;
    private Vector3 soldierOutsidePos;
    private Soldier currentSoldier;
    private bool soldierInside;

    [Header("Clamp properties")]
    public bool clamp;
    public float clampX;
    public float clampY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Interact();

        if(TeamManager.instance.turnTime <= .1F)
        {
            if (soldierInside)
            {
                ExitTank();
            }
        }

        if (Input.GetButtonDown("Enter"))
        {
            if (soldierInside)
            {
                ExitTank(); 
            }
        }
	}

    private void FixedUpdate()
    {
        if (soldierInside)
        {
            Movement();
        }
    }

    void Movement()
    {
        Vector2 mousePos = Input.mousePosition;

        if(mousePos.x > Screen.width - screenBoundary)
        {
            //Right side
            turret.transform.Rotate(transform.up * barrelRotationSpeed * Time.deltaTime);
        }
        if (mousePos.x < 0 + screenBoundary)
        {
            //Left side
            turret.transform.Rotate(-transform.up * barrelRotationSpeed * Time.deltaTime);
        }
        if (mousePos.y > Screen.height - screenBoundary)
        {
            //Top side
            barrel.transform.Rotate(transform.up * barrelRotationSpeed * Time.deltaTime);
        }
        if (mousePos.y < 0 + screenBoundary)
        {
            //Bottom side
            barrel.transform.Rotate(-transform.up * barrelRotationSpeed * Time.deltaTime);
        }

    }

    public override void Interact()
    {
        if (soldierNearby())
        {
            if (Input.GetKeyDown("e"))
            {
                soldierOutsidePos = currentSoldier.gameObject.transform.position;
                currentSoldier.gameObject.transform.position = soldierInsidePos.position;
                currentSoldier.isActive = false;
                currentSoldier.soldierMovement.canMove = false;

                Camera.main.enabled = false;
                barrelCam.enabled = true;
                soldierInside = true;

                
            }
        }
    }

    public void ExitTank()
    {
        currentSoldier.gameObject.transform.position = soldierOutsidePos;
        currentSoldier.isActive = true;
        currentSoldier.soldierMovement.canMove = true;

        TeamManager.instance.mainCamera.GetComponent<Camera>().enabled = true;
        barrelCam.enabled = false;
        soldierInside = false;
    }

    public bool soldierNearby()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, sphereRadius);
        foreach (Collider c in cols)
        {
            if (c.GetComponentInParent<Soldier>())
            {
                if (c.GetComponentInParent<Soldier>().isActive)
                {
                    currentSoldier = c.GetComponentInParent<Soldier>();
                    return true;
                }
            }
        }

        return false;
    }
}
