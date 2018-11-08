using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum WeaponKind
    {
        Light,
        Heavy,
    }

    [Header("Gun proporties")]
    public int gunID;
    public WeaponKind weaponKind;
    public Transform barrelExit;
    public GameObject bulletPrefab;
    public Sprite gunCrosshair;
    public Sprite weaponSprite;
    public LayerMask crosshairRayMask;
    public float bulletVelocity;
    public Vector2 bulletSpread;
    public Soldier mySoldier;
    public float recoil;
    public bool specialFunctionality;
    public Transform iKPositionLeft;
    public Transform iKPositionRight;

    public bool isTank;
    [Header("Gun Sound Effects")]
    public CustomAudioClip shotSound;
    public CustomAudioClip[] reloadSounds;

    [Header("Clip proporties")]
    public int currentClip;
    public int clipMax;

    private RaycastHit hit;
    public GameObject newGameObject;

    private bool reloadTutorial;

    public virtual void Start() 
    {
        FillClip();
    }

    public virtual void Update() 
    {
        if(mySoldier != null)
        {
            if(mySoldier.isActive == true)
            {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");

                if(x == 0 && y == 0)
                {
                    Inputs();
                }
            }
            else if(!mySoldier.isActive && isTank)
            {
                Inputs();
            }
        }
    }

    public virtual void Inputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(mySoldier.canShoot == true)
            {
                ShootBullet();
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            if(mySoldier.canShoot != true && mySoldier.canSwitch == true)
            {
                if(currentClip != 0)
                {
                    mySoldier.CombatToggle();
                }
            }
        }
        if(Input.GetButtonDown("R"))
        {
            if(mySoldier.canShoot != true)
            {
                if(currentClip != clipMax)
                {
                    Reload();
                }
            }
        }
    }

    public virtual void Death()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        transform.SetParent(null);
    }

    public virtual void SpecialFunctionalityToggle()
    {
        if(specialFunctionality == false)
        {
            specialFunctionality = true;
        }
        else
        {
            specialFunctionality = false;
        }
    }

    public virtual void ShootBullet()
    {
        if(currentClip > 0)
        {
            EffectsManager.instance.PlayAudio3D(shotSound,transform.position);
            EffectsManager.instance.PlayParticle(EffectsManager.instance.FindParticle("Muzzle Flash"), barrelExit.position, barrelExit.forward);
            print("Set");
            mySoldier.anim.SetTrigger("Shoot");

            newGameObject = Instantiate(bulletPrefab, barrelExit.position, barrelExit.rotation);
            currentClip -= 1;
                    
            Rigidbody rb = newGameObject.GetComponent<Rigidbody>();
            Vector2 spread = CalculatedBulletSpread();

            Vector3 bulletDirection = barrelExit.forward * bulletVelocity;
            bulletDirection.x += spread.x;
            bulletDirection.y += spread.y;
            rb.velocity = bulletDirection;
        }
        else
        {
            if (!reloadTutorial)
            {
                UIManager.instance.ShowMessageOnUI("Press R to reload the weapon.", 5);
                reloadTutorial = true;
            }
            EffectsManager.instance.PlayAudio3D(EffectsManager.instance.FindAudioClip("Empty Clip"), transform.position);
        }
    }

    public Vector2 CalculatedBulletSpread()
    {
        float x = Random.Range(bulletSpread.x, -bulletSpread.x);
        float y = Random.Range(bulletSpread.y, -bulletSpread.y);

        Vector2 ToReturn = new Vector2(x, y);

        return ToReturn;
    }

    public void ShowCrosshair()
    {
        Debug.DrawRay(barrelExit.position, barrelExit.forward * 100, Color.red);
        Physics.Raycast(barrelExit.position, barrelExit.forward, out hit, 100, crosshairRayMask);

        if (hit.transform != null)
        {
            float scale = Mathf.Clamp(100 / Vector3.Distance(transform.position,hit.point) * 2 , 60 , 160);
            UIManager.instance.crosshairImage.GetComponent<RectTransform>().sizeDelta = new Vector2(scale,scale);
            UIManager.instance.ShowCrosshairOnScreen(gunCrosshair, hit.point);
        }
        else
        {
            UIManager.instance.HideCrosshair();
        }
    }

    public virtual void Reload()
    {
        mySoldier.DisableMovementAnimation();
        FillClip();
        mySoldier.canSwitch = false;
        mySoldier.soldierMovement.canMove = false;
        mySoldier.anim.SetTrigger("Reload");
        StartCoroutine(AferReloadTeamSwitch(5));
    }

    private void OnEnable() 
    {
        if(mySoldier != null)
        {
            if(weaponKind == WeaponKind.Heavy)
            {
                mySoldier.anim.SetBool("BigGun", true);
            }
            else
            {
                mySoldier.anim.SetBool("BigGun", false);
            }
        }
    }

    public void FillClip()
    {
        currentClip = clipMax;
    }

    public virtual IEnumerator AferReloadTeamSwitch(float time)
    {
        yield return new WaitForSeconds(time);
        if(TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Topview || TeamManager.instance.mainCamera.cameraState != CameraMovement.CameraStates.Idle)
        {
            if(TeamManager.instance.mainCamera.cameraState == CameraMovement.CameraStates.CombatVieuw)
            {
                mySoldier.CombatToggle();
            }
            mySoldier.canSwitch = true;
            mySoldier.soldierMovement.canMove = true;
            TeamManager.instance.lastTeamIndex = TeamManager.instance.teamIndex;
            TeamManager.instance.NextTeam();
        }
        else
        {
            yield return null;
        }
    }
}
