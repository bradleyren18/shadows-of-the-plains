using System.Collections;
using UnityEngine;
using ItemTypeNamespace;

public class WeaponScript : MonoBehaviour
{
    [Header("Attack Settings")]
    private float damage;
    private ItemType weapon;

    [Header("Sword Prefabs")]
    [SerializeField]
    private GameObject woodenSword;
    [SerializeField]
    private GameObject ironSword;
    [SerializeField]
    private GameObject diamondSword;
    [SerializeField]
    private GameObject netheriteSword;

    [Header("References")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private GameObject hand1;

    void Start()
    {
        AssignRandomWeapon();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            HandleAttack();
        }
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
        CalcDamagePoints();
    }

    public void HandleAttack()
    {
        RotateHand(hand1);
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5))
        {
            HealthScript AIHealth = hit.collider.GetComponent<HealthScript>();
            if (AIHealth != null)
            {
                AIHealth.OnTakeDamage(damage);
                AIHealth.FlashRed();
            }
        }
    }

    void CalcDamagePoints()
    {
        switch (weapon)
        {
            case ItemType.WoodenSword:
                damage = 4;
                EquipSword(woodenSword);
                break;
            case ItemType.IronSword:
                damage = 6;
                EquipSword(ironSword);
                break;
            case ItemType.DiamondSword:
                damage = 7;
                EquipSword(diamondSword);
                break;
            case ItemType.NetheriteSword:
                damage = 8;
                EquipSword(netheriteSword);
                break;
        }
    }

    void EquipSword(GameObject swordPrefab)
    {
        GameObject sword = Instantiate(
            swordPrefab,
            hand1.transform.position + Vector3.up * 0.3f,
            hand1.transform.rotation
        );
        sword.transform.SetParent(hand1.transform);
        Debug.Log("sword equipped");
    }

    public void RotateHand(GameObject hand)
    {
        StartCoroutine(RotateHandCoroutine(hand));
    }

    IEnumerator RotateHandCoroutine(GameObject hand)
    {
        hand.transform.Rotate(30, 0, 0);
        yield return new WaitForSeconds(0.25f);
        hand.transform.Rotate(-30, 0, 0);
    }
}
