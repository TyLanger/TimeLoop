using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int clipSize = 10;
    int currentBulletsInClip;
    [SerializeField] float reloadTime = 1;
    bool reloading = false;
    [SerializeField] float timeBetweenShots = 0.5f;
    float timeOfNextFire;

    [SerializeField] Projectile bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] Transform bulletSpawn;

    void Start()
    {
        currentBulletsInClip = clipSize;
        timeOfNextFire = 0;
    }

    public bool Shoot()
    {
        if(CanShoot())
        {
            currentBulletsInClip--;
            timeOfNextFire = Time.time + timeBetweenShots;
            Projectile copy = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            copy.Setup(bulletSpeed, bulletDamage);
            return true;
        }
        return false;
    }

    bool CanShoot()
    {
        return (Time.time > timeOfNextFire) && (currentBulletsInClip > 0) && !reloading;
    }

    bool CanReload()
    {
        return !reloading && currentBulletsInClip < clipSize;
    }

    public bool StartReload()
    {
        if(CanReload())
        {
            StartCoroutine(Reload());
            return true;
        }
        return false;
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentBulletsInClip = clipSize;
        reloading = false;
    }
}
