using UnityEngine;

public class MachineGunPickup : MonoBehaviour,
    IInteractable
{
    [Header("WEAPON PREFAB")]
    public MachineGunWeapon weaponPrefab;

    public bool destroyOnPickup = true;

    private bool pickedUp;

    public bool CanInteract()
    {
        return !pickedUp;
    }

    public void Interact()
    {
        if (pickedUp)
            return;

        if (weaponPrefab == null)
        {
            Debug.LogWarning(
                "Weapon Prefab Missing");
            return;
        }

        if (WeaponManager.Instance == null)
        {
            Debug.LogWarning(
                "WeaponManager Missing");
            return;
        }

        // EQUIP WEAPON
        WeaponManager.Instance
            .EquipWeapon(weaponPrefab);

        pickedUp = true;

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}