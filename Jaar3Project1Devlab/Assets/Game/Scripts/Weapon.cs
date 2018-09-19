using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [Header("Gun proporties")]
    public Transform barrelExit;
    public GameObject bulletPrefab;
    public Sprite gunCrosshair;
    public LayerMask crosshairRayMask;
    public float bulletVelocity;
    public Vector2 bulletSpread;
    public float recoil;

    [Header("Clip proporties")]
    public int currentClip;
    public int clipMax;

    private RaycastHit hit;

    public void ShootBullet()
    {
        GameObject newGameObject = Instantiate(bulletPrefab, barrelExit.position, bulletPrefab.transform.rotation);
        Rigidbody rb = newGameObject.GetComponent<Rigidbody>();
        Vector2 spread = CalculatedBulletSpread();

        Vector3 bulletDirection = barrelExit.forward * bulletVelocity;
        bulletDirection.x += spread.x;
        bulletDirection.y += spread.y;
        rb.velocity = bulletDirection;
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
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
        Physics.Raycast(transform.position, transform.forward, out hit, 20, crosshairRayMask);

        if (hit.transform != null)
        {
            UIManager.instance.ShowCrosshairOnScreen(gunCrosshair, hit.point);
        }
        else
        {
            UIManager.instance.HideCrosshair();
        }
    }
}
