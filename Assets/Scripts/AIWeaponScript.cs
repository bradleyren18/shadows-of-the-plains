using ItemTypeNamespace;
using UnityEngine;

public class AIWeaponScript : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 2f;

    [Header("Weapon Settings")]
    public ItemType weapon;
    public GameObject woodenSword;
    public GameObject ironSword;
    public GameObject diamondSword;
    public GameObject netheriteSword;

    public Transform hand; // where weapon is held
    public Transform attackOrigin; // e.g., AI's head for raycasting

    private float damage;

    void Start()
    {
        AssignRandomWeapon();
    }

    void AssignRandomWeapon()
    {
        ItemType[] swords =
        {
            ItemType.WoodenSword,
            ItemType.IronSword,
            ItemType.DiamondSword,
            ItemType.NetheriteSword
        };

        weapon = swords[Random.Range(0, swords.Length)];
        SetWeaponStats(weapon);
    }

    void SetWeaponStats(ItemType weaponType)
    {
        switch (weaponType)
        {
            case ItemType.WoodenSword:
                damage = 4;
                EquipSword(woodenSword, hand);
                break;
            case ItemType.IronSword:
                damage = 6;
                EquipSword(ironSword, hand);
                break;
            case ItemType.DiamondSword:
                damage = 7;
                EquipSword(diamondSword, hand);
                break;
            case ItemType.NetheriteSword:
                damage = 8;
                EquipSword(netheriteSword, hand);
                break;
        }
    }

    void EquipSword(GameObject swordPrefab, Transform hand1)
    {
        GameObject sword = Instantiate(
            swordPrefab,
            hand1.transform.position + hand1.transform.forward * 0.5f + Vector3.up * 0.5f,
            hand1.transform.rotation
        );
        sword.transform.SetParent(hand1.transform);
        sword.transform.localScale = new Vector3(50, 20, 5);
        Debug.Log("sword equipped");
    }

    public void TryAttack()
    {
        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            HealthScript targetHealth = hit.collider.GetComponent<HealthScript>();
            if (targetHealth != null)
            {
                targetHealth.OnTakeDamage(damage);
                targetHealth.FlashRed();
            }
        }
    }
}
