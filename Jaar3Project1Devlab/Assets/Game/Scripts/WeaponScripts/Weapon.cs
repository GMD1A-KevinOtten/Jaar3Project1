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
    public LayerMask crosshairRayMask;
    public float bulletVelocity;
    public Vector2 bulletSpread;
    public float recoil;

    public bool isTank;
    [Header("Gun Sound Effects")]
    public CustomAudioClip shotSound;
    public CustomAudioClip[] reloadSounds;

    [Header("Clip proporties")]
    public int currentClip;
    public int clipMax;

    private RaycastHit hit;
    private GameObject newGameObject;


    public virtual void Start() 
    {
        FillClip();
    }

    public virtual void Update() 
    {
        if(gameObject.transform.root.GetComponent<Soldier>() != null)
        {
            if(transform.root.GetComponent<Soldier>().isActive == true)
            {
                Inputs();
            }
        }
    }

    public virtual void Inputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
    }

    public virtual void ShootBullet()
    {
        if(currentClip > 0)
        {
            //play shot sound

            newGameObject = Instantiate(bulletPrefab, barrelExit.position, bulletPrefab.transform.rotation);
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
            // play empty mag klick
        }
    }

    private Vector2 CalculatedBulletSpread()
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

    public virtual void Reload(int reloadSoundIndex)
    {
        if (currentClip != clipMax)
        {
            AudioManager.instance.PlayAudio2D(reloadSounds[reloadSoundIndex]);
            FillClip();
            TeamManager.instance.lastTeamIndex = TeamManager.instance.teamIndex;
            TeamManager.instance.NextTeam();
        }
        else
        {
            //whatever de fuck we willen als reloaden waardenloos is aka max ammo in geweer
        }
    }

    private void FillClip()
    {
        currentClip = clipMax;
    }
}
