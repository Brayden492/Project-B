using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] int damage = 10;
    [SerializeField] float lifetime = 5f;

    Rigidbody rb;

    // self explanatory, grabs the rigid body component
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // shoots the projectile and destroys it after a set amount of time
    public void Launch(Vector3 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    // if projectile collides with player do damage if possible
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Destroy the projectile when it hits something.
        Destroy(gameObject);
    }
}

