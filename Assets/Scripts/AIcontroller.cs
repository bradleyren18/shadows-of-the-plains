using System.Collections;
using UnityEngine;

public class AIcontroller : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    public float moveSpeed = 3f;
    public float aiDamage = 5f;
    public Renderer aiRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aiRenderer = GetComponentInChildren<Renderer>();
        StartCoroutine(jump());
    }

    void FixedUpdate() // Use FixedUpdate for physics updates
    {
        if (transform.GetComponent<HealthScript>().health >= 10)
        {
            Vector3 lookPosition = player.transform.position;
            lookPosition.y = transform.position.y;
            transform.LookAt(lookPosition);

            // Move forward while preserving gravity
            Vector3 forward = transform.forward * moveSpeed;
            rb.linearVelocity = new Vector3(forward.x, rb.linearVelocity.y, forward.z);

            if (Vector3.Distance(transform.position, player.transform.position) <= 2)
            {
                player.transform.position += Vector3.up * 10;
                transform.GetComponent<AIWeaponScript>().TryAttack();
            }
        }
        else
        {
            Vector3 lookPosition = player.transform.position;
            lookPosition.y = transform.position.y;
            transform.LookAt(-lookPosition);

            // Move forward while preserving gravity
            Vector3 forward = transform.forward * moveSpeed;
            rb.linearVelocity = new Vector3(forward.x, rb.linearVelocity.y, forward.z);

            if (Vector3.Distance(transform.position, player.transform.position) <= 2)
            {
                player.transform.position += Vector3.up * 10;
                transform.GetComponent<AIWeaponScript>().TryAttack();
            }
        }
    }

    IEnumerator jump()
    {
        transform.position += Vector3.up * 2;
        yield return new WaitForSeconds(2);
    }
}