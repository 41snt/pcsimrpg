using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    [Header("CURRENT WEAPON")]
    public MachineGunWeapon equippedWeapon;

    [Header("WEAPON HOLDER")]
    public Transform weaponHolder;

    private GameObject currentWeaponObject;

    private void Awake()
    {
        Instance = this;
    }

    // =========================
    // EQUIP WEAPON
    // =========================

    public void EquipWeapon(
        MachineGunWeapon weaponPrefab)
    {
        if (weaponPrefab == null)
            return;

        // REMOVE OLD WEAPON
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        // CREATE NEW WEAPON
        currentWeaponObject =
            Instantiate(
                weaponPrefab.gameObject,
                weaponHolder);

        currentWeaponObject.transform.localPosition =
            Vector3.zero;

        currentWeaponObject.transform.localRotation =
            Quaternion.identity;

        equippedWeapon =
            currentWeaponObject
            .GetComponent<MachineGunWeapon>();

        Debug.Log(
            "Equipped Weapon: "
            + equippedWeapon.name);
    }

    // =========================
    // SHOOT
    // =========================

    public void Shoot(Vector2 direction)
    {
        if (equippedWeapon == null)
            return;

        equippedWeapon.Shoot(direction);
    }
}