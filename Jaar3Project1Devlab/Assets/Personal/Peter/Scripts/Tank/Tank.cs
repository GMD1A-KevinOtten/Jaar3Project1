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
    public bool soldierInside;

    private Weapon previousWeapon;

    [Header("Clamp properties")]
    public bool clamp;
    public float clampX;
    public float clampY;
    private float xRotInput;
    private float yRotInput;
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

        if (Input.GetKeyDown("k"))
        {
            if(Cursor.lockState != CursorLockMode.Locked && Cursor.visible == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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

        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        if (!clamp)
        {
            turret.transform.Rotate(transform.up * h * barrelRotationSpeed * Time.deltaTime);
            barrel.transform.Rotate(transform.up * v * barrelRotationSpeed * Time.deltaTime);
        }
        else
        {
            xRotInput += Input.GetAxis("Mouse X") * Time.deltaTime * barrelRotationSpeed;
            xRotInput = Mathf.Clamp(xRotInput, -clampX, clampX);
            turret.transform.localRotation = Quaternion.Euler(0, xRotInput, 0);

            yRotInput += Input.GetAxis("Mouse Y") * Time.deltaTime * barrelRotationSpeed;
            yRotInput = Mathf.Clamp(yRotInput, -clampY, clampY);
            barrel.transform.localRotation = Quaternion.Euler(0, yRotInput, 0);

        }




    }

    public override void Interact()
    {
        if (soldierNearby())
        {
            if (Input.GetKeyDown("e"))
            {
                previousWeapon = currentSoldier.equippedWeapon;

                soldierOutsidePos = currentSoldier.gameObject.transform.position;
                currentSoldier.gameObject.transform.position = soldierInsidePos.position;
                currentSoldier.isActive = false;
                currentSoldier.soldierMovement.canMoveAndRotate = false;

                Camera.main.enabled = false;
                barrelCam.enabled = true;
                soldierInside = true;
                currentSoldier.equippedWeapon = GetComponentInChildren<Weapon>();

                
            }
        }
    }

    public void ExitTank()
    {
        currentSoldier.gameObject.transform.position = soldierOutsidePos;
        currentSoldier.isActive = true;
        currentSoldier.soldierMovement.canMoveAndRotate = true;

        TeamManager.instance.mainCamera.GetComponent<Camera>().enabled = true;
        barrelCam.enabled = false;

        currentSoldier.equippedWeapon = previousWeapon;
        previousWeapon = null;

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
