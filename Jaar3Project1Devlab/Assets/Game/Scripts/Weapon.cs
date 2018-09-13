using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [Header("Gun proporties")]
    public Transform barrelExit;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public Vector2 bulletSpread;
    public float recoil;

    [Header("Clip proporties")]
    public int currentClip;
    public int clipMax;

    public void ShootBullet()
    {
        GameObject newGameObject = Instantiate(bulletPrefab, barrelExit.position, bulletPrefab.transform.rotation);
        Rigidbody rb = newGameObject.GetComponent<Rigidbody>();
        Vector2 spread = CalculatedBulletSpread();
        rb.velocity = new Vector3(spread.x, spread.y, bulletVelocity);
    }

    private Vector2 CalculatedBulletSpread()
    {
        float x = Random.Range(bulletSpread.x, -bulletSpread.x);
        float y = Random.Range(bulletSpread.y, -bulletSpread.y);

        Vector2 ToReturn = new Vector2(x, y);

        return ToReturn;
    }
}
