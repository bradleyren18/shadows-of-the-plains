using System.Collections;
using ItemTypeNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float health = 20;
    public Renderer renderer;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image armourType;
    [SerializeField]
    private bool hasHealthBar = true;
    [SerializeField]
    private bool hasArmourImage = true;
    [SerializeField]
    private GameObject leatherHelmet;
    [SerializeField]
    private GameObject ironHelmet;
    [SerializeField]
    private GameObject diamondHelmet;
    [SerializeField]
    private GameObject netheriteHelmet;
    private ItemType helmet;
    private float damageMultiplier = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();

        ItemType[] helmets =
        {
            ItemType.LeatherHelmet,
            ItemType.IronHelmet,
            ItemType.DiamondHelmet,
            ItemType.NetheriteHelmet
        };

        helmet = helmets[Random.Range(0, helmets.Length)];
        CalcArmourPoints();
    }

    // Update is called once per frame
    void Update() { }

    public void OnTakeDamage(float damage)
    {
        FlashRed();
        health -= damage * damageMultiplier;
        if (hasHealthBar)
        {
            RectTransform healthTransform = healthBar.GetComponent<RectTransform>();
            healthTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * 25);
            healthTransform.anchoredPosition = new Vector2(
                1000 + (healthTransform.rect.width / 2),
                -750
            );
        }
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy(transform.gameObject);
            Debug.Log(transform.gameObject.name + "died");
        }
    }

    void CalcArmourPoints()
    {
        switch (helmet)
        {
            case ItemType.LeatherHelmet:
                damageMultiplier = 0.85f;
                if (hasArmourImage)
                {
                    armourType.color = Color.saddleBrown;
                }
                EquipHelmet(leatherHelmet);
                break;
            case ItemType.IronHelmet:
                damageMultiplier = 0.70f;
                if (hasArmourImage)
                {
                    armourType.color = Color.grey;
                }
                EquipHelmet(ironHelmet);
                break;
            case ItemType.DiamondHelmet:
                damageMultiplier = 0.55f;
                if (hasArmourImage)
                {
                    armourType.color = Color.lightSkyBlue;
                }
                EquipHelmet(diamondHelmet);
                break;
            case ItemType.NetheriteHelmet:
                damageMultiplier = 0.40f;
                if (hasArmourImage)
                {
                    armourType.color = Color.black;
                }
                EquipHelmet(netheriteHelmet);
                break;
        }
    }

    void EquipHelmet(GameObject helmetPrefab)
    {
        GameObject helmet = Instantiate(
            helmetPrefab,
            transform.position + Vector3.up * .8f,
            Quaternion.identity
        );
        helmet.transform.SetParent(transform);
        Debug.Log("helmet equipped");
    }

    public void FlashRed()
    {
        StartCoroutine(FlashRedCoroutine());
    }

    IEnumerator FlashRedCoroutine()
    {
        renderer.material.color = Color.red; // Turn red
        yield return new WaitForSeconds(0.25f); // Wait 1 second
        renderer.material.color = Color.white; // Turn white
    }
}
