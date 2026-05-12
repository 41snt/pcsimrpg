using UnityEngine;

public class MachineGunWeapon : MonoBehaviour
{
    [Header("GUN")]
    public GameObject bulletPrefab;

    public Transform firePoint;

    public float fireRate = 0.08f;

    public float bulletSpeed = 20f;

    public int damage = 10;

    private float nextShootTime;

    public void Shoot(Vector2 direction)
    {
        if (Time.time < nextShootTime)
            return;

        nextShootTime =
            Time.time + fireRate;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity);

        Rigidbody2D rb =
            bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity =
                direction.normalized
                * bulletSpeed;
        }

        MachineGunBullet bulletScript =
            bullet.GetComponent<MachineGunBullet>();

        if (bulletScript != null)
        {
            bulletScript.damage = damage;
        }

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x)
            * Mathf.Rad2Deg;

        bullet.transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle);
    }
}